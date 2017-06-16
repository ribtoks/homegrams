require 'wx'
require File.expand_path(File.dirname(__FILE__)) + '/../classes/interval.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../classes/interval_transformer.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../classes/function_solver.rb'
require File.expand_path(File.dirname(__FILE__)) + '/../visual/ia_interface.rb'
require 'gnuplot'

class MyTransformer1 < IntervalTransformer
  def initialize
    super
  end

  def transform(interval)
    Interval.sin(interval)
  end
end

class MyDerivTransformer1 < IntervalTransformer
  def initialize
    super
  end

  def transform(interval)
    Interval.cos(interval)
  end
end


def my_task_function(x)
  #Math.log(x**2 + 2*x + 3) - (Math::PI*x - 1)*Math.sin(Math::PI*x + 1)
  Math.exp(x - x*x) - 2 - Math.log(x*x + 1)/Math.log(0.5)
end

def my_task_derivative(x)
  #pi = Math::PI
  #(2*x + 2)/(x**2 + 2*x + 3) + pi*Math.cos(pi*x + 1) - pi*Math.sin(pi*x + 1) - pi*pi*x*Math.cos(pi*x + 1)
  (1 - 2*x)*Math.exp(x - x*x) + 2*x/((x*x + 1)*Math.log(2))
end

class MyTaskTransformer < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    #temp_interval1 = Interval.ln(Interval.sqr(int) + int*2 + 3)
    #temp_interval2 = (int*Math::PI - 1)*Interval.sin(int*Math::PI + 1)

    #temp_interval1 - temp_interval2

    Interval.exp(int - Interval.sqr(int)) - 2 - Interval.log(0.5, Interval.sqr(int) + 1)    
  end  
end

class MyTaskDerivTransformer < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    # temp_interval1 = (int*2 + 2)/(Interval.sqr(int) + int*2 + 3)
    # temp_interval2 = Interval.cos(int*Math::PI + 1)*Math::PI
    # temp_interval3 = Interval.sin(int*Math::PI + 1)*Math::PI
    # temp_interval4 = Interval.cos(int*Math::PI + 1)*int*Math::PI*Math::PI
    
    # result = temp_interval2 - temp_interval3 - temp_interval4

    # if (temp_interval1.kind_of? Array)
    #  result = Interval.union2(temp_interval1[0] + result, temp_interval1[1] + result)
    # else
    #  result = temp_interval1 + result
    # end

    temp_interval1 = Interval.exp(int - Interval.sqr(int))
    temp_interval1 = temp_interval1*(int*(-2.0) + 1)

    temp_interval2 = (int*2.0)/(Interval.sqr(int) + 1)
    temp_interval2 = temp_interval2 / (Math.log(2.0))

    temp_interval1 + temp_interval2    
  end
end

def my_polynom(x)
  (x - 1)*(x - 2)*(x - 3)
end

class MyPolynomTransformer < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    (int - 1)*(int - 2)*(int - 3)
  end  
end

class IntervalAnalysisMainFrame < IntervalAnalysisFrame
  def initialize(parent=nil)
    super

    evt_button(@drawgraphicbutton) {|event| draw_graphics_click }
    evt_button(@runbutton) {|event| run_calculations_click }
  end

  def draw_graphics_click

    left_bound = @leftboundtextbox.value.to_f
    right_bound = @rightboundtextbox.value.to_f

    left_bound = -20.0 if left_bound.nil? or left_bound.zero?
    right_bound = 20.0 if right_bound.nil? or right_bound.zero?
    
    Gnuplot.open do |gp|
      Gnuplot::Plot.new( gp ) do |plot|
        
        plot.title  "Task graphics"
        plot.xrange "[#{left_bound}:#{right_bound}]"

        plot.data << Gnuplot::DataSet.new( "0" ) do |ds|
          ds.with = "lines"
          ds.notitle
        end

        if (@taskradiobutton.get_selection == 0)
          plot.data << Gnuplot::DataSet.new( "(x - 1)*(x - 2)*(x - 3)" ) do |ds|
            ds.with = "lines"
            ds.notitle
          end
        else
          plot.data << Gnuplot::DataSet.new( "exp(x - x*x) - 2 + log(x*x + 1)/log(2)" ) do |ds|
          #plot.data << Gnuplot::DataSet.new( "log(x*x+2*x+3)-(pi*x - 1)*sin(pi*x+1)" ) do |ds|
          #plot.data << Gnuplot::DataSet.new( "(2*x+2)/(x*x+2*x+3) + pi*cos(pi*x+1)-pi*sin(pi*x+1)-pi*pi*x*cos(pi*x+1)" ) do |ds|
            ds.with = "lines"
            ds.notitle
          end
        end
        
      end
    end
  end

  def run_calculations_click
    @resultstextbox.clear

    epsilon = @epsilontextbox.value.to_f
    a = @leftboundtextbox.value.to_f
    b = @rightboundtextbox.value.to_f
    interval = Interval.new(a, b)

    case @taskradiobutton.get_selection
    when 0:
        # polynomial
        binaryFinder = BinaryFinder.new(MyPolynomTransformer.new)
      results = binaryFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
      binaryFinder.log.each do |s| @resultstextbox.append_text(s + "\n\r") end

      @resultstextbox.append_text "-----------------------------------\n\r"
      @resultstextbox.append_text results.inspect
      @resultstextbox.append_text "\n\r"
      @resultstextbox.append_text "#{binaryFinder.steps_counter} steps"
      
    when 1:
        # Moore method
        newtonFinder = NewtonMooreZerosFinder.new(lambda {|x| my_task_function(x)}, MyTaskDerivTransformer.new)
      results = newtonFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
      newtonFinder.log.each do |s| @resultstextbox.append_text(s + "\n\r") end

      
      @resultstextbox.append_text "-----------------------------------\n\r"
      @resultstextbox.append_text results.inspect
      @resultstextbox.append_text "\n\r"
      @resultstextbox.append_text "#{newtonFinder.steps_counter} steps"
      
    when 2:
        # Hansen method
        newtonFinder = NewtonHansenZerosFinder.new(lambda {|x| my_task_function(x)}, MyTaskDerivTransformer.new)
      results = newtonFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
      newtonFinder.log.each do |s| @resultstextbox.append_text(s + "\n\r") end

      
      @resultstextbox.append_text "-----------------------------------\n\r"
      @resultstextbox.append_text results.inspect
      @resultstextbox.append_text "\n\r"
      @resultstextbox.append_text "#{newtonFinder.steps_counter} steps"
    when 3:
        # Kravchyk method
        newtonFinder = NewtonKravchykZerosFinder.new(lambda {|x| my_task_function(x)}, lambda {|x| my_task_derivative(x)}, MyTaskDerivTransformer.new)
      results = newtonFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
      newtonFinder.log.each do |s| @resultstextbox.append_text(s + "\n\r") end

      
      @resultstextbox.append_text "-----------------------------------\n\r"
      @resultstextbox.append_text results.inspect
      @resultstextbox.append_text "\n\r"
      @resultstextbox.append_text "#{newtonFinder.steps_counter} steps"
    end
    
  end
  
end

Wx::App.run do
  IntervalAnalysisMainFrame.new.show
end
