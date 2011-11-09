require File.expand_path(File.dirname(__FILE__)) + '/classes/interval.rb'
require File.expand_path(File.dirname(__FILE__)) + '/classes/interval_transformer.rb'
require File.expand_path(File.dirname(__FILE__)) + '/classes/function_solver.rb'

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

class MyTransformer2 < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    Interval.sqr(int) - int*10 + 9
  end
end

def my_func2(x)
  x**2 - 10.0*x + 9.0
end

class MyDerivTransformer2 < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    int*2 - 10
  end
end

class MyTaskTransformer < IntervalTransformer
  def initialize
    super
  end

  def transform(int)
    
  end  
end

def my_task_function(x)
  Math.log(x**2 + 2*x + 3) - (Math::PI*x - 1)*Math.sin(Math::PI*x + 1)
end

epsilon = 0.0000001
interval = Interval.new(-70, 100)

binaryFinder = BinaryFinder.new(MyTransformer2.new)
p binaryFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
p binaryFinder.steps_counter

newtonFinder = NewtonHansenZerosFinder.new(lambda {|x| my_func2(x)}, MyDerivTransformer2.new)
p newtonFinder.solve(epsilon, interval).uniq_eps(2*epsilon)
p newtonFinder.steps_counter
