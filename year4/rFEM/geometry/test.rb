require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/triangulator.rb'
require 'gnuplot'


#polygon = Polygon.copy([Point.new(-6, 2), Point.new(-3, 5), Point.new(3, 2.2), Point.new(5, 4), Point.new(6, -5), Point.new(0.2, -3.5), Point.new(-4.5, 3)])

polygon = Polygon.copy([Point.new(-10, 6), Point.new(-5, 6), Point.new(-4, 7), Point.new(-3, 4), Point.new(0, 5), Point.new(1, 0), Point.new(-4, 1), Point.new(-5, 4), Point.new(-6, 2)])

def plot_triangles (triangles)
  Gnuplot.open do |gp|
    Gnuplot::Plot.new( gp ) do |plot|
      
      plot.title  "Triangulation"
      
      triangles.each do |triangle|
        
        x = triangle.points.map {|p| p.x}
        y = triangle.points.map {|p| p.y}
        
        x << x.first
        y << y.first
        
        plot.data << Gnuplot::DataSet.new( [x, y] ) do |ds|
          ds.with = "linespoints"
          ds.notitle
        end
        
      end
      
    end
  end
end

#polygon.gnuplot_draw

#polygon.points.count.times do |i|
#  polygon.curr_index = i
#  triangles = Triangulator.triangulate polygon
#  plot_triangles triangles
#end

hash = {}
a = Point.new 2, 3

hash[a] = 14
hash[Point.new(2.0, 3.0)] = 12

p hash

#polygon.curr_index = 5
#triangles = Triangulator.triangulate polygon
#plot_triangles triangles
