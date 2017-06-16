class Interval
  
  attr_reader :a, :b

  POSITIVE_INFINITY = 1.0/0.0
  NEGATIVE_INFINITY = -1.0/0.0

  def initialize(*args)
    case args.size
    when 0:
        # raise ArgumentError.new('Cannot create interval from zero points')
        @a = @b = nil
    when 1:
        if args[0].instance_of? Interval
          @a, @b = args[0].a.to_f, args[0].b.to_f
        elsif args[0].kind_of? Numeric
          @a = @b = args[0].to_f
        end
    when 2:
        @a, @b = args
    else
      raise ArgumentError
    end
  end

  def contains_zero?
    @a <= 0.0 and 0.0 <= @b
  end

  def contains_interval?(interval)
    raise ArgumentError.new('Parameter must be an interval') unless interval.instance_of? Interval
    (@a <= interval.a) and (interval.b <= @b)
  end

  def empty?
    @a.nil? or @b.nil?
  end

  def finite?
    @a.finite? and @b.finite?
  end

  def set_ab(a, b)
    @a, @b = a, b
  end

  def a=(value)
    if @b.nil? or (value <= @b)
      @a = value.to_f
    else
      raise ArgumentError.new('Left bound cannot be greater than right one')
    end
  end

  def b=(value)
    if @a.nil? or (value >= @a)
      @b = value.to_f
    else
      raise ArgumentError.new('Right bound cannot be less than left one')
    end
  end  

  def contains_point?(x)
    @a <= x and x <= @b
  end

  def width
    if a.nil? or b.nil?
      nil
    else
      @b - @a
    end
  end

  def divide2
    arr = []
    arr << Interval.new(@a, self.middle)
    arr << Interval.new(self.middle, @b)
    arr
  end

  def middle
    (@a + @b) / 2.0
  end

  def ==(op)
    raise ArgumentError.new('Wrong operand in comparison')
    op.a == @a and op.b == @b
  end

  def to_s
    "[#{@a}, #{@b}]"
  end

  def inspect
    "[#{@a}, #{@b}]"
  end

  def +(op)
    if op.instance_of? Interval
      return Interval.new(@a + op.a, @b + op.b)
    elsif op.kind_of? Numeric
      return Interval.new(@a + op, @b + op)
    else
      raise ArgumentError.new('Bad operand in addition')
    end
  end

  def -(op)
    if op.instance_of? Interval
      Interval.new(@a - op.b, @b - op.a)
    elsif op.kind_of? Numeric
      Interval.new(@a - op, @b - op)
    else
      raise ArgumentError, 'Bad operand in substraction'
    end
  end

  def *(op)
    if op.instance_of? Interval
      arr = [@a*op.a, @a*op.b, @b*op.a, @b*op.b]
      Interval.new(arr.min, arr.max)
    elsif op.kind_of? Numeric
      arr = [@a*op, @b*op]
      Interval.new(arr.min, arr.max)
    else
      raise ArgumentError, 'Bad operand in multiplication'
    end
  end

  def converse
    unless contains_zero?
      Interval.new(1.0/@b, 1.0/@a)
    else
      if @a.zero? and !(@b.zero?)
        Interval.new(1.0/@b, POSITIVE_INFINITY)
      elsif !(a.zero?) and @b.zero?
        Interval.new(NEGATIVE_INFINITY, 1.0/@a)
      else
        [Interval.new(NEGATIVE_INFINITY, 1/@a), Interval.new(1/@b, POSITIVE_INFINITY)]
      end
    end
  end

  def /(op)
    if op.instance_of? Interval
      conv = op.converse
      if conv.instance_of? Interval
        conv*self
      else
        conv.map {|interval| interval*self}
      end
    elsif op.kind_of? Numeric
      #raise ZeroDivisionError if op.is_zero?
      arr = [@a/op, @b/op]
      Interval.new(arr.min, arr.max)
    else
      raise ArgumentError, 'Bad operand in division'
    end
  end

  def Interval.ln(int)
    raise 'Negative numbers in logarifm!' if int.a <= 0
    raise 'Parameter 2 must be of Interval type' unless int.instance_of? Interval
    Interval.new(Math.log(int.a), Math.log(int.b))
  end

  def Interval.log(a, int)
    raise 'Negative numbers in logarifm!' if int.a <= 0
    raise 'Parameter 2 must be of Interval type' unless int.instance_of? Interval
    arr = [Math.log(int.a)/Math.log(a), Math.log(int.b)/Math.log(a)]
    Interval.new(arr.min, arr.max)
  end

  def Interval.sqrt(int)
    raise 'Negative numbers in squere root!' if int.a < 0
    raise 'Parameter must be of Interval type' unless int.instance_of? Interval
    Interval.new(Math.sqrt(int.a), Math.sqrt(b))
  end

  def Interval.sqr(int)
    raise 'Parameter must be of Interval type' unless int.instance_of? Interval
    return Interval.new(int.a**2, int.b**2) if int.a >= 0
    return Interval.new(int.b**2, int.a**2) if int.b < 0
    Interval.new(0, [int.a**2, int.b**2].max)
  end

  def Interval.exp(int)
    raise 'Parameter must be of Interval type' unless int.instance_of? Interval
    Interval.new(Math.exp(int.a), Math.exp(int.b))
  end

  def Interval.pow(int, n)
    raise 'Parameter 1 must be of Interval type' unless int.instance_of? Interval
    if n.integer?
      if (n % 2).nonzero?
        Interval.new(int.a**n, int.b**n)
      else
        return Interval.new(int.a**n, int.b**n) if int.a >= 0
        return Interval.new(int.b**n, int.a**n) if int.b < 0
        Interval.new(0.0, [int.a**n, int.b**n].max)
      end
    else
      raise NotImplementedError.new('Aaaa')
    end
  end

  def Interval.sin(int)
    raise 'Parameter 1 must be of Interval type' unless int.instance_of? Interval
    return Interval.new(-1.0, 1.0) if int.width > 2*Math::PI

    arr = [Math.sin(int.a), Math.sin(int.b)]

    count = (int.a / Math::PI).floor
    left = count*Math::PI + (Math::PI / 2.0)

    (left..int.b).step(Math::PI) do |x| arr << Math.sin(x) if int.contains_point? x end

    Interval.new(arr.min, arr.max)
    
  end

  def Interval.cos(int)
    raise 'Parameter 1 must be of Interval type' unless int.instance_of? Interval
    return Interval.new(-1.0, 1.0) if (int.a - int.b).abs > 2*Math::PI

    arr = [Math.cos(int.a), Math.cos(int.b)]
    count = (int.a / Math::PI).ceil

    left = count*Math::PI

    (left..int.b).step(Math::PI) do |x| arr << Math.cos(x) if int.contains_point? x end
        
    Interval.new(arr.min, arr.max)    
  end

  def Interval.intersect2(op1, op2)
    raise ArgumentError.new('Operands are not intervals') unless op1.instance_of?(Interval) and op2.instance_of?(Interval)

    int1, int2 = nil
    if op1.a <= op2.a
      int1, int2 = op1, op2
    else
      int1, int2 = op2, op1
    end
    
    return Interval.new if int2.a > int1.b
    return Interval.new(int2) if int1.contains_interval? int2
    return Interval.new(int2.a, int1.b) if int2.a <= int1.b
    
    Interval.new
  end

  def Interval.union2(op1, op2)
    raise ArgumentError.new('Operands are not intervals') unless op1.instance_of?(Interval) and op2.instance_of?(Interval)

    if op1.contains_point?(op2.a) or op1.contains_point?(op2.b)
      Interval.new([op1.a, op2.a].min, [op1.b, op2.b].max)
    else
      Interval.new
    end
  end

  def Interval.intersect(intervals)
    result_intervals = []

    points_hash = {}
    intervals.each {|x| points_hash[x.a] = :start; points_hash[x.b] = :end}

    points = points_hash.sort

    curr_start = 0
    points.each do |p|
      if p[1] == :start
        curr_start = p[0]
      elsif p[1] == :end
        unless curr_start.nil?
          result_intervals << Interval.new(curr_start, p[0])
          curr_start = nil
        end
      end
    end

    result_intervals
  end

  def Interval.union(intervals)
    result_intervals = []

    points_hash = {}
    intervals.each {|x| points_hash[x.a] = :start; points_hash[x.b] = :end}

    points = intervals.sort

    curr_start = 0
    counter = 0
    points.each do |p|
      if p[1] == :start
        curr_start = p[0] if counter.zero?
        counter += 1
      elsif p[1] == :end
        counter -= 1        
        result_intervals << Interval.new(curr_start, p[0]) if counter.zero?
        curr_start = nil
      end
    end

    result_intervals
  end
  
end
