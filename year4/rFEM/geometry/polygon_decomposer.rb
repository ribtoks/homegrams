require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'

class PolygonDecomposer
  attr_reader :polygons

  def decompose(polygon)
    @polygons = []
    inner_decompose(polygon)
    @polygons
  end  
  
  private
  def get_distances(polygon)
    distances = []
    
    polygon.size.times do |i|
      polygon.curr_index = i

      next unless is_vertex_convex polygon

      d = find_intruding_vertex polygon
      distances << [Edge.length(d, polygon.curr_point), polygon.curr_point, d] unless d.nil?
    end

    distances.sort {|a, b| a[0] <=> b[0]}
  end

  def is_vertex_convex(p)
    a = p.neighbor :counterclockwise
    b = p.curr_point
    c = p.neighbor :clockwise

    c.classify(a, b) == :right
  end

  def point_in_triangle?(p, a, b, c)
    (p.classify(a, b) != :left) and (p.classify(b, c) != :left) and (p.classify(c, a) != :left)
  end

  def find_intruding_vertex(polygon)
    a = polygon.neighbor :counterclockwise
    b = polygon.curr_point
    c = polygon.advance! :clockwise
    
    # best candidate
    d = nil
    # best candidate distance
    bestD = -1.0
    ca = Edge.new(c, a)
    v = polygon.move_next!
    
    until v == a
      if point_in_triangle? v, a, b, c
        dist = v.distance(ca.origin, ca.destination)
        if dist > bestD
          bestD = dist
          d = Point.new(v.x, v.y)
        end
      end
      v = polygon.move_next!
    end
    polygon.curr_point = b
    d
  end
  
  def inner_decompose(polygon)
    distances = get_distances polygon
    if distances.empty?
      @polygons << Polygon.copy(polygon.points)
      return
    end

    element = distances[0]
    point1 = element[1]
    point2 = element[2]

    new_polygon = polygon.split_points!(point1, point2)
    
    inner_decompose new_polygon
    inner_decompose polygon    
  end
  
end
