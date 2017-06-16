#!/usr/bin/ruby
require 'wx'
require 'gnuplot'
require File.expand_path(File.dirname(__FILE__)) + '/sample.rb'
require File.expand_path(File.dirname(__FILE__)) + '/SampleOperations.rb'
require File.expand_path(File.dirname(__FILE__)) + '/sampleswork.rb'

class SamplesMainFrame < SamplesWorkFrame

  def initialize(parent=nil)
    super

    evt_button(@submitbutton) {|event| submit_samples_buttonClick }
    evt_button(@recalculatebutton) {|event| recalculatebutton_click }
    evt_button(@additionalbutton) {|event| additionalbutton_click }
    evt_button(@drawgraphicsbutton) {|event| drawgraphicsbutton_click}
    evt_button(@checkphisherbutton) {|event| checkphisherbutton_click}
    
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
    sample3str = @sample3textbox.value

    @sample1 = []
    @sample2 = []
    @sample3 = []

    sample1str.split.each {|x| @sample1 << x.to_f }
    sample2str.split.each {|x| @sample2 << x.to_f }
    sample3str.split.each {|x| @sample3 << x.to_f }

    raise 'Different samples sizes' if @sample1.size != @sample2.size

    columns = %w[N X X2 Y X1*X1 X2*X2 Y*Y X1*Y X2*Y X1*X2 Y~ (Y-mean(Y)) (~Y-mean(Y)) (Y-mean(Y))^2 (~Y-mean(Y))^2 (Y-Y~) (Y-Y~)^2]
    
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
    
    x1 = @sample1
    x2 = @sample2
    y = @sample3

    recalculate_additional
    @y = []
    
    @sample1.size.times do |i|
      @y << @b0 + @b1*@sample1[i] + @b2*@sample2[i]      
    end

    product_x1_x1 = @sample1*@sample1
    product_x1_x2 = @sample1*@sample2
    product_x2_x2 = @sample2*@sample2
    product_x1_y = @sample1*@sample3
    product_x2_y = @sample2*@sample3
    product_y_y = @sample3*@sample3

    mean_vector = SampleOperations.get_vector @sample3.mean, @sample3.size

    ym_mean = SampleOperations.delta y, mean_vector
    ym_mean_squares = SampleOperations.square_array ym_mean

    # @b1 = SampleOperations.calculate_b1 @sample1, @sample2
    # @b0 = SampleOperations.calculate_b0 @sample1, @sample2, @b1

    @yt = @y

    ytm_mean = SampleOperations.delta @yt, mean_vector
    ytm_mean_squares = SampleOperations.square_array ytm_mean

    ym_yt = SampleOperations.delta y, @yt
    ym_yt_squares = SampleOperations.square_array ym_yt

    delta_vector = SampleOperations.delta y, @yt

    # -------------------- END --------------------

    # --------------- SET VALUES TO GRID ---------------
    x1.size.times do |i|

      @samplesgrid.set_cell_value i, 0, i.to_s

      @samplesgrid.set_cell_value i, 1, x1[i].to_s
      @samplesgrid.set_cell_value i, 2, x2[i].to_s
      @samplesgrid.set_cell_value i, 3, y[i].to_s

      @samplesgrid.set_cell_value i, 4, product_x1_x1[i].to_s
      
      @samplesgrid.set_cell_value i, 5, product_x2_x2[i].to_s
      @samplesgrid.set_cell_value i, 6, product_y_y[i].to_s

      @samplesgrid.set_cell_value i, 7, product_x1_y[i].to_s

      @samplesgrid.set_cell_value i, 8, product_x2_y[i].to_s
      @samplesgrid.set_cell_value i, 9, product_x1_x2[i].to_s

      @samplesgrid.set_cell_value i, 10, @y[i].to_s

      @samplesgrid.set_cell_value i, 11, ym_mean[i].to_s
      @samplesgrid.set_cell_value i, 12, ytm_mean[i].to_s

      @samplesgrid.set_cell_value i, 13, ym_mean_squares[i].to_s
      @samplesgrid.set_cell_value i, 14, ytm_mean_squares[i].to_s

      @samplesgrid.set_cell_value i, 15, ym_yt[i].to_s
      @samplesgrid.set_cell_value i, 16, ym_yt_squares[i].to_s
    end

    last_row = @samplesgrid.get_number_rows
    last_row -= 1

    # -------------------- SET MEAN VALUES --------------------
    
    # @samplesgrid.set_cell_value last_row, 1, x.mean.to_s
    # @samplesgrid.set_cell_value last_row, 2, y.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 3, products.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 4, xSquares.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 5, @yt.mean.to_s
    # @samplesgrid.set_cell_value last_row, 6, delta_vector.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 7, ySquares.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 8, ym_yt_squares.mean.to_s
    # @samplesgrid.set_cell_value last_row, 9, ytm_mean.mean.to_s
    # @samplesgrid.set_cell_value last_row, 10, ytm_mean_squares.mean.to_s
    
    # @samplesgrid.set_cell_value last_row, 11, ym_mean.mean.to_s
    # @samplesgrid.set_cell_value last_row, 12, ym_mean_squares.mean.to_s
    
     # ajust all sizes
    @samplesgrid.auto_size

    initialize_table_grid
    # table grid
    
    ssr = ytm_mean_squares.sum
    sse = ym_yt_squares.sum
    sst = ym_mean_squares.sum

    msr = ssr / 1.0
    mse = sse / (y.size - 2)

    @tablegrid.set_cell_value 0, 0, ssr.to_s
    @tablegrid.set_cell_value 0, 1, '2'
    @tablegrid.set_cell_value 0, 2, msr.to_s

    @tablegrid.set_cell_value 1, 0, sse.to_s
    @tablegrid.set_cell_value 1, 1, '2'
    @tablegrid.set_cell_value 1, 2, mse.to_s

    @tablegrid.set_cell_value 2, 0, sst.to_s
    @tablegrid.set_cell_value 2, 1, '4'

    @tablegrid.auto_size
    
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

  def initialize_additional_grid
    columns = %w[R_y R2_x1 R_x2]
    
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
    @additonalgrid.set_row_label_value 0, 'R_y'

    @additonalgrid.append_rows
    @additonalgrid.set_row_label_value 1, 'R_x1'

    @additonalgrid.append_rows
    @additonalgrid.set_row_label_value 2, 'R_x2'

    columns.size.times do |i|
      @additonalgrid.append_cols
      @additonalgrid.set_col_label_value i, columns[i]
    end
    @additonalgrid.set_col_label_alignment Wx::ALIGN_CENTRE, Wx::ALIGN_CENTRE
  end

  def initialize_table_grid
    columns = %w[Square_sums Degrees Middle_squares]
    
    @tablegrid.create_grid 0, 0
    
    @tablegrid.enable_editing true
    @tablegrid.enable_grid_lines true
    @tablegrid.set_margins 0, 0
    @tablegrid.auto_size_columns
    @tablegrid.enable_drag_col_move
    @tablegrid.enable_drag_col_size
    @tablegrid.set_col_label_size 30
    @tablegrid.enable_drag_row_size

    @tablegrid.get_number_rows().times do |i|
      @tablegrid.delete_rows
    end

    @tablegrid.get_number_cols().times do |i|
      @tablegrid.delete_cols
    end

    @tablegrid.append_rows
    @tablegrid.set_row_label_value 0, 'SSR'

    @tablegrid.append_rows
    @tablegrid.set_row_label_value 1, 'SSE'

    @tablegrid.append_rows
    @tablegrid.set_row_label_value 2, 'SST'

    columns.size.times do |i|
      @tablegrid.append_cols
      @tablegrid.set_col_label_value i, columns[i]
    end
    @tablegrid.set_col_label_alignment Wx::ALIGN_CENTRE, Wx::ALIGN_CENTRE
  end

  def additionalbutton_click

    initialize_additional_grid

    recalculate_additional
    
    @m_notebook1.set_selection 2
  end

  def recalculate_additional

    x1 = @sample1
    x2 = @sample2
    y = @sample3

    r_yy = SampleOperations.calculate_r y, y
    r_x1y = SampleOperations.calculate_r x1, y
    r_x1x2 = SampleOperations.calculate_r x1, x2
    r_x2x2 = SampleOperations.calculate_r x2, x2
    r_x2y = SampleOperations.calculate_r x2, y
    r_x1x1 = SampleOperations.calculate_r x1, x1

    b1_t = (r_x1y - r_x1x2*r_x2y) / (1 - r_x1x2**2)
    b2_t = (r_x2y - r_x1x2*r_x1y) / (1 - r_x1x2**2)

    var_y = SampleOperations.calculate_var y 
    var_x1 = SampleOperations.calculate_var x1
    var_x2 = SampleOperations.calculate_var x2

    @b1 = b1_t*Math.sqrt(var_y/var_x1)
    @b2 = b2_t*Math.sqrt(var_y/var_x2)
    @b0 = @sample3.mean - @b1*@sample1.mean - @b2*@sample2.mean

    p @b0
    p @b1
    p @b2

    initialize_additional_grid    
    
    @additonalgrid.set_cell_value 0, 0, r_yy.to_s
    @additonalgrid.set_cell_value 0, 1, r_x1y.to_s
    @additonalgrid.set_cell_value 0, 2, r_x2y.to_s

    @additonalgrid.set_cell_value 1, 0, r_x1y.to_s
    @additonalgrid.set_cell_value 1, 1, r_x1x1.to_s
    @additonalgrid.set_cell_value 1, 2, r_x1x2.to_s

    @additonalgrid.set_cell_value 2, 0, r_x2y.to_s
    @additonalgrid.set_cell_value 2, 1, r_x1x2.to_s
    @additonalgrid.set_cell_value 2, 2, r_x2x2.to_s

    @additonalgrid.auto_size


    
  end

  def checkphisherbutton_click
    table_value = @criticalValues[@sample1.size - 2]  # @phishertablevalue.value.to_f

    phisher = @tablegrid.get_cell_value(0, 2).to_f / @tablegrid.get_cell_value(1, 2).to_f

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
    y = @sample2.values
    x = @sample1.values

    x_mean_vector = SampleOperations.get_vector @sample1.mean, @sample1.size
    xm_mean = SampleOperations.delta x, x_mean_vector
    xm_mean_squares = SampleOperations.square_array xm_mean

    ym_yt = SampleOperations.delta y, @yt
    ym_yt_squares = SampleOperations.square_array ym_yt

    sigma_e = Math.sqrt(ym_yt_squares.sum / (@sample2.size - 2.0))
    sigma_b0 = Math.sqrt(sigma_e*@sample1.squares.sum / (@sample1.size.to_f*xm_mean_squares.sum))
    sigma_b1 = Math.sqrt(sigma_e / xm_mean_squares.sum)

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
