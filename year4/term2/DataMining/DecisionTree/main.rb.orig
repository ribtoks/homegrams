# $KCODE = 'u'
require 'storage_reader.rb'
require 'attribute.rb'
require 'decision.rb'
require 'graph.rb'
require 'decision_set.rb'

def build_tree(set, node)

  return if set.rules.empty?

  sz = set.rules[0].attributes.size

  if sz == 1
    set.rules[0].attributes.each do |attr|
      # build terminal node in graph
      terminal_node = Node.new(attr.value)
      terminal_node.data = set.rules[0].decision

      node.add_edge terminal_node
    end
        
    attr_name = set.attr_names[0]
    node.data = attr_name    
    return
  end

  info = set.info
  
  # get index and name of splitting attribute
  attr_index = (0...sz).max_by {|i| info - set.infoX(i)}
  attr_name = set.attr_names[attr_index]

  sets = set.split(attr_index)
  # set index of splitted attribute
  # to parent node
  node.data = attr_name

  # for each splitted tree
  sets.each do |attr_value, splitted_set|

    subnode = Node.new(attr_value)

    node.add_edge subnode
    
    # recursively continue building a tree
    build_tree(splitted_set, subnode)
  end
end

set = DecisionsFileReader.read('irisdat',';')#("test1", ',')
tree = Graph.new(:root)

puts set.info
puts set.infoX 0
# build_tree(set, tree.root)

# element = {:attr1 => "a", :attr2 => "5", :attr3 => "0"}

# tree.root.print_node

# puts tree.classify(element)
