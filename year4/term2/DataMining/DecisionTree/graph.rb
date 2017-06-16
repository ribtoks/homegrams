class Node
  # appropriate apex
  attr_reader :subnodes
  # value of node
  attr :value, true
  # data of node
  attr :data, true

  def initialize(value)
    @value = value
    @subnodes = []
    @data = nil
  end

  def is_terminal?
    @subnodes.empty?
  end

  def print_node
    puts to_str
  end

  def to_str(depth=0)
    str = ""
    depth.times {|i| str += "\t" }

    str += "node [value=#{@value.inspect} data=#{@data.inspect}]\r\n"

    @subnodes.each do |sn|
      str += sn.to_str(depth + 1)
    end

    str
  end

  def inspect
    "#obj val=#{@value}"
  end

  def add_edge(node)
    @subnodes << node
  end

  def classify(element)
    if is_terminal?
      return @data
    end

    attr_name = @data

    @subnodes.each do |node|
      if node.value == element[attr_name]
        return node.classify(element)
      end
    end
    
    return nil
  end  
end

class Graph
  attr_reader :root

  def initialize(root_value)
    # create head apex
    @root = Node.new(root_value)
  end

  def inspect
    @root.inspect
  end

  def classify(element)
    @root.classify element
  end

end
