require File.expand_path(File.dirname(__FILE__)) + '/../geometry/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../geometry/complete_triangulator.rb'
require 'matrix'

class Matrix
  def []=(i, j, x)
    @rows[i][j] = x
  end

  def dump(firstLine = "")
    str = ""
    if firstLine != ""
      str << firstLine << "\n"
    end
    for i in 0...self.row_size
      space = ""
      for j in 0...self.column_size
        str << space << self[i,j].to_s
        space = " "
      end
      str += "\n"
    end
    return str
  end
  
end


class MatricesCalculator
  class << self
    
    def get_matrix(triangle_index, triangulator, a, b, d, func)
      # get triangle for matrix calculating
      triangle = triangulator.triangles[triangle_index]

      delta = 2.0*triangle_square(triangle.points)

      ai = []
      bi = []
      ci = []

      3.times do |i|
        ai << get_a(triangle.points, i)
        bi << get_b(triangle.points, i)
        ci << get_c(triangle.points, i)
      end

      k_matrix = Matrix.zero(3)
      k2_matrix = Matrix.zero(3)

      3.times do |i|
        val = bi[i]**2 + ci[i]**2
        k_matrix[i, i] = val*a/(2.0*delta)

        val2 = ci[i]**2
        k2_matrix[i, i] = val2*a/(2.0*delta)
      end

      i = 0
      while i < 3
        j = i + 1
        while j < 3
          val = bi[i]*bi[j] + ci[i]*ci[j]
          k_matrix[i,j] = k_matrix[j,i] = val*a/(2.0*delta)

          val2 = ci[i]*ci[j]
          k2_matrix[i,j] = k2_matrix[j,i] = val2*(b - a)/(2.0*delta)
          
          j += 1
        end
        i += 1
      end

      m_matrix = Matrix.zero(3)
      3.times do |i|
        3.times do |j|
          m_matrix[i,j] = 1
        end
      end

      3.times do |i|
        m_matrix[i,i] = 2
      end

      3.times do |i|
        3.times do |j|
          m_matrix[i,j] *= delta/24.0
        end
      end

      puts k_matrix.dump
      puts "\n\n"
      puts k2_matrix.dump
      puts "\n\n"
      puts m_matrix.dump      
      
    end

    private
    
    def triangle_square(points)
      a = Edge.length(points[0], points[1])
      b = Edge.length(points[1], points[2])
      c = Edge.length(points[0], points[2])

      p = (a + b + c)/2
      Math.sqrt(p*(p - a)*(p - b)*(p - c))
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
