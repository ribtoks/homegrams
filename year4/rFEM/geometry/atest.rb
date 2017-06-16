require File.expand_path(File.dirname(__FILE__)) + '/point.rb'
require File.expand_path(File.dirname(__FILE__)) + '/edge.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon.rb'
require File.expand_path(File.dirname(__FILE__)) + '/complete_triangulator.rb'
require File.expand_path(File.dirname(__FILE__)) + '/polygon_decomposer.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../matrices/calc_matrices.rb'
require 'gnuplot'

def plot_figures (figures, centroids=nil, clusters = nil)
  Gnuplot.open do |gp|
    Gnuplot::Plot.new( gp ) do |plot|
      
      plot.title  "Triangulation"
      
      figures.each do |figure|
        
        x = figure.points.map {|p| p.x}
        y = figure.points.map {|p| p.y}
        
        x << x.first
        y << y.first
        
        plot.data << Gnuplot::DataSet.new( [x, y] ) do |ds|
          ds.with = "linespoints"
          ds.notitle
        end        
        
      end

      unless centroids.nil?
        #centroids.each do |c|
        plot.data << Gnuplot::DataSet.new([centroids.map {|p| p.x},
                                           centroids.map {|p| p.y}]) do |ds|
          ds.with = "points"
          ds.notitle
        end
        #end        
      end


      unless clusters.nil?
        clusters.each do |cluster|

          next if cluster.empty?
          
          cluster.sort! {|p1, p2| p1.y <=> p2.y}

          plot.data << Gnuplot::DataSet.new([cluster.map {|p| p.x},
                                             cluster.map {|p| p.y}]) do |ds|
            ds.with = "line"
            ds.notitle
          end
        end
      end
      
    end
  end
end

def mix_clusters(little_clusters, diameter)
  next_clusters = little_clusters.delete_if {|x| x.empty?}

  clusters = []

  distances = []

  (next_clusters.size - 1).times do |i|
    cluster1 = next_clusters[i]
    cluster2 = next_clusters[i + 1]
    
    distances << [Clusterizator.get_clusters_distance(cluster1, cluster2, lambda {|p1, p2| (p1.x - p2.x).abs}), [i, i + 1]]
  end

  flags = Array.new(little_clusters.size) {|i| true}
  new_clusters = []

  distances.sort!{|a, b| a[0] <=> b[0]}

  d = distances[0][0]
  index = 0
  while d < diameter
    el = distances[index]

    # if used parts of this elements
    if (flags[el[1][0]] == false) or (flags[el[1][1]] == false)
      index += 1
      d = distances[index][0]
      next
    end
    
    new_clusters << (little_clusters[el[1][0]] +
      little_clusters[el[1][1]])
    
    flags[el[1][0]] = false
    flags[el[1][1]] = false

    index += 1
    d = distances[index][0]
    
  end

  little_clusters.size.times do |i|
    clusters << little_clusters[i] if (flags[i])
  end

  clusters.concat new_clusters

  clusters
end


#polygon = Polygon.copy([Point.new(-10, 6), Point.new(-5, 6), Point.new(-4, 7), Point.new(-3, 4), Point.new(0, 5), Point.new(1, 0), Point.new(-4, 1), Point.new(-4, 5), Point.new(-6, 2)])
#polygon = Polygon.copy([Point.new(-4, -1.5), Point.new(-3.5, 2), Point.new(1, 4), Point.new(6, 0.5), Point.new(2, -6)])
#polygon = Polygon.copy([Point.new(-3, 0), Point.new(0, 3), Point.new(3, 0), Point.new(1, 0), Point.new(0, 1), Point.new(-1, 0)])
#polygon = Polygon.copy([Point.new(0, 1), Point.new(0, 2), Point.new(2, 0), Point.new(1, 0)])
#polygon = Polygon.copy([Point.new(-6, 2), Point.new(-3, 5), Point.new(3, 2.2), Point.new(5, 4), Point.new(6, -5), Point.new(0.2, -3.5), Point.new(-4.5, 3)])
polygon = Polygon.copy([Point.new(0, 0), Point.new(0, 4), Point.new(4, 4), Point.new(4, 0)])


#polygon.gnuplot_draw

diameter = 1.5

#decomposer = PolygonDecomposer.new
#polygons = decomposer.decompose(polygon)

#plot_figures polygons

#all_triangles = []
#polygons.each do |pol|
#  triangulator = AdvancedTriangulator.new(diameter, pol)
#  triangles = triangulator.triangulate

#  all_triangles.concat triangles
#end

#centroids = []

#all_triangles.each do |tr|
#  x = tr.points[0].x + tr.points[1].x + tr.points[2].x
#  y = tr.points[0].y + tr.points[1].y + tr.points[2].y
#  centroids << Point.new(x / 3.0, y / 3.0)
#end

#triangulator = AdvancedTriangulator.new(1.2, polygon)
#triangulator.precalculate_polygon.gnuplot_draw
#triangles = triangulator.triangulate

#centroids.sort! {|p1, p2| p1.x <=> p2.x}
#min_distance = 1.0/0.0
#centroids.each do |c1|
#  centroids.each do |c2|
#    if (c1.y - c2.y).abs <= 2*diameter
#      distance = Math.sqrt((c1.x - c2.x)**2 + (c2.y - c1.y)**2)
#      next if distance.zero?
#      min_distance = distance if (distance < min_distance)
#    end
#  end
#end

#width = centroids.last.x - centroids.first.x

#clusters = []
#little_clusters = Clusterizator.clusterize(centroids, width, (width/min_distance).ceil.to_int, lambda {|p1, p2| (p1.x - p2.x).abs })

#clusters = mix_clusters little_clusters, diameter / 2.0
#clusters = little_clusters

#plot_figures all_triangles, centroids, clusters

# ------------------------------

triangulator = Triangulator.new
triangles = triangulator.triangulate(polygon, diameter)
plot_figures triangles
smoothed_triangles = triangulator.provide_smoothing
plot_figures smoothed_triangles

numbered_points = triangulator.enumerate_triangles

points_array = numbered_points.sort {|a,b| a[1] <=> b[1]}

# p "                "
# p points_array

points = points_array.map {|x| x[0]}
p points[0]
Polygon.copy(points).gnuplot_draw
#p triangles.size

MatricesCalculator.get_matrix(5, triangulator, 8, 2, 0, lambda {|x| 1})

#pd = PolygonDecomposer.new
#polygons = pd.decompose polygon
#plot_figures polygons
