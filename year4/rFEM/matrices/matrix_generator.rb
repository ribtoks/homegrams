require File.expand_path(File.dirname(__FILE__)) + '/../geometry/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/complete_triangulator.rb'
require 'matrix'
require File.expand_path(File.dirname(__FILE__)) + '/matrix_plus.rb'

class MatrixGenerator
  class << self

    def generate_matrix(triangulator, conditions)

      # make sure, that going by indices
      # 0, 1, 2 in triangle we're going
      # counterclockwise
      triangulator.correct_all_triangles!
      
      # -------------------------------
      # ------- initialization --------
      # -------------------------------

      triangles = triangulator.triangles
      points_numbers = triangulator.vertices_hash

      bounds_hash = triangulator.bound_points

      n = triangulator.points_to_points.size

      m_matrix = Matrix.zero(n)
      f_vector = Array.new(n, 0)

      # ------------------------------
      # ----- simple algorigthm ------
      # ------------------------------

      # loop through all triangles
      triangles.each do |tr|

        matrices_and_vector = get_matrices_and_vector(tr,
                                                      bounds_hash,
                                                      conditions)
        matrices = matrices_and_vector[0]
        vector = matrices_and_vector[1]

        # make an array from points
        p_numbers = tr.points.map {|p| points_numbers[p]}

        3.times do |i|
          f_vector[ p_numbers[i] ] += vector[i, 0]
        end
        
        # loop thougt all matrices
        # and add them to big matrix
        # (position of element is calculated
        #  using it's number)
        matrices.each do |m|

          3.times do |i|
            3.times do |j|
              m_matrix[ p_numbers[i], p_numbers[j] ] += m[i, j]
            end
          end
          
        end # each matrix
        
      end # each triangle

      [m_matrix, f_vector]

    end

    private
    
    def get_matrices_and_vector(triangle, bounds_hash, conditions)

      bconditions = conditions.boundary_conditions
      a = conditions.a
      b = conditions.b
      d = conditions.d
      
      func = conditions.f
      
      matrices = []

      # intergrals on omega (inside region)
      matrices << get_k(triangle, a) unless a == 0
      matrices << get_k_simplified(triangle, b - a) unless a == b
      matrices << get_d_matrix(triangle, d) unless d == 0

      # generate f-vector
      fi = triangle.points.map {|p| func.call(p)}
      fi_vector = Matrix.columns([fi])
      md = get_d_matrix(triangle, 1)
      # mul matrix on vector
      f_vector = md * fi_vector

      # calculate bound intervals if needed
      
      has_no_bound_point = true

      # triangle bound points
      tr_bound_points = []

      tr_points = triangle.points
      tr_segments = []
      # save segment and
      # index of point that doesn't belong
      # to segment
      tr_segments << [tr_points[0], tr_points[1], 2]
      tr_segments << [tr_points[1], tr_points[2], 0]
      tr_segments << [tr_points[2], tr_points[0], 1]

      bound_segments = []

      # get all bound segments
      tr_segments.each_index do |i|
        segment = tr_segments[i]
        # if both points of a segment belong to a bound
        arr = [segment[0].hash, segment[1].hash].sort
        if bconditions.points_to_condindex.has_key?(arr)
          bound_segments << segment

          has_no_bound_point = false
        end
      end

      # return if our triangle has no bound points
      return [matrices, f_vector] if has_no_bound_point

      # else there can be two cases
      # when triangle has one side, that
      # is on omega bound (most of) and
      # with two sides

      # cykle through all segments and add matrices
      condindex_hash = bconditions.points_to_condindex
      
      bound_segments.each do |segment|
        arr = []
        arr << segment[0].hash
        arr << segment[1].hash

        # index of point, that is not on
        # current bound side
        unbound_point_index = segment[2]

        # get boundary conditions for current segment
        conditions_index = condindex_hash[arr.sort]
        ci = bconditions[conditions_index]

        # just coeficients before matrices
        coef1 = -ci.alpha / ci.lambda
        coef2 = ci.u_environment*ci.alpha/ci.lambda

        matrices << get_bounds_k(triangle, unbound_point_index, coef1)
        
        f_temp = get_bounds_k_simplified(triangle, unbound_point_index, coef2)
        3.times do |i|
          f_vector[i, 0] -= f_temp[i]
        end
        #TODO get one more matrix - not done yetx
        
      end
            
      [matrices, f_vector]
    end

    # the simpliest precomputed matrix
    def get_d_matrix(triangle, coef)
      delta = 2.0*triangle_square(triangle.points)

      d_matrix = Matrix.rows([
                              [2, 1, 1],
                              [1, 2, 1],
                              [1, 1, 2]
                             ])

      3.times do |i|
        3.times do |j|
          d_matrix[i, j] = d_matrix[i, j] * coef * delta / 24.0
        end
      end

      d_matrix
    end

    def get_bounds_k(triangle, unbound_point_index, coef)
      k_matrix = Matrix.zero(3)

      points = triangle.points
      
      line_points = []
      # get two points, that are on bound
      points.each_index do |i|
        line_points << Point.copy(points[i]) unless i == unbound_point_index
      end
      # get their length
      line_length = Edge.length line_points[0], line_points[1]

      # set diagonal elements
      3.times do |i|
        next if i == unbound_point_index

        val = 2.0/6.0
        k_matrix[i, i] = val*coef*line_length
      end

      i = 0
      while i < 3
        # continue if current point is not on bounds
        if i == unbound_point_index
          i += 1
          next
        end

        j = i + 1
        while j < 3
          # continue if current point is not on bounds
          if j == unbound_point_index
            j += 1
            next
          end
          
          val = 1.0/6.0
          k_matrix[i,j] = k_matrix[j,i] = val*coef*line_length
          
          j += 1
        end
        i += 1
      end

      k_matrix
    end

    def get_bounds_k_simplified(triangle, unbound_point_index, coef)
      ks_vector = Array.new(3, 0)
      
      points = triangle.points
      
      line_points = []

      points.each_index do |i|
        line_points << Point.copy(points[i]) unless i == unbound_point_index
      end

      line_length = Edge.length line_points[0], line_points[1]

      3.times do |i|
        next if i == unbound_point_index

        val = 1.0/2.0
        ks_vector[i] = val*coef*line_length
      end

      ks_vector
    end

    def get_k(triangle, coef)
      k_matrix = Matrix.zero(3)

      delta = 2.0*triangle_square(triangle.points)

      if (delta == 0)
        p triangle
        raise 'd'
      end
      
      ai = []
      bi = []
      ci = []

      3.times do |i|
        ai << get_a(triangle.points, i)
        bi << get_b(triangle.points, i)
        ci << get_c(triangle.points, i)
      end

      3.times do |i|
        val = bi[i]**2 + ci[i]**2
        k_matrix[i, i] = val*coef/(2.0*delta)
      end

      i = 0
      while i < 3
        j = i + 1
        while j < 3
          val = bi[i]*bi[j] + ci[i]*ci[j]
          k_matrix[i,j] = k_matrix[j,i] = val*coef/(2.0*delta)
          
          j += 1
        end
        i += 1
      end

      k_matrix
      
    end

    def get_k_simplified(triangle, coef)
      ks_matrix = Matrix.zero(3)

      delta = 2.0*triangle_square(triangle.points)

      ai = []
      bi = []
      ci = []

      3.times do |i|
        ai << get_a(triangle.points, i)
        bi << get_b(triangle.points, i)
        ci << get_c(triangle.points, i)
      end

      3.times do |i|
        val = ci[i]**2
        ks_matrix[i, i] = val*coef/(2.0*delta)
      end

      i = 0
      while i < 3
        j = i + 1
        while j < 3
          val = ci[i]*ci[j]
          ks_matrix[i,j] = ks_matrix[j,i] = val*coef/(2.0*delta)
          
          j += 1
        end
        i += 1
      end

      ks_matrix
      
    end

    def triangle_square(points)
      a = Edge.length(points[0], points[1])
      b = Edge.length(points[1], points[2])
      c = Edge.length(points[2], points[0])
      p = (a + b + c) / 2
      Math.sqrt(p*(p - a)*(p - b)*(p - c))
      #0.5*((b.x - a.x)*(c.y - a.y) - (c.y - a.y)*(b.y - a.y)).abs
    end

    def get_a(points, i)
      i1 = (i + 1) % points.size
      i2 = (i + 2) % points.size
      points[i1].x*points[i2].y - points[i1].y*points[i2].x
    end

    def get_b(points, i)
      i1 = (i + 1) % points.size
      i2 = (i + 2) % points.size
      points[i1].y - points[i2].y
    end

    def get_c(points, i)
      i1 = (i + 1) % points.size
      i2 = (i + 2) % points.size
      points[i2].x - points[i1].x
    end
    
  end
end
