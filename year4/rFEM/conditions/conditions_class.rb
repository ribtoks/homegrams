require File.expand_path(File.dirname(__FILE__)) + '/../geometry/edge.rb'

class ConditionsItem
  attr :alpha, true
  attr :lambda, true
  attr :u_environment, true
  
  def initialize(hash)
    @alpha = hash[:alpha]
    @lambda = hash[:lambda]
    @u_environment = hash[:u_environment]
  end

  def ConditionsItem.copy(cond)
    ConditionsItem.new({:alpha=>cond.alpha,
                         :lambda=>cond.lambda,
                         :u_environment=>cond.u_environment})
  end
  
end

class BoundaryConditions
  # hash of
  # [sorted array with two points] -> index in boundary conditions array
  attr_reader :points_to_condindex
  # diameter of triangles
  attr_reader :diameter
    
  # conditions_hash
  # start, end - bounds of segment
  # [[start, end]] -> condition
  def initialize(conditions_hash, diameter)

    @conditions = []
    @points_to_condindex = {}

    @diameter = diameter
    
    conditions_hash.each do |key, value|
      p_start = key[0]
      p_end = key[1]

      points = Edge.split p_start, p_end, diameter

      points << Point.copy(p_end) unless (points.last == p_end)

      @conditions << ConditionsItem.copy(value)

      # add all segments for big segment partition
      (points.size - 1).times do |i|
        p1_hash = points[i].hash
        p2_hash = points[i + 1].hash

        arr = [p1_hash, p2_hash].sort
        @points_to_condindex[arr] = @conditions.size - 1
      end
      
      #points.each do |p|
      #  @points_to_condindex[p] = @conditions.size - 1
      #end
    end    
  end

  def [](index)
    @conditions[index]
  end
  
end

class CompleteTaskConditions
  attr_reader :a, :b, :d, :f, :boundary_conditions

  def initialize(a, b, d, function, conditions)
    @a = a
    @b = b
    @d = d
    @f = function

    @boundary_conditions = conditions
  end
end
