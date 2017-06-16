require File.expand_path(File.dirname(__FILE__)) + '/interval.rb'

class IntervalTransformer

  def initialize
  end

  def transform(interval)
    raise NotImplementedError.new("#{self.class.name} is an abstract class")
  end
  
end

class Array

  def sum
    sum = 0.0
    self.each {|x| sum += x}
    sum
  end

  def mean
    sum/size
  end
  
  def uniq_eps(epsilon)

    return [] if empty?

    arr = self.sort

    output = []
    curr = arr.first
    currArr = []

    arr.each do |x|

      if (x - curr).abs <= epsilon
        currArr << x
        curr = x
      else
        output << currArr
        currArr = [x]
        curr = x
      end
      
    end

    output << currArr

    output.map! {|x| x.mean}
    output
  end
end
