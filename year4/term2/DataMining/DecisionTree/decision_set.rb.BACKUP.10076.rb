require 'decision_rule.rb'

module Math
  def self.log2(x)
    log(x)/log(2.0)
  end
end

class DecisionSet
  # array of rules
  attr_reader :rules
  attr_reader :attr_names

  def initialize(rules, attr_names)
    @rules = rules
    @attr_names = attr_names
  end

  # splits set of attributes
  # using one attribute
  def split(attr_index)
    # split rules on some sets
    # based on attribute values
    sets = @rules.group_by {|rule| rule.attributes[attr_index].value }

    attr_names = Array.new(@attr_names)
    attr_names.delete_at(attr_index)
    
    # remove splitted attribute
    # from each set
    sets.each_key do |key|
      rules = sets[key]
      # make deep copy of current obj
      sets[key] = Marshal.load(Marshal.dump(rules))

      sets[key].each do |rule|
        rule.attributes.delete_at(attr_index)
      end
    end
    
    # create decision set from each array
    sets.each do |attr_value, arr|
      sets[attr_value] = DecisionSet.new(arr, attr_names)
    end

    sets
  end

  # returns entropy of current set
  def info
    decisions = @rules.map{|rule| rule.decision}

    decisions_hash = {}
    decisions_hash.default = 0

    decisions.each do |d|
      decisions_hash[d.value] += 1
    end

    puts decisions_hash.inspect
    count = decisions.size.to_f

    sum = 0
    decisions_hash.each do |key, value|
      p = value / count
      sum += p*Math.log2(p)
    end

    -count * sum    
  end

  # returns entropy of current set
  # after splitting it with some attribute
  def infoX(attr_index)

    attr_values = @rules.map{|rule| rule.attributes[attr_index].value }
    attr_values.uniq!

    # [attr_value][decision] -> count
    sets_counts = Hash.new {|hash, key| hash[key] = Hash.new(0) }
    general_counts = {}
    general_counts.default = 0

    @rules.each do |rule|
      sets_counts[ rule.attributes[attr_index].value ][rule.decision.value] += 1
      general_counts[rule.attributes[attr_index].value] += 1
    end

    # now calculate infoX itself
    sum = 0

    sets_counts.each do |attr_value, decisions_hash|
      count = general_counts[attr_value].to_f

      temp_sum = 0
      decisions_hash.each do |key, value|
        p = value / count
        temp_sum += p*Math.log2(p)
      end
      
      sum += (-count * temp_sum)*count / @rules.size
    end

    sum
  end
  
end
