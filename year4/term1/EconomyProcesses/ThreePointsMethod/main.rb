require 'wx'
require 'gnuplot'
require File.expand_path(File.dirname(__FILE__)) + '/ThreePointsMethod.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../Statistics/sample.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../Statistics/SampleOperations.rb'

class MyThreePointsMethod < MainThreePointsFrame

  def initialize(parent=nil)
    super

    @sample1 = Sample.new
    @sample2 = Sample.new
    @alpha, @beta, @gamma = 0, 1, 1
    
    evt_button(@submitsamplesbutton) {|event| submit_samples_click }
  end

  def get_mode(arr)
    ind = arr.size / 2
    return arr[ind] if (arr.size % 2) == 1
    (arr[ind] + arr[ind - 1])/2.0
  end

  def submit_samples_click
    sample1str = @xsampletextbox.value
    sample2str = @ysampletextbox.value

    @sample1 = Sample.new
    @sample2 = Sample.new
    
    sample1str.split.each {|x| @sample1 << x.to_f }
    sample2str.split.each {|x| @sample2 << x.to_f }
    
    raise 'Different samples sizes' if @sample1.size != @sample2.size

    case @modelradiobutton.get_selection
    when 0:
        # modificated exponent
        mod_exp_hash = calculate_mod_exp
      alpha = mod_exp_hash['alpha']
      beta = mod_exp_hash['beta']
      gamma = mod_exp_hash['gamma']

      @alphalabel.set_label alpha.to_s
      @betalabel.set_label beta.to_s
      @gammalabel.set_label gamma.to_s
      
      modif_exp_string = "#{alpha}*(#{beta}**x)+#{gamma}"

      draw_graphics modif_exp_string
    when 1:
        # usual exponent
        exp_hash = calculate_exp
      alpha = exp_hash['alpha']
      beta = exp_hash['beta']

      exp_string = "#{alpha}*(#{beta}**x)"

      @alphalabel.set_label alpha.to_s
      @betalabel.set_label beta.to_s

      draw_graphics exp_string
    when 2:
        # hyperbolic
        hyp_hash = hyperbolic_func
      b0 = hyp_hash['b0']
      b1 = hyp_hash['b1']

      hyp_string = "#{b0}+#{b1}/x"

      @alphalabel.set_label b0.to_s
      @betalabel.set_label b1.to_s
      
      draw_graphics hyp_string
    end

  end

  def calculate_covariation(func_string)
    str = %[def func(x)\n #{func_string}\n end]
    func = eval(str)

    numerator = 0
    y_mean = @sample2.mean
    @sample2.values.each do |y|
      numerator += (y - y_mean)*(func())
    end
  end

  def calculate_mod_exp
    # split arrays
    m = (@sample1.size / 3).floor.to_i
    x_parts = []

    y_parts = []

    i1, i2 = 0

    r = @sample1.size % 3
    case r
    when 0:
        i1 = m
      i2 = 2*m
    when 1:
        i1 = m
      i2 = 2*m + 1
    when 2:
        i1 = m + 1
      i2 = 2*m + 1
    end

    size = @sample1.size

    x_parts << @sample1.values[0...i1]
    y_parts << @sample2.values[0...i1]

    x_parts << @sample1.values[i1...i2]
    y_parts << @sample2.values[i1...i2]

    x_parts << @sample1.values[i2...size]
    y_parts << @sample2.values[i2...size]

    x_modes = x_parts.map{|arr| get_mode(arr)}
    y_modes = y_parts.map{|arr| get_mode(arr)}

    p x_modes
    p y_modes

    delta = x_modes[1] - x_modes[0]

    
    beta = ((y_modes[2] - y_modes[1])/(y_modes[1] - y_modes[0]))**(1.0/delta)
    
    alpha = (y_modes[1] - y_modes[0])/(beta**x_modes[1] - beta**x_modes[0])
    
    gamma = y_modes[0] - alpha*beta**x_modes[0]

    p beta
    p alpha
    p gamma

    {'alpha' => alpha, 'beta' => beta, 'gamma' => gamma}
  end

  def calculate_exp
    denominator = 0

    ln_square_sum = @sample1.values.map{|x| Math.log(x)**2}.mean
    square_ln_sum = @sample1.values.map{|x| Math.log(x)}.mean**2

    x_ln_sum = @sample1.values.map{|x| Math.log(x)}.mean
    x_y_ln_sum = 0
    @sample1.size.times do |i|
      x_y_ln_sum += Math.log(@sample2.values[i])*Math.log(@sample1.values[i])
    end
    x_y_ln_sum /= @sample1.size

    beta = (x_y_ln_sum - @sample2.mean*x_ln_sum) / (ln_square_sum - square_ln_sum)
    alpha = Math.exp(@sample2.mean - beta*x_ln_sum)

    {'alpha' => alpha, 'beta' => beta}    
  end

  def hyperbolic_func

    sample = Sample.new
    @sample1.values.each do |x|
      sample << 1.0/x
    end
    
    b1 = SampleOperations.calculate_b1 sample, @sample2
    b0 = SampleOperations.calculate_b0 sample, @sample2, b1

    {'b0' => b0, 'b1' => b1}
  end

  def draw_graphics(str)
    Gnuplot.open do |gp|
      Gnuplot::Plot.new( gp ) do |plot|
        
        plot.title  "Regression graphics"
        # plot.ylabel "x"
        # plot.xlabel "x^2"
        
        x = @sample1.values
        y = @sample2.values
        
        plot.data << Gnuplot::DataSet.new( [x, y] ) do |ds|
          ds.with = "points"
          ds.notitle
        end

        plot.data << Gnuplot::DataSet.new(str) do |ds|
          ds.with = "lines"
          ds.notitle
        end
        
      end
    end
  end
  
end


Wx::App.run do
  MyThreePointsMethod.new.show
end
