class Point
  attr :x, true
  attr :y, true

  def initialize(x, y)
    @x, @y = x.to_f, y.to_f
  end

  def Point.copy(p)
    Point.new(p.x, p.y)
  end

  def +(p)
    raise ArgumentError.new('Wrong argument type') unless p.instance_of? Point
    Point.new(@x + p.x, @y + p.y)
  end

  def -(p)
    raise ArgumentError.new('Wrong argument type') unless p.instance_of? Point
    Point.new(@x - p.x, @y - p.y)
  end

  def *(a)
    if a.instance_of? Point
      @x*a.x + @y*a.y
    elsif a.kind_of? Numeric
      Point.new(@x*a, @y*a)
    end
  end

  def length
    Math.sqrt(@x**2 + @y**2)
  end

  def [](i)
    if i == 0
      x
    elsif i == 1
      y
    end
  end

  def []=(i, value)
    if i == 0
      @x = value
    elsif i == 1
      @y = value
    end
  end

  #def !=(p)
  #  (@x != p.x) or (@y != p.y)
  #end

  def <(p)
    eps = 0.000000001
    ((@x + eps) < p.x) or (((p.x - @x).abs < eps) and ((@y + eps) < p.y))
  end

  def >(p)
    eps = 0.000000001
    (@x > (p.x + eps)) or (((p.x - @x).abs < eps) and (@y > (p.y + eps)))
  end

  def <=>(p)
    return 0 if self == p
    return 1 if self > p
    -1
  end

  def classify(p0, p1)
    p2 = self

    a = p1 - p0
    b = p2 - p0

    sa = a.x*b.y - b.x*a.y
    if sa > 0.0
      :left
    elsif sa < 0.0
      :right
    elsif (a.x * b.x < 0.0) or (a.y * b.y < 0)
      :behind
    elsif a.length < b.length
      :beyond
    elsif p0 == p2
      :origin
    elsif p2 == p1
      :destination
    else
      :between
    end
    
  end

  def distance(p0, p1)
    (((p0.y - p1.y)*@x + (p1.x - p0.x)*@y + (p0.x*p1.y - p1.x*p0.y))/Math.sqrt((p1.x - p0.x)**2 + (p0.y - p1.y)**2)).abs

#    edge = Edge.new(p0, p1)
#    edge.flip!
#    edge.rotate!

#    n = p1 - p0
#    n = n*(1.0/n.length)

#    f = Edge.new(self, self + n)

#    f.intersect edge    
  end

  def Point.orientation(p0, p1, p2)
    a = p1 - p0
    b = p2 - p0

    sa = a.x*b.y - b.x*a.y
    if sa > 0.0
      # orientated positive
      1
    elsif sa < 0.0
      # orientated negative
      -1
    else
      # collinear
      0
    end
  end

  def to_s
    "(#{@x}, #{@y})"
  end

  def inspect
    "(#{@x}, #{@y})"
  end

  def eql?(obj)
    eps = 0.000000001
    self.class.equal?(obj.class) && ((obj.x - @x).abs < eps) &&
      ((obj.y - @y).abs < eps)
  end

  alias == eql?

  def hash
    a = 100000000
    ((@x*a).ceil * 13.0 + (@y*a).ceil * 57.0).hash
  end
  
end
