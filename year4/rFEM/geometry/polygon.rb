require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'

class Polygon
  attr_reader :points
  attr :curr_index, true

  def initialize
    @points = []
    @curr_index = -1
  end

  def Polygon.copy(points)
    polygon = Polygon.new
    @curr_index = 0
    points.each do |p| polygon << Point.new(p.x, p.y) end
    polygon
  end

  def size
    @points.size
  end

  def curr_point
    @points[@curr_index]
  end

  def concat(points_arr)
    @points.concat points_arr
  end

  def curr_point=(value)
    @curr_index = @points.index(value) unless value.nil?
    @points[@curr_index]
  end

  def <<(p)
    if @points.empty?
      @curr_index = 0
    end

    @points << p
    
  end

  def insert_point(index, point)
    @points.insert index, point
  end

  def edge
    Edge.new(self.curr_point, self.next_point)
  end

  def next_point
    @points[(@curr_index + 1) % @points.size]
  end
  
  def prev_point
    @points[@curr_index - 1]
  end

  def move_next!
    @curr_index = (@curr_index + 1) % @points.size
    @points[@curr_index]
  end

  def prev
    if @curr_index > 0
      @points[@curr_index - 1]
    else
      @points.last
    end
  end

  def next
    if @curr_index < @points.size - 1
      @points[@curr_index + 1]
    else
      @points.first
    end
  end

  def nextnext
    if @curr_index < @points.size - 2
      @points[@curr_index + 2]
    else
      @points[@curr_index + 2 - @points.size]
    end
  end

  def prevprev
    if @curr_index > 1
      @points[@curr_index - 2]
    else
      @points[@curr_index - 2 + @points.size]
    end
  end

  def move_prev!
    @curr_index -= 1
    @curr_index += @points.size if @curr_index < 0
    @points[@curr_index]
  end

  def neighbor(rotation)
    if rotation == :clockwise
      self.next
    elsif rotation == :counterclockwise
      self.prev
    end      
  end

  def advance!(rotation)
    if rotation == :clockwise
      self.move_next!
    elsif rotation == :counterclockwise
      self.move_prev!
    end
  end

  def get_angle(index)

    index = index % @points.size if index >= @points.size

    prev_index = index - 1
    next_index = index + 1

    next_index = next_index % @points.size if next_index >= @points.size
    
    pr_point = @points[prev_index]
    n_point = @points[next_index]

    #p "#{prev_index} | #{index} | #{next_index} ||| #{@points.size}"

    angle = Edge.angle_points pr_point, @points[index], n_point

    if n_point.classify(pr_point, @points[index]) == :left
      angle = Math::PI*2.0 - angle
    end

    angle
  end

  def remove_current!
    return if @points.empty?

    if (@points.size > 1)
      @points.delete_at(@curr_index)
      @curr_index -= 1
      @curr_index += @points.size if @curr_index < 0
    else
      @points.clear
      @curr_index = -1
    end
        
  end

  def get_centroid
    x_sum = 0.0
    y_sum = 0.0

    @points.each {|p| x_sum += p.x; y_sum += p.y}
    Point.new(x_sum / @points.size, y_sum / @points.size)
  end

  def contains?(p)
    @points.include? p
  end

  def replace_current!(point)
    @points[@curr_index].x = point.x
    @points[@curr_index].y = point.y
  end

  def replace_point!(point, new_point)
    index = @points.index point
    raise 'Polygon has no requested point' if index.nil?
    @points[index].x = new_point.x
    @points[index].y = new_point.y
  end

  def split!(point)
    index1 = @curr_index
    index2 = @points.index point

    return nil if (index1 == -1) or (index2 == -1)

    temp_curr = Point.new(curr_point.x, curr_point.y)

    p1 = Point.new(curr_point.x, curr_point.y)
    p2 = Point.new(point.x, point.y)

    if index2 < index1
      index1, index2 = index2, index1
      p1, p2 = p2, p1
    end

    @points.insert(index1, Point.new(p1.x, p1.y))
    # keep indices pointing their elements
    index1 += 1
    index2 += 1
    @points.insert(index2, Point.new(p2.x, p2.y))

    pol2 = Polygon.copy @points.slice!(index1..index2)
    pol2.curr_point = temp_curr

    self.curr_point = point
        
    pol2
  end

  def has_point?(p)
    c = 0
    x = p.x
    y = p.y
    @points.size.times do |i|
      c = 1 - c if (((@points[i].y<=y and y<@points[i-1].y) or (@points[i-1].y<=y and y<@points[i].y)) and 
          (x > (@points[i-1].x - @points[i].x) * (y - @points[i].y) / (@points[i-1].y - @points[i].y) + @points[i].x))
    end
    (c == 1)
  end

  def split_points!(point1, point2)
    index1 = @points.index point1
    index2 = @points.index point2

    return nil if (index1 == -1) or (index2 == -1)

    split_indices! index1, index2
  end

  def split_indices!(ind1, ind2)
    index1 = ind1 % @points.size
    index2 = ind2 % @points.size

    index1 += @points.size if index1 < 0
    index2 += @points.size if index2 < 0

    point1 = @points[index1]
    point2 = @points[index2]

    p1 = Point.new(point1.x, point1.y)
    p2 = Point.new(point2.x, point2.y)

    if index2 < index1
      index1, index2 = index2, index1
      p1, p2 = p2, p1
    end

    @points.insert(index1, Point.new(p1.x, p1.y))
    # keep indices pointing their elements
    index1 += 1
    index2 += 1
    @points.insert(index2, Point.new(p2.x, p2.y))

    Polygon.copy @points.slice!(index1..index2)
  end

  def inspect
    @points.inspect
  end

  def gnuplot_draw
    Gnuplot.open do |gp|
      Gnuplot::Plot.new( gp ) do |plot|
        
        plot.title  "Polygon"        
        
        x = @points.map {|p| p.x}
        y = @points.map {|p| p.y}

        x << x.first
        y << y.first
        
        plot.data << Gnuplot::DataSet.new( [x, y] ) do |ds|
          ds.with = "linespoints"
          ds.notitle
        end
        
      end
    end
  end

  def Polygon.get_center(points)
    x_sum = 0.0
    y_sum = 0.0
    
    points.each {|p| x_sum += p.x; y_sum += p.y}
    Point.new(x_sum / points.size, y_sum / points.size)
  end

end
