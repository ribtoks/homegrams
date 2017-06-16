class FuzzySet

  # create getters and setters
  attr :set, true
  attr :set_func

  # constructor
  def initialize(set_func)

    @set = {}
    @set_func = set_func    
    
  end

  def set_func(x)

    @set_func.call x
    
  end

  def [](x)
    unless @set.has_key? x
      @set[x] = @set_func.call x
    end

    @set[x]
  end

  def <<(x)
    unless @set.has_key? x
      @set[x] = @set_func.call x
    end
  end

  def concentrate
    copy = FuzzySet.new( lambda { |x| (@set_func.call(x))**2 } )
    copy.set.merge! self.set
    copy.recalculate!
    copy
  end

  def concentrate!    
    @set.each_key { |key| @set[key] = @set[key]**2  }
  end

  def recalculate!
    @set.each_key { |key| @set[key] = @set_func.call key }
  end

  def FuzzySet.min(set1, set2)
    result_set = FuzzySet.new( lambda { |x| [set1.set_func(x), set2.set_func(x)].min } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.max(set1, set2)
    result_set = FuzzySet.new( lambda { |x| [set1.set_func(x), set2.set_func(x)].max } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.lower_sum(set1, set2)
    result_set = FuzzySet.new( lambda { |x| [set1.set_func(x) + set2.set_func(x) - 1, 0].max } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.upper_sum(set1, set2)
    result_set = FuzzySet.new( lambda { |x| return [set1.set_func(x) + set2.set_func(x), 1].min } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.product(set1, set2)
    result_set = FuzzySet.new( lambda { |x| set1.set_func(x) * set2.set_func(x) } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.xor(set1, set2)
    result_set = FuzzySet.new(
                              lambda { |x|
                                f1 = set1.set_func(x)
                                f2 = set2.set_func(x)
                                f1 + f2 - f1*f2
                              } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.lower_difference(set1, set2)
    result_set = FuzzySet.new( lambda { |x| [set1.set_func(x) - set2.set_func(x), 0].max } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end

  def FuzzySet.difference(set1, set2)
    result_set = FuzzySet.new( lambda { |x| 1 - set2.set_func(x) } )

    result_set.set = set1.set.merge(set2.set)
    result_set.recalculate!
    
    result_set
  end
  
end
