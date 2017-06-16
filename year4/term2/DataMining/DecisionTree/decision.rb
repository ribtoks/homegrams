class Decision
  attr :value, true

  def initialize(value)
    @value = value
  end

  def inspect
    "{#{@value}}"
  end

  def to_s
    "{#{@value}}"
  end
end
