class DecisionRule
  # array of decision attributes
  attr :attributes, true
  
  # decision itself
  attr :decision, true

  def initialize(attributes, value)
    @attributes = attributes
    @decision = value
  end

  def inspect
    "{#{@attributes.inspect}} -> #{@decision}"
  end

  def to_s
    "{#{@attributes.to_s}} -> #{@decision}"
  end
end
