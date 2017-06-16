# deprecated
class Clusterizator
  class << self

    def clusterize(elements, elements_width, clusters_count, distance_func)

      srand

      clusters = Array.new(clusters_count)
      clusters_count.times do |i| clusters[i] = [] end

      elements.each do |e|
        clusters[rand(clusters_count)] << e
      end

      #step = (elements_width / clusters_count).ceil

      #elements.size.times do |i|
      #  clusters[ (elements[i].x / step).floor.to_int ] << elements[i]
      #end

      centroids = Array.new(clusters_count)

      modified = true

      begin

        clusters_count.times do |i|
          centroids[i] = get_centroid clusters[i], distance_func
        end

        modified = false

        new_clusters = Array.new(clusters_count)
        clusters_count.times do |i| new_clusters[i] = [] end

        clusters_count.times do |i|
          clusters[i].each do |element|
            
            # just find closest centroid
            min_distance = 1.0/0.0

            new_centroid_index = i
            
            clusters_count.times do |j|
              next if centroids[j].nil?
              
              distance = distance_func.call centroids[j], element

              if (distance < min_distance)
                new_centroid_index = j
                min_distance = distance
              end
              # each centroid element
            end
            

            unless new_centroid_index == i
              modified = true
            end
            
            new_clusters[new_centroid_index] << element

            # each cluster element
          end
          #each cluster
        end
        clusters = new_clusters
      end while modified


      clusters
    end

    def get_centroid(elements, distance_func)
      min_distance = 1.0/0.0
      centroid = nil

      elements.each do |el|
        distance = 0

        elements.each do |inn_el|
          distance += distance_func.call(el, inn_el)
        end
        
        if distance < min_distance
          centroid = el
          min_distance = distance
        end
      end
      centroid
    end

    def get_centroid_width(elements, distance_func)

      if (elements.size == 0) or (elements.size == 1)
        return -1.0/0.0
      end
      
      #elements.sort! {|p1, p2| p1.y <=> p2.y}

      max_distance = -1.0/0.0

      elements.each do |el|
        elements.each do |inn_el|
          distance = distance_func.call el, inn_el
          max_distance = distance if max_distance < distance
        end
      end

      max_distance
      
    end

    def get_clusters_distance(cluster1, cluster2, distance_func)
      array = cluster1 + cluster2

      get_centroid_width array, distance_func
    end
    
  end  
  
end
