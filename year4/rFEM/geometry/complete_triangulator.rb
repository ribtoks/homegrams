require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/ad_triangulator.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon_decomposer.rb'
  
class Triangulator

  # initial polygon of triangulation
  attr_reader :initial_polygon
  attr :diameter, true
  # triangulation elements
  attr_reader :triangles
  # hash [point] -> [triangles which it belongs to]
  attr_reader :points_to_triangles
  # hash [point] -> [adjacent points]
  attr_reader :points_to_points
  # hash of bound points
  attr_reader :bound_points
  # hashes with indices
  attr_reader :triangles_hash
  attr_reader :vertices_hash
  
  def triangulate(input_polygon, diameter)
    @initial_polygon = Polygon.copy(input_polygon.points)
    decomposer = PolygonDecomposer.new
    polygon = Polygon.copy(input_polygon.points)
    
    polygons = decomposer.decompose polygon

    @bound_points = {}
    # calculate boundary points
    temp_triangulator = MonotonePolygonTriangulator.new(diameter, Polygon.copy(input_polygon.points))
    temp_triangulator.precalculate_polygon
    @bound_points.merge! temp_triangulator.get_bound_points
    
    @triangles = []

    # and now decompose and triangulate polygons
    polygons.each do |pol|
      triangulator = MonotonePolygonTriangulator.new(diameter, pol)
      # add triangles of current polygon
      @triangles.concat triangulator.triangulate
    end
    
    calculate_hashes

    @triangles
  end

  def provide_smoothing!
    smoothed_triangles = []
    @triangles.each do |tr|
      smoothed_triangles << Polygon.copy(tr.points)
    end
    
    @points_to_points.each do |key, value|
      # don't move boundary points
      next if @bound_points.has_key? key
      
      # calculate new point coordinates
      new_point = Polygon.get_center value
      
      # replace new value in all triangles
      @points_to_triangles[key].each do |i|
        smoothed_triangles[i].replace_point!(key, new_point)
      end
    end

    @triangles = smoothed_triangles
    calculate_hashes

    @triangles
  end


  def correct_all_triangles!

    @triangles.each do |tr|

      a = tr.points[0]
      b = tr.points[1]
      c = tr.points[2]

      next unless (c.classify(a, b) == :right)

      tr.points[0], tr.points[1] = tr.points[1], tr.points[0]
      
    end
    
  end
  

  def enumerate_triangles
    # find vertex with smallest degree
    min_angle = Math::PI*2.0
    vertex_index = -1
    @initial_polygon.points.each_index do |i|
      curr_angle = @initial_polygon.get_angle(i)

      # just usual minimum search
      if curr_angle < min_angle
        min_angle = curr_angle
        vertex_index = i
      end
    end

    # queue for bfs
    queue = []
    # hash of marked points
    marked = {}
    @triangles.each do |tr|
      tr.points.each do |p|
        # at first all vertices
        # are not marked
        marked[p] = false
      end
    end

    curr_point = @initial_polygon.points[vertex_index]
    queue.push curr_point
    marked[curr_point] = true
    #result_queue = [curr_point]

    curr_number = 0
    # hash [point] -> [it's number in enumeration]
    @vertices_hash = {}
    @vertices_hash[curr_point] = curr_number
    curr_number += 1

    # main bfs loop
    until queue.empty?
      point = queue.shift
      
      # push to queue all adjacent points
      # to current point
      @points_to_points[point].each do |p|
        next if marked[p]

        marked[p] = true
        queue.push p

        # assign to current point next nuber
        @vertices_hash[p] = curr_number
        curr_number += 1
        
        #result_queue.push p
      end
    end

    @vertices_hash    
  end
  
  private
  
  def calculate_hashes
    @points_to_points = Hash.new {|hash,key| hash[key] = []}
    @points_to_triangles = Hash.new {|hash,key| hash[key] = []}
    
    @triangles.each_index do |index|
      triangle = @triangles[index]
      
      triangle.points.each do |point|
        @points_to_triangles[point] << index
      end
      
      triangle.points.each do |p|
        triangle.points.each do |inn_p|
          next if inn_p == p
          
          @points_to_points[p] << inn_p
        end
        @points_to_points[p].sort!.uniq!
      end
    end
  end
  
end
