require File.expand_path(File.dirname(__FILE__)) + '/point.rb'

class Edge
  attr :origin, true
  attr :destination, true

  def initialize(*args)
    case args.size
    when 0 then
        @origin = Point.new(0, 0)
      @destination = Point.new(0, 0)
    when 1 then
        raise ArgumentError.new('Argument is not an Edge') unless args[0].instance_of? Edge
      @origin = Point.new(args[0].origin.x, args[0].origin.y)
      @destination = Point.new(args[0].destination.x, args[0].destination.y)
    when 2 then
        raise ArgumentError.new('Arguments are not Points') unless args[0].instance_of?(Point) and args[1].instance_of?(Point)
      @origin = Point.new(args[0].x, args[0].y)
      @destination = Point.new(args[1].x, args[1].y)
    end
  end

  # rotate on 90* clockwise
  def rotate!
    m = (@origin + @destination)*0.5
    v = @destination - @origin

    n = Point.new(v.y, -v.x)

    @origin = m - n*0.5
    @destination = m + n*0.5
  end

  def ==(e)
    (@origin == e.origin) and (@destination == e.destination)
  end

  def length
    Math.sqrt((@origin.x - @destination.x)**2 +
              (@destination.y - @origin.y)**2)
  end

  def flip!
    rotate!
    rotate!
  end

  def intersect (edge)
    a = @origin
    b = @destination

    c = edge.origin
    d = edge.destination

    n = Point.new((d - c).y, (c - d).x)
    denominator = n*(b - a)

#    if denominator.zero?
#      aclass = @origin.classify(edge.origin, edge.destination)
#      if (aclass == :left) or (aclass == :right)
#        return :parallel
#      else
#        return :collinear
#      end
    #    end
    if denominator.zero?
      raise 'denominator is zero!'
    end

    num = n*(a - c)
    -num/denominator
    
  end

  def Edge.length(p1, p2)
    Math.sqrt((p1.x - p2.x)**2 + (p1.y - p2.y)**2)
  end

  def Edge.angle(edge1, edge2)
    # just use the cosine theorem
    len1 = edge1.length
    len2 = edge2.length

    len3 = Edge.length(edge1.origin, edge2.destination)

    value = (len1*len1 + len2*len2 - len3*len3)/(2.0*len1*len2)

    value = -1.0 if value < -1.0
    value = 1.0 if value > 1.0

    Math.acos(value).abs
  end

  def Edge.angle_points(point1, point2, point3)
    len1 = Edge.length point1, point2
    len2 = Edge.length point2, point3

    len3 = Edge.length point1, point3

    value = (len1*len1 + len2*len2 - len3*len3)/(2.0*len1*len2)

    value = -1.0 if value < -1.0
    value = 1.0 if value > 1.0

    Math.acos(value).abs
  end

  def Edge.angle_points_orient(point_next, point, point_prev)
    angle = Edge.angle_points point_prev, point, point_next
    
    if point_next.classify(point_prev, point) == :left
      angle = Math::PI*2.0 - angle
    end
    angle
  end

  def Edge.split(point1, point2, diameter)
    points_array = []

    length = Edge.length(point1, point2)
      
    points_count = (length/diameter).ceil
    step = length / points_count
    
    angle = Math.atan((point2.y - point1.y).abs/(point2.x - point1.x).abs)
    step_x = step * Math.cos(angle)
    step_y = step * Math.sin(angle)
    
    sign_x = 1.0
    sign_x = -1.0 if (point2.x - point1.x) < 0
    
    sign_y = 1.0
    sign_y = -1.0 if (point2.y - point1.y) < 0
    
    points_count.times do |i|
      points_array << Point.new(point1.x + i*step_x*sign_x,
                      point1.y + i*step_y*sign_y)
    end

    points_array
  end
  
end
