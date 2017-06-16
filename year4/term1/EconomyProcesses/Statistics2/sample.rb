
class SortedArray < Array
  
  def initialize(*args, &sort_by)
    @sort_by = sort_by || Proc.new { |x,y| x <=> y }
    super(*args)
    sort! &sort_by
  end
  def insert(i, v)
    # The next line could be further optimized to perform a
    # binary search.
    insert_before = index(find { |x| @sort_by.call(x, v) == 1 })
    super(insert_before ? insert_before : -1, v)
  end
  def <<(v)
    insert(0, v)
  end
  
  alias push <<
  alias unshift <<
    ["collect!", "flatten!", "[]="].each do |method_name|
    module_eval %{
      def #{method_name}(*args)
        super
        sort! &@sort_by
      end
    }
  end
  def reverse!
    #Do nothing; reversing the array would disorder it.
  end
end

class Array
  def sum
    self.inject{|sum,x| sum + x }
  end

  def mean
    sum/size
  end

  def squares
    arr = []
    self.each {|key| arr << key*key}
    arr
  end

  def *(sample)
    if sample.instance_of? Array
      arr1 = self
      arr2 = sample
      result = []
      raise ArgumentError.new('Different sizes of samples') if arr1.size != arr2.size

      arr1.size.times do |i|
        result << arr1[i]*arr2[i]
      end
      result
    else
      nil
    end
  end
  
end

class Sample
  
  def initialize
    @values = SortedArray.new
  end

  def <<(value)
    @values << value
  end

  def sum
    sum = 0
    @values.each {|key| sum += key}
    sum
  end

  def values
    @values
  end

  def clear
    @values.clear
  end

  def size
    @values.size
  end

  def squares
    arr = []
    @values.each {|key| arr << key*key}
    arr
  end

  def mean
    sum/@values.size
  end

  def *(sample)
    if sample.instance_of? Sample
      arr1 = @values
      arr2 = sample.values
      result = []
      raise ArgumentError.new('Different sizes of samples') if arr1.size != arr2.size

      arr1.size.times do |i|
        result << arr1[i]*arr2[i]
      end
      result
    else
      nil
    end
  end
  
end
