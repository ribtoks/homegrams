#!/usr/bin/ruby

require File.expand_path(File.dirname(__FILE__)) + '/geometry/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/geometry/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/geometry/complete_triangulator.rb'
require 'matrix'
require 'gnuplot'
require File.expand_path(File.dirname(__FILE__)) + '/matrices/matrix_plus.rb'
require File.expand_path(File.dirname(__FILE__)) + '/matrices/matrix_generator.rb'
require File.expand_path(File.dirname(__FILE__)) + '/conditions/conditions_class.rb'
require File.expand_path(File.dirname(__FILE__)) + '/solver/slae_solver.rb'



#polygon = Polygon.copy([Point.new(-10, 6), Point.new(-5, 6), Point.new(-4, 7), Point.new(-3, 4), Point.new(0, 5), Point.new(1, 0), Point.new(-4, 1), Point.new(-4, 5), Point.new(-6, 2)])
#polygon = Polygon.copy([Point.new(-4, -1.5), Point.new(-3.5, 2), Point.new(1, 4), Point.new(6, 0.5), Point.new(2, -6)])

#polygon = Polygon.copy([Point.new(0, 1), Point.new(0, 2), Point.new(2, 0), Point.new(1, 0)])
#polygon = Polygon.copy([Point.new(-6, 2), Point.new(-3, 5), Point.new(3, 2.2), Point.new(5, 4), Point.new(6, -5), Point.new(0.2, -3.5), Point.new(-4.5, 3)])

p1 = Point.new(0,0)
p2 = Point.new(0,3)
p3 = Point.new(6,3)
p4 = Point.new(6,0)
polygon = Polygon.copy([p1, p2, p3, p4])
bounds_hash = {
  [p1, p2] => ConditionsItem.new({:alpha=>0, :lambda=>1, :u_environment=>0}),
  [p2, p3] => ConditionsItem.new({:alpha=>10000, :lambda=>1, :u_environment=>0}),
  [p3, p4] => ConditionsItem.new({:alpha=>0, :lambda=>1, :u_environment=>0}),
  [p4, p1] => ConditionsItem.new({:alpha=>10000, :lambda=>1, :u_environment=>100})}


#p1 = Point.new(0, 1)
#p2 = Point.new(0, 3)
#p3 = Point.new(3, 0)
#p4 = Point.new(1, 0)

#polygon = Polygon.copy([p1, p2, p3, p4])

#bounds_hash = {
#  [p1, p2] => ConditionsItem.new({:alpha=>2, :lambda=>1, :u_environment=>0}),
#  [p2, p3] => ConditionsItem.new({:alpha=>1000000, :lambda=>1, :u_environment=>5}),
#  [p3, p4] => ConditionsItem.new({:alpha=>3, :lambda=>1, :u_environment=>0}),
#  [p4, p1] => ConditionsItem.new({:alpha=>1000000, :lambda=>1, :u_environment=>0})}

diameter = 1.0
a = -8
b = -8
d = 0
f = lambda {|x| 1}
#f = lambda {|x| 0.0000001}



boundary_conditions = BoundaryConditions.new(bounds_hash, diameter)
                                             
conditions = CompleteTaskConditions.new(a, b, d, f, boundary_conditions)

# create new triangulator class instance
triangulator = Triangulator.new
triangulator.triangulate(polygon, diameter)
triangulator.provide_smoothing!
nums = triangulator.enumerate_triangles


m_f = MatrixGenerator.generate_matrix(triangulator, conditions)

m_matrix = m_f[0]
f_vector = m_f[1]

puts m_matrix.dump
puts f_vector

# get result
u = SLAE_Solver.solve_linear_system(m_matrix, f_vector)

puts u

# -------------------------------
# save result to gnuplot datafile
# -------------------------------

file = File.new("results/datafile_#{Time.now.strftime("%H_%M")}.dat", 'w+')

file.puts("# X Y Z")

# sort hash by values
arr = nums.sort {|a, b| a[1] <=> b[1]}
#arr.each do |element|
#  point = element[0]
#  index = element[1]
#  u_value = u[index]

#  file.puts("#{point.x} #{point.y} #{u_value}\n")
#end

triangulator.triangles.each do |triangle|
  triangle.points.each do |p|
    index = nums[p]
    u_value = u[index]
    
    file.puts("#{p.x} #{p.y} #{u_value}")
  end

  #p = triangle.points[0]
  #index = nums[p]
  #u_value = u[index]
  #file.puts("#{p.x} #{p.y} #{u_value}")
  file.puts("")
end

file.close

# gnuplot hacks
# set pm3d
# set dgrid3d 30,30
# set hidden3d
# splot "datafile.dat" u 1:2:3 with lines
