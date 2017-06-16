
# This class was automatically generated from XRC source. It is not
# recommended that this file is edited directly; instead, inherit from
# this class and extend its behaviour there.  
#
# Source file: SamplesWork.xrc 
# Generated at: Wed Dec 15 22:25:57 +0200 2010

class SamplesWorkFrame < Wx::Frame
	
	attr_reader :m_notebook1, :m_panel1, :m_statictext2, :sample1textbox,
              :m_statictext3, :sample2textbox, :m_statictext15,
              :sample3textbox, :submitbutton, :m_panel2,
              :recalculatebutton, :additionalbutton,
              :drawgraphicsbutton, :samplesgrid, :m_panel4,
              :additonalgrid, :m_panel41, :checkphisherbutton,
              :phisherresult, :tablegrid
	
	def initialize(parent = nil)
		super()
		xml = Wx::XmlResource.get
		xml.flags = 2 # Wx::XRC_NO_SUBCLASSING
		xml.init_all_handlers
		xml.load("SamplesWork.xrc")
		xml.load_frame_subclass(self, parent, "MyFrame1")

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
		
		@m_notebook1 = finder.call("m_notebook1")
		@m_panel1 = finder.call("m_panel1")
		@m_statictext2 = finder.call("m_staticText2")
		@sample1textbox = finder.call("sample1TextBox")
		@m_statictext3 = finder.call("m_staticText3")
		@sample2textbox = finder.call("sample2TextBox")
		@m_statictext15 = finder.call("m_staticText15")
		@sample3textbox = finder.call("sample3TextBox")
		@submitbutton = finder.call("submitButton")
		@m_panel2 = finder.call("m_panel2")
		@recalculatebutton = finder.call("recalculateButton")
		@additionalbutton = finder.call("additionalButton")
		@drawgraphicsbutton = finder.call("drawGraphicsButton")
		@samplesgrid = finder.call("samplesGrid")
		@m_panel4 = finder.call("m_panel4")
		@additonalgrid = finder.call("additonalGrid")
		@m_panel41 = finder.call("m_panel41")
		@checkphisherbutton = finder.call("checkphisherButton")
		@phisherresult = finder.call("phisherresult")
		@tablegrid = finder.call("tableGrid")
		if self.class.method_defined? "on_init"
			self.on_init()
		end
	end
end


