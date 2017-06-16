require 'attribute.rb'
require 'decision.rb'
require 'decision_rule.rb'

class DecisionsFileReader
  class << self
    def read(file, delimiter=',')
      table = []
      first = true
      names = []
            
      File.open(file).each do |line|
        line.strip!
        
        next if line.empty?

        if first
          names = line.split(delimiter)
          names.map! {|name| name.intern}
          first = false
          next
        end
        
        arr = line.split(delimiter)        
        value = Decision.new(arr.pop.intern)
        
        arr.map! {|x| Attribute.new(x)}
        
        table << DecisionRule.new(arr, value)        
      end

      DecisionSet.new(table, names)
    end
  end
end
