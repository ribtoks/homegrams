require File.expand_path(File.dirname(__FILE__)) + '/sample.rb'

class Array

  def sum
    sum = 0
    self.each {|x| sum += x}
    sum
  end
  
  def mean
    sum/self.size
  end
end

class SampleOperations
  class << self

    def square_array(sample_values)
      arr = []

      sample_values.each {|x| arr << x*x}
      arr
    end

    def delta(arr1, arr2)
      arr = []

      raise ArgumentError.new('Different sizes of samples') if arr1.size != arr2.size

      arr1.size.times do |i|
        arr << arr1[i] - arr2[i]
      end
      arr
    end

    def calculate_b1(sample1, sample2)
      s1mean = sample1.mean
      s2mean = sample2.mean
      ((sample1*sample2).mean - s1mean*s2mean)/(sample1.squares.mean - s1mean*s1mean)
    end

    def calculate_b0(sample1, sample2, b1)
      sample2.mean - b1*sample1.mean
    end

    def get_rvector(sample_values, b0, b1)
      arr = []
      sample_values.each {|x| arr << b0 + b1*x}
      arr
    end

    def get_vector(value, count)
      arr = []
      count.times do |i|
        arr << value
      end
      arr
    end
    
  end
end
