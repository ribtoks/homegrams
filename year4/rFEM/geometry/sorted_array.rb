class SortedArray < Array

  def self.[] *array
    SortedArray.new(array)
  end

  def initialize array=nil
    super( array.sort ) if array
  end

  def << value
    insert index_of_last_LE(value), value
  end

  alias push <<
  alias shift <<

  def index_of_last_LE value
    # puts "Insert #{value} into #{inspect}"
    l, r = 0, length-1
    while l <= r
      m = (r+l) / 2
      # puts "#{l}(#{self[l]})--#{m}(#{self[m]})--#{r}(#{self[r]})"
      if value < self[m]
        r = m - 1
      else
        l = m + 1
      end
    end
    # puts "Answer: #{l}:(#{self[l]})"
    l
  end
end
