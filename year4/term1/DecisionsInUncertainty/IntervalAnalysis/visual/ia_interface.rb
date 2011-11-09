
# This class was automatically generated from XRC source. It is not
# recommended that this file is edited directly; instead, inherit from
# this class and extend its behaviour there.  
#
# Source file: ia_interface.xrc 
# Generated at: Fri Oct 29 00:30:03 +0300 2010

class IntervalAnalysisFrame < Wx::Frame
	
	attr_reader :m_statictext1, :m_statictext2, :leftboundtextbox,
              :m_statictext3, :rightboundtextbox, :epsilonlabel,
              :epsilontextbox, :drawgraphicbutton, :taskradiobutton,
              :runbutton, :resultstextbox
	
	def initialize(parent = nil)
		super()
		xml = Wx::XmlResource.get
		xml.flags = 2 # Wx::XRC_NO_SUBCLASSING
		xml.init_all_handlers
		xml.load(File.expand_path(File.dirname(__FILE__)) + '/../visual/ia_interface.xrc')
		xml.load_frame_subclass(self, parent, "IntervalsVisualFrame")

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
		
		@m_statictext1 = finder.call("m_staticText1")
		@m_statictext2 = finder.call("m_staticText2")
		@leftboundtextbox = finder.call("leftBoundTextBox")
		@m_statictext3 = finder.call("m_staticText3")
		@rightboundtextbox = finder.call("rightBoundTextBox")
		@epsilonlabel = finder.call("epsilonLabel")
		@epsilontextbox = finder.call("epsilonTextBox")
		@drawgraphicbutton = finder.call("drawGraphicButton")
		@taskradiobutton = finder.call("taskRadioButton")
		@runbutton = finder.call("runButton")
		@resultstextbox = finder.call("resultsTextBox")
		if self.class.method_defined? "on_init"
			self.on_init()
		end
	end
end


