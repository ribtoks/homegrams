require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'

class Triangulator
  class << self

    def triangulate(pol)

      #pol.gnuplot_draw

      polygon = Polygon.copy(pol.points)
      polygon.curr_index = pol.curr_index
      
      triangles = []
      if polygon.size == 3
        triangles << Polygon.copy(polygon.points)
        return triangles
      end

      find_convex_vertex polygon

      d = find_intruding_vertex polygon
      if d.nil?
        c = polygon.neighbor :clockwise
        polygon.advance! :counterclockwise

        q = polygon.split! c
        
        triangles.concat triangulate(polygon)
        triangles.concat triangulate(q)
      else
        q = polygon.split! d
        triangles.concat triangulate(q)
        triangles.concat triangulate(polygon)
      end

      triangles
      
    end

    def point_in_triangle?(p, a, b, c)
      (p.classify(a, b) != :left) and (p.classify(b, c) != :left) and (p.classify(c, a) != :left)
    end

    def find_convex_vertex(p)
      a = p.neighbor :counterclockwise
      b = p.curr_point
      c = p.neighbor :clockwise

      until c.classify(a, b) == :right
        a = p.curr_point
        b = p.advance! :clockwise
        c = p.neighbor :clockwise
      end
      
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
    
  end
end
