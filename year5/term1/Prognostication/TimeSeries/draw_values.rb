require 'gnuplot'

def draw_datasets(time_values, values, trend, m5, m7, tplus2, first_n=nil)

  Gnuplot.open do |gp|
    Gnuplot::Plot.new( gp ) do |plot|
      
      plot.title  "Time series"
      first_n = time_values.size if first_n.nil?


      plot.data << Gnuplot::DataSet.new( [time_values.first(first_n), values.first(first_n)] ) do |ds|
        ds.with = "linespoints lt -1"
        ds.notitle
      end
      
      unless trend.size.zero?
        # puts "\n" + trend
        
        plot.data << Gnuplot::DataSet.new( trend ) do |ds|
          ds.with = "lines lt 1"
          ds.notitle
        end        
      end

      plot.data << Gnuplot::DataSet.new( [time_values.first(first_n), m5.first(first_n)] ) do |ds|
        ds.with = "lines lt 2"
        ds.notitle
      end
      
      plot.data << Gnuplot::DataSet.new( [time_values.first(first_n), m7.first(first_n)] ) do |ds|
        ds.with = "lines lt 3"
        ds.notitle
      end

      plot.data << Gnuplot::DataSet.new( [time_values.first(first_n), tplus2.first(first_n)] ) do |ds|
        ds.with = "lines lt 8"
        ds.notitle
      end

    end
  end
end
