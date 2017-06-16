require File.expand_path(File.dirname(__FILE__)) + '/interval.rb'

class ZerosFinder

  attr :funcTransformer, true
  attr_reader :zeros, :steps_counter
  attr :epsilon, true
  attr :log
  
  def initialize (funcTransformer, write_to_log=true)
    @funcTransformer = funcTransformer
    @zeros = []
    @write_to_log = write_to_log
    @log = [] if write_to_log
  end

  def solve(epsilon, interval)
    raise NotImplementedError.new("#{self.class.name} is an abstract class")
  end
  
end

class BinaryFinder < ZerosFinder
  
  def initialize (funcTransformer, write_to_log=true)
    super(funcTransformer, write_to_log)
  end

  def solve(epsilon, interval)
    @steps_counter = 0
    @zeros.clear
    @epsilon = epsilon
    @log.clear if @write_to_log
    inner_solve(interval)

    @zeros
  end

  private
  def inner_solve(interval)
    # solves problem using binary search

    # main loop of algorithm
    while interval.width > @epsilon
      @steps_counter += 1
      if @write_to_log
        lower_value = funcTransformer.transform Interval.new(interval.a, interval.a)
        middle_value = funcTransformer.transform Interval.new(interval.middle, interval.middle)
        upper_value = funcTransformer.transform Interval.new(interval.b, interval.b)
        @log << "#{interval} || #{interval.width} || #{lower_value.middle} || #{middle_value.middle} || #{upper_value.middle}"
      end

      newIntervals = interval.divide2

      funcIntervals = newIntervals.map { |i| @funcTransformer.transform(i) }
      containsZero = funcIntervals.map do |i| i.contains_zero? end
      hash = Hash[funcIntervals.zip(containsZero)]

      # remove intervals with no zeros
      results = hash.delete_if {|key, value| value == false }

      # if no zeros - return
      break if results.empty?

      # if there're two zeros - solve other
      # interval with this method
      if results.size == 2

        if newIntervals[1].width <= @epsilon
          @zeros << newIntervals[0].b
          break
        else
          inner_solve(newIntervals[1])
          interval = newIntervals[0]
        end
        
      else
        interval = newIntervals[ results.has_key?(funcIntervals[0]) ? 0 : 1 ]
      end

      # save current solution if out interval is pretty small
      @zeros << interval.middle if interval.width <= @epsilon
      
    end
    
  end

end

class NewtonMooreZerosFinder < ZerosFinder

  def initialize(function, derivativeTransformer, write_to_log=true)
    super(nil, write_to_log)
    @function = function
    @derivTransformer = derivativeTransformer
  end

  def solve(epsilon, interval)
    @steps_counter = 0
    @epsilon = epsilon
    @log.clear if @write_to_log
    inner_solve interval
    @zeros
  end

  private
  def inner_solve(interval)
    curr_interval = Interval.new(interval)
   
    while curr_interval.width > @epsilon

      if @write_to_log
        lower_value = @function.call(interval.a)
        middle_value = @function.call(interval.middle)
        upper_value = @function.call(interval.b)
        
        @log << "#{interval} || #{interval.width} || #{lower_value} || #{middle_value} || #{upper_value}"
      end

      curr_point = curr_interval.middle

      temp_interval = Interval.new curr_point
      func_interval = Interval.new @function.call(curr_point)

      deriv_interval = @derivTransformer.transform curr_interval

      if deriv_interval.contains_zero?
        arr = curr_interval.divide2
        inner_solve arr[0]
        inner_solve arr[1]
        break
      end

      next_interval = temp_interval -
        func_interval/deriv_interval

      next_interval = Interval.intersect2 curr_interval, next_interval
      
      break if next_interval.empty?

      if next_interval.width > 0.5*curr_interval.width
        arr = curr_interval.divide2
        inner_solve arr[0]
        inner_solve arr[1]
        break
      end

      @zeros << next_interval.middle if next_interval.width <= @epsilon
      @steps_counter += 1

      if @function.call(next_interval.middle).abs <= @epsilon
        @zeros << next_interval.middle
        break
      end
      
      curr_interval = next_interval
    end
    
  end
  
