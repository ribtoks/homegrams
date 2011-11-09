require 'gnuplot'

def draw_datasets(time_values, values_arr, polynom_str="", first_n=nil)

  Gnuplot.open do |gp|
    Gnuplot::Plot.new( gp ) do |plot|
      
      plot.title  "Time series"
      first_n = time_values.size if first_n.nil?

      values_arr.each do |values|

        plot.data << Gnuplot::DataSet.new( [time_values.first(first_n), values.first(first_n)] ) do |ds|
          ds.with = "linespoints"
          ds.notitle
        end
        
      end

      unless polynom_str.size.zero?
        puts "\n" + polynom_str
        
        plot.data << Gnuplot::DataSet.new( polynom_str ) do |ds|
          ds.with = "lines"
          ds.notitle
        end
        
      end
      
    end
  end
end
