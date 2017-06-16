require File.expand_path(File.dirname(__FILE__)) + '/../GraphicsPlotter/FunctionsPlotter.rb'
require 'FuzzySet'

a,b = ''

#print "Enter left bound: "
a = -5

#print "Enter right bound: "
b = 5

print "Enter operation number:"
opType = gets
opType = opType.to_i

if opType == 9
  func1_str = ''
  print "Enter function 1. y(x) = "
  func1_str = gets

  str = %[def func1(x)\n #{func1_str}\n end]
  func1 = eval(str)

  set1 = FuzzySet.new(lambda{ |x| func1 x})

  result_set = set1.concentrate

  funcPlotter = FunctionPlotter.new(a.to_i, b.to_i, 0.001, [lambda{|x| set1.set_func x}, lambda{|x| result_set.set_func x}],
                                  "Global title", ['steelblue', 'maroon'],
                                  'white', 800, 600, 3)


  funcPlotter.process_graphics
  funcPlotter.out_graphics(0)
else

  func1_str = ''
#  print "Enter function 1. y(x) = "
  func1_str = 'Math.cos(2*x).abs*0.5'
  
  func2_str = ''
#  print "Enter function 2: y(x) = "
  func2_str = '0.75*Math.sin(x)**2'
  
  str = %[def func1(x)\n #{func1_str}\n end]
  func1 = eval(str)
  
  str = %[def func2(x)\n #{func2_str}\n end]
  func2 = eval(str)
  
  set1 = FuzzySet.new(lambda{ |x| func1 x})
  set2 = FuzzySet.new(lambda{ |x| func2 x})
  
  result_set = nil
  
  case opType
  when 1:
      result_set = FuzzySet.min(set1, set2)
  when 2:
      result_set = FuzzySet.lower_sum(set1, set2)
  when 3:
      result_set = FuzzySet.difference(set1, set2)
  when 4:
      result_set = FuzzySet.product(set1, set2)
  when 5:
      result_set = FuzzySet.max(set1, set2)
  when 6:
      result_set = FuzzySet.upper_sum(set1, set2)
  when 7:
      result_set = FuzzySet.xor(set1, set2)
  when 8:
      result_set = FuzzySet.lower_difference(set1, set2)
  end
  
  funcPlotter = FunctionPlotter.new(a.to_i, b.to_i, 0.001, [lambda{|x| set1.set_func x}, lambda{|x| set2.set_func x}, lambda{|x| result_set.set_func x}],
                                    "Global title", ['steelblue', 'seagreen', 'maroon'],
                                    'white', 800, 600, 3)
  
  
  funcPlotter.process_graphics
  funcPlotter.out_graphics(0)
end
