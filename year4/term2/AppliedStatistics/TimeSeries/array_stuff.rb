class Array
  def sum
    self.inject(:+)
  end

  def mean
    sum / self.size.to_f
  end

  def diffs
    diffs_arr = [] # Array.new(arr.size - 1)

    (self.size - 1).times do |i|
      next if self[i].nil?
      diffs_arr[i + 1] = self[i + 1] - self[i]
    end

    diffs_arr
  end

  def max_abs
    self.compact.max {|a,b| a.abs <=> b.abs}.abs
  end
end
