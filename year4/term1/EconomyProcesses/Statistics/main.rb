#!/usr/bin/ruby
require 'wx'
require 'gnuplot'
require File.expand_path(File.dirname(__FILE__)) + '/sample.rb'
require File.expand_path(File.dirname(__FILE__)) + '/SampleOperations.rb'
require File.expand_path(File.dirname(__FILE__)) + '/sampleswork.rb'

class SamplesMainFrame < SamplesWorkFrame

# 4 6 8 11 12 14 15 16 18 20
# 20 30 37 52 55 65 69 75 89 86
  
  def initialize(parent=nil)
    super

    evt_button(@submitbutton) {|event| submit_samples_buttonClick }
    evt_button(@recalculatebutton) {|event| recalculatebutton_click }
    evt_button(@additionalbutton) {|event| additionalbutton_click }
    evt_button(@checkphisherbutton) {|event| checkphisherbutton_click }
    evt_button(@drawgraphicsbutton) {|event| drawgraphicsbutton_click}
    evt_button(@intervalscalculatebutton) {|event| intervalscalculate_click}
    evt_button(@calculatenextpbutton) {|event| nextpoint_click}

    @criticalValues = {
      1 => 161.4,
      2 => 18.51,
      3 => 10.13,
      4 => 7.71,
      5 => 6.61,
      6 => 5.99,
      7 => 5.59,
      8 => 5.32,
      9 => 5.12,
      10 => 4.96
    }

    # alpha = 5% (alpha/2 = 2.5%)
    @otherCriticalValues = {
      1 => 12.706,
      2 => 4.303,
      3 => 3.182,
      4 => 2.776,
      5 => 2.571,
      6 => 2.447,
      7 => 2.365,
      8 => 2.306,
      9 => 2.262,
      10 => 2.228
    }
        
  end

  def submit_samples_buttonClick()

    sample1str = @sample1textbox.value
    sample2str = @sample2textbox.value

    @sample1 = Array.new
    @sample2 = Array.new

    sample1str.split.each {|x| @sample1 << x.to_f }
    sample2str.split.each {|x| @sample2 << x.to_f }

    hash = Hash[@sample1.zip(@sample2)]
    arrays = hash.sort.transpose
    @sample1 = arrays[0]
    @sample2 = arrays[1]

    # raise 'Different samples sizes' if @sample1.size != @sample2.size

    columns = %w[N X Y X*Y X^2 ~Y E Y^2 (Y-Y~)^2 (~Y-Y_)^2 ~Y-Y_ Y-Y_ (Y-Y_)^2]
    
    @samplesgrid.create_grid 0, 0
    
    @samplesgrid.enable_editing true
    @samplesgrid.enable_grid_lines true
    @samplesgrid.set_margins 0, 0
    @samplesgrid.auto_size_columns
    @samplesgrid.enable_drag_col_move
    @samplesgrid.enable_drag_col_size
    @samplesgrid.set_col_label_size 30
    @samplesgrid.enable_drag_row_size

    @samplesgrid.get_number_rows().times do |i|
      @samplesgrid.delete_rows
    end

    @samplesgrid.get_number_cols().times do |i|
      @samplesgrid.delete_cols
    end

    @sample1.size.times do |i|
      @samplesgrid.append_rows
      @samplesgrid.set_row_label_value i, (i + 1).to_s
    end
    @samplesgrid.append_rows
    @samplesgrid.set_row_label_value((@samplesgrid.get_number_rows() - 1), 'Mean')

    columns.size.times do |i|
      @samplesgrid.append_cols
      @samplesgrid.set_col_label_value i, columns[i]
    end
    @samplesgrid.set_col_label_alignment Wx::ALIGN_CENTRE, Wx::ALIGN_CENTRE

    recalculate_statistics
    
    @m_notebook1.set_selection 1
  end

  def recalculate_statistics
     # -------------------- CALCULATIONS --------------------
    
    x = @sample1
    y = @sample2

    xSquares = @sample1.squares
    ySquares = @sample2.squares

    products = @sample1*@sample2

    mean_vector = SampleOperations.get_vector @sample2.mean, @sample2.size

    ym_mean = SampleOperations.delta y, mean_vector
    ym_mean_squares = SampleOperations.square_array ym_mean

    @b1 = SampleOperations.calculate_b1 @sample1, @sample2
    @b0 = SampleOperations.calculate_b0 @sample1, @sample2, @b1

    @yt = SampleOperations.get_rvector x, @b0, @b1

    ytm_mean = SampleOperations.delta @yt, mean_vector
    ytm_mean_squares = SampleOperations.square_array ytm_mean

    ym_yt = SampleOperations.delta y, @yt
    ym_yt_squares = SampleOperations.square_array ym_yt

    delta_vector = SampleOperations.delta y, @yt

    # -------------------- END --------------------

    # --------------- SET VALUES TO GRID ---------------
    x.size.times do |i|

      @samplesgrid.set_cell_value i, 0, i.to_s

      @samplesgrid.set_cell_value i, 1, x[i].to_s
      @samplesgrid.set_cell_value i, 2, y[i].to_s

      @samplesgrid.set_cell_value i, 3, products[i].to_s

      @samplesgrid.set_cell_value i, 4, xSquares[i].to_s
      
      @samplesgrid.set_cell_value i, 5, @yt[i].to_s
      @samplesgrid.set_cell_value i, 6, delta_vector[i].to_s

      @samplesgrid.set_cell_value i, 7, ySquares[i].to_s

      @samplesgrid.set_cell_value i, 8, ym_yt_squares[i].to_s
      @samplesgrid.set_cell_value i, 9, ytm_mean[i].to_s
      @samplesgrid.set_cell_value i, 10, ytm_mean_squares[i].to_s
      
      @samplesgrid.set_cell_value i, 11, ym_mean[i].to_s
      @samplesgrid.set_cell_value i, 12, ym_mean_squares[i].to_s
      
    end

    last_row = @samplesgrid.get_number_rows
    last_row -= 1

    # -------------------- SET MEAN VALUES --------------------
    
    @samplesgrid.set_cell_value last_row, 1, x.mean.to_s
    @samplesgrid.set_cell_value last_row, 2, y.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 3, products.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 4, xSquares.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 5, @yt.mean.to_s
    @samplesgrid.set_cell_value last_row, 6, delta_vector.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 7, ySquares.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 8, ym_yt_squares.mean.to_s
    @samplesgrid.set_cell_value last_row, 9, ytm_mean.mean.to_s
    @samplesgrid.set_cell_value last_row, 10, ytm_mean_squares.mean.to_s
    
    @samplesgrid.set_cell_value last_row, 11, ym_mean.mean.to_s
    @samplesgrid.set_cell_value last_row, 12, ym_mean_squares.mean.to_s
    
     # ajust all sizes
    @samplesgrid.auto_size
  end

  def recalculatebutton_click
    @sample1.clear
    @sample2.clear

    rows_count = @samplesgrid.get_number_rows
    # -2, cause last row are means
    (rows_count - 1).times do |i|
      @sample1 << @samplesgrid.get_cell_value(i, 1).to_f
      @sample2 << @samplesgrid.get_cell_value(i, 2).to_f
    end

    recalculate_statistics
    
  end

  def additionalbutton_click

    columns = %w[Square_sums Degrees Middle_squares]
    
    @additonalgrid.create_grid 0, 0
    
    @additonalgrid.enable_editing true
    @additonalgrid.enable_grid_lines true
    @additonalgrid.set_margins 0, 0
    @additonalgrid.auto_size_columns
    @additonalgrid.enable_drag_col_move
    @additonalgrid.enable_drag_col_size
    @additonalgrid.set_col_label_size 30
    @additonalgrid.enable_drag_row_size

    @additonalgrid.get_number_rows().times do |i|
      @additonalgrid.delete_rows
    end

    @additonalgrid.get_number_cols().times do |i|
      @additonalgrid.delete_cols
    end

    @additonalgrid.append_rows
    @additonalgrid.set_row_label_value 0, 'Regression (SSR)'

    @additonalgrid.append_rows
    @additonalgrid.set_row_label_value 1, 'Error (SSE)'

    @additonalgrid.append_rows
    @additonalgrid.set_row_label_value 2, 'General (SST)'

    columns.size.times do |i|
      @additonalgrid.append_cols
      @additonalgrid.set_col_label_value i, columns[i]
    end
    @additonalgrid.set_col_label_alignment Wx::ALIGN_CENTRE, Wx::ALIGN_CENTRE

    recalculate_additional
    
    @m_notebook1.set_selection 2    
  end

  def recalculate_additional
    y = @sample2

    mean_vector = SampleOperations.get_vector @sample2.mean, @sample2.size

    ytm_mean = SampleOperations.delta @yt, mean_vector
    ytm_mean_squares = SampleOperations.square_array ytm_mean

    ytm_y = SampleOperations.delta @yt, y
    ytm_y_squares = SampleOperations.square_array ytm_y

    ym_mean = SampleOperations.delta y, mean_vector
    ym_mean_squares = SampleOperations.square_array ym_mean

    ssr = ytm_mean_squares.sum
    sse = ytm_y_squares.sum
    sst = ym_mean_squares.sum

    msr = ssr / 1.0
    mse = sse / (y.size - 2)

    @additonalgrid.set_cell_value 0, 0, ssr.to_s
    @additonalgrid.set_cell_value 0, 1, '1'
    @additonalgrid.set_cell_value 0, 2, msr.to_s

    @additonalgrid.set_cell_value 1, 0, sse.to_s
    @additonalgrid.set_cell_value 1, 1, (y.size - 2).to_s
    @additonalgrid.set_cell_value 1, 2, mse.to_s

    @additonalgrid.set_cell_value 2, 0, sst.to_s
    @additonalgrid.set_cell_value 2, 1, (y.size - 1).to_s

    @additonalgrid.auto_size
  end

  def checkphisherbutton_click
    table_value = @criticalValues[@sample1.size - 2]  # @phishertablevalue.value.to_f

    phisher = @additonalgrid.get_cell_value(0, 2).to_f / @additonalgrid.get_cell_value(1, 2).to_f

    if phisher < table_value
      @phisherresult.set_label 'Regression fails' + " (#{phisher} < #{table_value})"
    else
      @phisherresult.set_label 'Regression fits' + " (#{phisher} >= #{table_value})"
    end
    
  end

  def drawgraphicsbutton_click
    Gnuplot.open do |gp|
      Gnuplot::Plot.new( gp ) do |plot|
        
        plot.title  "Regression graphics"
        # plot.ylabel "x"
        # plot.xlabel "x^2"
        
        x = @sample1.values
        y = @sample2.values
        yt = @yt
        
        plot.data << Gnuplot::DataSet.new( [x, y] ) do |ds|
          ds.with = "points"
          ds.notitle
        end

        plot.data << Gnuplot::DataSet.new( [x, yt] ) do |ds|
          ds.with = "linespoints"
          ds.notitle
        end
        
      end
    end
  end

  def intervalscalculate_click
    y = @sample2
    x = @sample1

    x_mean_vector = SampleOperations.get_vector @sample1.mean, @sample1.size
    xm_mean = SampleOperations.delta x, x_mean_vector
    xm_mean_squares = SampleOperations.square_array xm_mean

    ym_yt = SampleOperations.delta y, @yt
    ym_yt_squares = SampleOperations.square_array ym_yt

    sigma_e = Math.sqrt(ym_yt_squares.sum / (@sample2.size - 2.0))
    sigma_b0 = Math.sqrt(sigma_e*sigma_e*@sample1.squares.sum / (@sample1.size.to_f*xm_mean_squares.sum))
    sigma_b1 = Math.sqrt(sigma_e*sigma_e / xm_mean_squares.sum)

    table_value = @otherCriticalValues[@sample1.size - 2]

    t0 = @b0 / sigma_b0
    t1 = @b1 / sigma_b1

    @sigmae_label.set_label sigma_e.to_s
    @sigmab0_label.set_label sigma_b0.to_s
    @sigmab1_label.set_label sigma_b1.to_s

    b0_interval_a = @b0 - table_value*sigma_b0
    b0_interval_b = @b0 + table_value*sigma_b0

    b1_interval_a = @b1 - table_value*sigma_b1
    b1_interval_b = @b1 + table_value*sigma_b1

    @ci_b0_label.set_label "b0 (#{@b0}) interval - [#{b0_interval_a}, #{b0_interval_b}]"
    @ci_b1_label.set_label "b1 (#{@b1}) interval - [#{b1_interval_a}, #{b1_interval_b}]"
    
  end

  def nextpoint_click

    y = @sample2.values
    x = @sample1.values

    x_mean_vector = SampleOperations.get_vector @sample1.mean, @sample1.size
    xm_mean = SampleOperations.delta x, x_mean_vector
    xm_mean_squares = SampleOperations.square_array xm_mean

    ym_yt = SampleOperations.delta y, @yt
    ym_yt_squares = SampleOperations.square_array ym_yt

    sigma_e = Math.sqrt(ym_yt_squares.sum / (@sample2.size - 2.0))
    
    x_next = @nextpointtextbox.value.to_f
    y_next = @b0 + @b1*x_next

    @nextvaluelabel.set_label "y = #{y_next}"

    table_value = @otherCriticalValues[@sample1.size - 2]
    value = table_value*sigma_e*Math.sqrt(1.0 + 1.0/x.size + (x_next - x.mean)**2/xm_mean_squares.sum)
    @intervallabel.set_label "y in [#{y_next - value}, #{y_next + value}]"
    
  end

end

Wx::App.run do
  SamplesMainFrame.new.show
end
