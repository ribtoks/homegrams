
# This class was automatically generated from XRC source. It is not
# recommended that this file is edited directly; instead, inherit from
# this class and extend its behaviour there.  
#
# Source file: fuzzysets.xrc 
# Generated at: Fri Oct 01 13:01:28 +0300 2010

class FuzzySetsFrame < Wx::Frame
	
	attr_reader :m_statictext4, :leftboundtextbox, :m_statictext5,
              :rightboundtextbox, :m_statictext1, :function1textbox,
              :color1choise, :m_statictext2, :function2textbox,
              :color2choise, :m_statictext3, :operationchoise,
              :resultcolorchoise, :drawbutton, :m_bitmap1
	
	def initialize(parent = nil)
		super()
		xml = Wx::XmlResource.get
		xml.flags = 2 # Wx::XRC_NO_SUBCLASSING
		xml.init_all_handlers
		xml.load("fuzzysets.xrc")
		xml.load_frame_subclass(self, parent, "MainFrame")

		finder = lambda do | x | 
			int_id = Wx::xrcid(x)
			begin
				Wx::Window.find_window_by_id(int_id, self) || int_id
			# Temporary hack to work around regression in 1.9.2; remove
			# begin/rescue clause in later versions
			rescue RuntimeError
				int_id
			end
		end
		
		@m_statictext4 = finder.call("m_staticText4")
		@leftboundtextbox = finder.call("leftBoundTextBox")
		@m_statictext5 = finder.call("m_staticText5")
		@rightboundtextbox = finder.call("rightBoundTextBox")
		@m_statictext1 = finder.call("m_staticText1")
		@function1textbox = finder.call("function1TextBox")
		@color1choise = finder.call("color1Choise")
		@m_statictext2 = finder.call("m_staticText2")
		@function2textbox = finder.call("function2TextBox")
		@color2choise = finder.call("color2Choise")
		@m_statictext3 = finder.call("m_staticText3")
		@operationchoise = finder.call("operationChoise")
		@resultcolorchoise = finder.call("resultColorChoise")
		@drawbutton = finder.call("drawButton")
		@m_bitmap1 = finder.call("m_bitmap1")
		if self.class.method_defined? "on_init"
			self.on_init()
		end
	end
end


