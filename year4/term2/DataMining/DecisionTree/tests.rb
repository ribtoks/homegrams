# $KCODE = 'u'
require 'storage_reader.rb'
require 'attribute.rb'
require 'decision.rb'
require 'graph.rb'
require 'decision_set.rb'

set = DecisionsFileReader.read("test1")
attr_index = 2

sets = set.split attr_index

sum = 0
sets.each_value do |s|
  sum += s.rules.size.to_f * s.info / set.rules.size
end

puts sum
puts set.infoX(attr_index)
