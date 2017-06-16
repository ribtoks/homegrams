require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'

# class for triangulating monotone polygons
# (with no intruding vertices)
class MonotonePolygonTriangulator

  attr :diameter, true
  attr :polygon, true
  attr_reader :precalculated_polygon
  attr_reader :triangles
  attr_reader :points_to_triangles
  attr_reader :points_to_points
  attr_reader :bound_points
  attr_reader :bounds_hash

  def initialize(diameter, polygon)
    @diameter = diameter
    @polygon = polygon
    @triangles = []
  end

  def triangulate
    @triangles = []

    @curr_polygon = precalculate_polygon
    # save local copy for smoothing and
    # bounds determining
    
    @curr_angles = precalculate_angles @curr_polygon    

    @points_to_points = Hash.new {|hash,key| hash[key] = []}
    @points_to_triangles = Hash.new {|hash,key| hash[key] = []}

    inner_triangulate

    @triangles
  end

  # pastes a lot of points on
  # polygon sides
  def precalculate_polygon
    diameter = @diameter

    @bounds_hash = {}
    
    my_polygon = Polygon.new
    
    @polygon.size.times do |i|
      point1 = polygon.points[i - 1]
      point2 = polygon.points[i]
      
      my_polygon.concat Edge.split(point1, point2, diameter)
      
    end

    @precalculated_polygon = Polygon.copy(my_polygon.points)
    
    my_polygon
  end

  def get_bound_points
    @bound_points = {}
    @precalculated_polygon.points.each do |p|
      @bound_points[p] = true
    end
    @bound_points
  end

  def provide_smoothing
    @bound_points = {}
    @precalculated_polygon.points.each do |p|
      @bound_points[p] = true
    end

    @smoothed_triangles = []
    @triangles.each do |tr|
      @smoothed_triangles << Polygon.copy(tr.points)
    end

    @points_to_points.each do |key, value|
      # don't move boundary points
      next if @bound_points.has_key? key

      # calculate new point coordinates
      new_point = Polygon.get_center value

      # replace new value in all triangles
      @points_to_triangles[key].each do |i|
        @smoothed_triangles[i].replace_point!(key, new_point)
      end
    end

    @smoothed_triangles
  end

  # private zone here
  
  private

  def precalculate_angles(polygon)
    angles = {}
    #index_copy = polygon.curr_index
    
    polygon.size.times do |i|
      angles[ polygon.points[i] ] = polygon.get_angle(i)
    end

    angles.sort {|a, b| a[1] <=> b[1]}
  end

  def inner_find_point(p1, p2, p3, s2, s3, s, angle)
    aa = s3*s3 + s*s - 2*s3*s*Math.cos(angle/2.0)
    bb = s2*s2 + s*s - 2*s2*s*Math.cos(angle/2.0)
    
    k = s*s - aa + (p2.x**2 - p1.x**2) + (p2.y**2 - p1.y**2)
    l = aa - bb + (p3.x**2 - p2.x**2) + (p3.y**2 - p2.y**2)
    
    m = 2.0*(p2.x - p1.x)
    n = 2.0*(p2.y - p1.y)
    p = 2.0*(p3.x - p2.x)
    q = 2.0*(p3.y - p2.y)
    
    det = n*p - q*m
    x_new = (l*n - k*q)/det
    y_new = (k*p - l*m)/det
    
    Point.new(x_new, y_new)
  end

  def process_polygon4(polygon)
    angles = []
    polygon.points.each_index {|i| angles << polygon.get_angle(i)}
    
    tr = nil
    if (angles[0] + angles[2]) >= (angles[1] + angles[3])
      tr = polygon.split_indices!(0, 2)
    else
      tr = polygon.split_indices!(1, 3)
    end

    @triangles << tr
    save_triangle_points tr, @triangles.size - 1
    
    @triangles << polygon
    save_triangle_points polygon, @triangles.size - 1
    
    polygon
  end

  def find_index(angle, point, values)
    left, right = 0, values.size-1
    while left <= right
      m = (left + right)/2
      #break if values[m][0] == point
      break if values[m][1] == angle
      if angle > values[m][1]
        left = m + 1
      else
        right = m - 1
      end
    end

    if left <= right
      [true, m]
    else
      [false, left]
    end
  end

  def find_by_angle(angle, point, values)
    index = find_index(angle, point, values)
    index[1] -= 1 while (values[index[1] - 1][1] == angle and index[1] > 0)
    index
  end

  def find_by_point(angle, point, values)
    #index = find_index angle, point, values
    index = values.index {|x| x[0] == point} 
    [index.nil? == false, index]
  end

  def delete_vertex(angle, point, values)
    index = find_by_point angle, point, values
    raise 'delete vertex not found' if index[0] == false
    values.delete_at index[1]
  end

  def change_alpha(point, alpha, new_alpha, values)
    index = find_by_point alpha, point, values
    raise 'change alpha not found' if index[0] == false
    raise 'found bad value' unless values[index[1]][1] == alpha

    # delete old point
    values.delete_at index[1]

    # add new point
    new_index = find_by_angle new_alpha, point, values
    values.insert new_index[1], [point, new_alpha]
    true
  end

  def insert_alpha_point(alpha, point, values)
    new_index = find_by_angle alpha, point, values
    values.insert new_index[1], [point, alpha]
  end

  def cut_off_vertex(index, alpha)
    new_angle = Edge.angle_points_orient(@curr_polygon.next, @curr_polygon.prev, @curr_polygon.prevprev)
    alpha1 = @curr_polygon.get_angle(index - 1)
    change_alpha(@curr_polygon.prev, alpha1, new_angle, @curr_angles)
    
    new_angle = Edge.angle_points_orient(@curr_polygon.nextnext, @curr_polygon.next, @curr_polygon.prev)
    alpha2 = @curr_polygon.get_angle(index + 1)
    change_alpha(@curr_polygon.next, alpha2, new_angle, @curr_angles)

    new_polygon = @curr_polygon.split_indices!(index - 1, index + 1)

    if @curr_polygon.size == 3
      @curr_polygon, new_polygon = new_polygon, @curr_polygon
    end

    @triangles << new_polygon
    save_triangle_points(new_polygon, @triangles.size - 1)
  end

  def save_triangle_points(triangle, index)
    triangle.points.each do |p|
      @points_to_triangles[p] << index
      # @points_to_triangles[p].sort!.uniq!
    end
    
    triangle.points.each do |p|
      triangle.points.each do |inn_p|
        next if inn_p == p
        
        @points_to_points[p] << inn_p
      end
      @points_to_points[p].sort!.uniq!
    end
  end

  
  def process_case2(angle)

    p1 = @curr_polygon.curr_point
    p2 = @curr_polygon.prev
    p3 = @curr_polygon.next

    s2 = Edge.length(p3, p1)
    s3 = Edge.length(p2, p1)
    s1 = Edge.length(@curr_polygon.nextnext, @curr_polygon.next)
    s4 = Edge.length(@curr_polygon.prev, @curr_polygon.prevprev)
    
    s = (s1 + 2*s2 + 2*s3 + s4)/6.0

    p = inner_find_point p1, p2, p3, s2, s3, s, angle

    return p if @curr_polygon.has_point? p

    p "-------------- doh doh doh"
    return nil
    
  end

  def process_case3(angle)
    l1 = Edge.length(@curr_polygon.next, @curr_polygon.curr_point)
    l2 = Edge.length(@curr_polygon.curr_point, @curr_polygon.prev)
    
    p2 = @curr_polygon.curr_point
    p1 = @curr_polygon.prev
    a = l2
    
    index = @curr_polygon.curr_index
    
    if l1 < l2
      p1 = @curr_polygon.curr_point
      p2 = @curr_polygon.next
      a = l1
      
      index += 1
    end
    
    p_middle = Point.new((p1.x + p2.x)/2.0, (p1.y + p2.y)/2.0)
    
    #p = inner_find_point p_middle, p1, p2, a/2.0, a/2.0, Math.sqrt(3.0)*a/2.0, Math::PI
    
    m = 2.0*(p2.x - p1.x)
    n = 2.0*(p2.y - p1.y)
    k = (p2.x**2 + p2.y**2) - (p1.x**2 + p1.y**2)
    # k == m*x + n*y
    
    p = p2.y - p1.y
    q = p1.x - p2.x
    l = p1.x*p2.y - p2.x*p1.y + Math.sqrt(3.0)*a*a/2.0
    
    det = n*p - q*m
    x_new = (l*n - k*q)/det
    y_new = (k*p - l*m)/det
    
    p = Point.new(x_new, y_new)
    
    return [p, index] if @curr_polygon.has_point? p

    # find symmetric point
    p_new = Point.new(2.0*p.x - p_middle.x, 2.0*p.y - p_middle.y)
    return [p_new, index] if @curr_polygon.has_point? p_new
    
    p "doh doh doh 3"
    return nil
    
  end
  
  def inner_triangulate
    count = 0

    return @curr_polygon if @curr_polygon.size < 3
    
    if @curr_polygon.size == 3
      @triangles << @curr_polygon
      return @curr_polygon
    end

    if @curr_polygon.size == 4
      return process_polygon4(@curr_polygon)
    end

    # main loop
    until @curr_angles.empty?

      # @curr_polygon.gnuplot_draw
      
      # first stupid/simple situations
      if (@curr_polygon.size == 4)
        process_polygon4 @curr_polygon
        break
      end

      if (@curr_polygon.size == 3)
        @triangles << @curr_polygon
        save_triangle_points @curr_polygon, @triangles.size - 1
        break
      end

      break if (@curr_polygon.size < 3)

      # and now sophisticated stuff

      # at first delete element with smalest angle
      element = @curr_angles[0]
      point = element[0]
      angle = element[1]

      @curr_angles.delete_at 0
      @curr_polygon.curr_point = point

      # if it is just a cutoff - do it
      # and go to next loop iteration
      if angle < Math::PI/2.0
        #p "upper cutoff"
        cut_off_vertex(@curr_polygon.curr_index, angle)
        next
      end

      a1 = (Math::PI / 2.0)
      a2 = (5.0*Math::PI / 6.0)
      a3 = 2.0*Math::PI

      bad_point = false
      used_elements = []
      
      begin
        
        case angle
        when 0..a1 then
            #p "inner cutoff"
          bad_point = false
          cut_off_vertex(@curr_polygon.curr_index, angle)          
        when a1..a2 then
            #p 'case 2'
          p = process_case2 angle
          
          if p.nil?
            used_elements << [Point.new(point.x, point.y), angle]
            bad_point = true
          else
            bad_point = false

            # if all is ok - than we will insert
            # a new point - need to add first elements
            unless used_elements.empty?
              @curr_angles = used_elements + @curr_angles
              used_elements.clear
            end

            p1 = @curr_polygon.curr_point
            p2 = @curr_polygon.prev
            p3 = @curr_polygon.next

            tr1 = Polygon.copy([p1, p3, p])
            tr2 = Polygon.copy([p1, p, p2])
            
            @triangles << tr1
            save_triangle_points(tr1, @triangles.size - 1)

            @triangles << tr2
            save_triangle_points(tr2, @triangles.size - 1)

            angle1 = @curr_polygon.get_angle(@curr_polygon.curr_index - 1)
            angle2 = @curr_polygon.get_angle(@curr_polygon.curr_index + 1)

            @curr_polygon.replace_current!(p)

            # change angle of previous point
            new_angle = @curr_polygon.get_angle(@curr_polygon.curr_index - 1)
            change_alpha(@curr_polygon.prev, angle1, new_angle, @curr_angles)

            # add angle of next point
            new_angle = @curr_polygon.get_angle(@curr_polygon.curr_index + 1)
            change_alpha(@curr_polygon.next, angle2, new_angle, @curr_angles)

            # add angle of current point
            new_angle = @curr_polygon.get_angle(@curr_polygon.curr_index)
            insert_alpha_point new_angle, @curr_polygon.curr_point, @curr_angles          end
        when a2..a3 then
            return
            p 'case 3'
            arr = process_case3 angle
          if arr.nil?
            used_elements << [Point.new(point.x, point.y), angle]
            bad_point = true
          else
            bad_point = false            
            
            unless used_elements.empty?
              @curr_angles = used_elements + @curr_angles
              used_elements.clear
            end

            p = arr[0]
            index = arr[1]

            @curr_polygon.curr_index = index
            p2 = @curr_polygon.curr_point
            p1 = @curr_polygon.prev

            tr = Polygon.copy([p1, p2, p])
            @triangles << tr
            save_triangle_points(tr, @triangles.size - 1)

            @curr_polygon.insert_point index, p

            # insert angle of current vertex (a new vertex)
            new_angle = @curr_polygon.get_angle(index)
            insert_alpha_point new_angle, p, @curr_angles
            
            new_angle = @curr_polygon.get_angle(index - 1)
            change_alpha(@curr_polygon.prev, angle, new_angle, @curr_angles)
            
            new_angle = @curr_polygon.get_angle(index - 2)
            change_alpha(@curr_polygon.prevprev, angle, new_angle, @curr_angles)
          end
        end

        if (@curr_angles.size <= 4)
          break
        end
        # break if @curr_angles.size <= 4

        #p @curr_angles
        #p @curr_polygon

        if bad_point

          element = @curr_angles[0]
          point = element[0]
          angle = element[1]

          @curr_angles.delete_at 0

          @curr_polygon.curr_point = point

          p 'move next'
        else
          unless used_elements.empty?
            @curr_angles = used_elements + @curr_angles
            used_elements.clear
          end
        end
        
      end while bad_point
      
    end
    
  end
  
end