end

class NewtonHansenZerosFinder < ZerosFinder

  def initialize(function, derivativeTransformer)
    super(nil)
    @function = function
    @derivTransformer = derivativeTransformer
  end

  def solve(epsilon, interval)
    @steps_counter = 0
    @epsilon = epsilon
    inner_solve interval
    @zeros
  end

  private
  def inner_solve(interval)
    
    curr_interval = Interval.new(interval)
   
    while curr_interval.width > @epsilon

      if @write_to_log
        lower_value = @function.call(interval.a)
        middle_value = @function.call(interval.middle)
        upper_value = @function.call(interval.b)
        
        @log << "#{interval} || #{interval.width} || #{lower_value} || #{middle_value} || #{upper_value}"
      end

      curr_point = curr_interval.a

      temp_interval = Interval.new curr_point
      func_interval = Interval.new @function.call(curr_point)

      deriv_interval = @derivTransformer.transform curr_interval

      conversed = deriv_interval.converse

      # if division by zero and
      # more, than one interval
      if conversed.kind_of? Array
        conversed.map! {|i| temp_interval -
          func_interval*i}

        conversed << curr_interval
        intersected = Interval.intersect conversed

        is_bad = false
        intersected.each do |i|
          if i.width > 0.5*curr_interval.width
            is_bad = true
            break
          end
        end

        if is_bad
          arr = curr_interval.divide2
          inner_solve arr[0]
          inner_solve arr[1]
          break
        end
                
        inner_solve intersected[0]
        inner_solve intersected[1]
        break
      end

      next_interval = temp_interval -
        func_interval/deriv_interval

      next_interval = Interval.intersect2 curr_interval, next_interval

      break if next_interval.empty?

      if next_interval.width > 0.5*curr_interval.width
        arr = curr_interval.divide2
        inner_solve arr[0]
        inner_solve arr[1]
        break
      end

      @zeros << next_interval.middle if next_interval.width <= @epsilon
      @steps_counter += 1

      if @function.call(next_interval.middle).abs <= @epsilon
        @zeros << next_interval.middle
        break
      end
      
      curr_interval = next_interval
    end
    
  end
  
end

class NewtonKravchykZerosFinder < ZerosFinder

  def initialize(function, functionDeriv, derivativeTransformer, write_to_log=true)
    super(nil, write_to_log)
    @function = function
    @functionDeriv = functionDeriv
    @derivTransformer = derivativeTransformer
  end

  def solve(epsilon, interval)
    @steps_counter = 0
    @epsilon = epsilon
    @log.clear if @write_to_log
    inner_solve interval
    @zeros
  end

  private
  def inner_solve(interval)
    curr_interval = Interval.new(interval)
   
    while curr_interval.width > @epsilon

      # save data to log
      if @write_to_log
        lower_value = @function.call(interval.a)
        middle_value = @function.call(interval.middle)
        upper_value = @function.call(interval.b)
        
        @log << "#{interval} || #{interval.width} || #{lower_value} || #{middle_value} || #{upper_value}"
      end

      curr_point = curr_interval.middle

      # calculate interval using kravchyk formula
      temp_interval = Interval.new curr_point
      deriv_point = @functionDeriv.call(curr_point)
      func_interval = Interval.new(@function.call(curr_point)/deriv_point)

      deriv_interval = @derivTransformer.transform curr_interval

      deriv_interval = Interval.new(1.0) - deriv_interval / deriv_point
      deriv_interval = deriv_interval * (curr_interval - temp_interval)
      
      next_interval = temp_interval - func_interval + deriv_interval
      
      next_interval = Interval.intersect2 curr_interval, next_interval
      
      break if next_interval.empty?

      if next_interval.width > 0.5*curr_interval.width
        arr = curr_interval.divide2
        inner_solve arr[0]
        inner_solve arr[1]
        break
      end

      @zeros << next_interval.middle if next_interval.width <= @epsilon
      @steps_counter += 1

      if @function.call(next_interval.middle).abs <= @epsilon
        @zeros << next_interval.middle
        break
      end
      
      curr_interval = next_interval
    end
    
  end
  
end
