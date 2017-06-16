class Attribute
  attr :value, true

  def initialize(value)
    @value = value
  end

  def <=>(v)
    @value <=> v
  end

  def inspect
    "[#{@value}]"
  end

  def to_s
    "[#{@value}]"
  end
    
end
