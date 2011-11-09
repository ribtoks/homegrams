#!/usr/bin/ruby
require 'wx'
require 'fuzzysets'
require 'FuzzySet'
require File.expand_path(File.dirname(__FILE__)) + '/../GraphicsPlotter/FunctionsPlotter.rb'

class FuzzySetsMainFrame < FuzzySetsFrame
  
  def initialize
    
    super

    evt_button(drawbutton) {|event| draw_button_click }
    
  end


  def draw_button_click
    #dlg = Wx::MessageDialog.new(self, "Hello from Ruby and wxRuby!", "A Message Box", Wx::OK | Wx::ICON_INFORMATION)

    #dlg.show_modal

    @a = @leftboundtextbox.value
    @b = @rightboundtextbox.value

    @color1 = 'steelblue' # @color1choise.get_selection #choices[@color1choise.selection]
    @color2 = 'seagreen' # @color2choise.get_selection #choices[@color2choise.selection]

    @result_color = 'maroon' # @resultcolorchoise.get_selection #choices[@resultcolorchoise.selection]

    str = %[def func1(x)\n #{@function1textbox.value}\n end]
    func1 = eval(str)

    str = %[def func2(x)\n #{@function2textbox.value}\n end]
    func2 = eval(str)

    set1 = FuzzySet.new(lambda{ |x| func1 x})
    set2 = FuzzySet.new(lambda{ |x| func2 x})

    result_set = nil

    case @operationchoise.selection
    when 0:
        result_set = FuzzySet.min(set1, set2)
    when 1:
        result_set = FuzzySet.max(set1, set2)
    when 2:
        result_set = FuzzySet.difference(set1, set2)
    when 3:
        result_set = FuzzySet.lower_sum(set1, set2)
    when 4:
        result_set = FuzzySet.upper_sum(set1, set2)
    when 5:
        result_set = FuzzySet.product(set1, set2)
    when 6:
        result_set = FuzzySet.xor(set1, set2)
    when 7:
        result_set = FuzzySet.lower_difference(set1, set2)
    when 8:
        result_set = set1.concentrate
    end

    funcPlotter = FunctionPlotter.new(@a.to_i, @b.to_i, 0.001, [lambda{|x| set1.set_func x}, lambda{|x| set2.set_func x}, lambda{|x| result_set.set_func x}],
                                      "Global title", [@color1, @color2, @result_color],
                                      'white', 800, 600, 3)
    funcPlotter.process_graphics
    funcPlotter.out_graphics(0)

    @m_bitmap1.set_bitmap Wx::Bitmap.new("myGraphic.png")
    
  end
  
end

Wx::App.run do
  FuzzySetsMainFrame.new.show
end

