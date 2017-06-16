
# This class was automatically generated from XRC source. It is not
# recommended that this file is edited directly; instead, inherit from
# this class and extend its behaviour there.  
#
# Source file: SamplesWork.xrc 
# Generated at: Tue Oct 26 23:51:09 +0300 2010

class SamplesWorkFrame < Wx::Frame
	
	attr_reader :m_notebook1, :m_panel1, :m_statictext2, :sample1textbox,
              :m_statictext3, :sample2textbox, :submitbutton,
              :m_panel2, :recalculatebutton, :additionalbutton,
              :drawgraphicsbutton, :samplesgrid, :m_panel4,
              :checkphisherbutton, :phisherresult, :additonalgrid,
              :m_panel41, :intervalscalculatebutton, :m_statictext8,
              :sigmae_label, :m_statictext10, :sigmab0_label,
              :m_statictext12, :sigmab1_label, :ci_b0_label,
              :ci_b1_label, :m_statictext16, :nextpointtextbox,
              :calculatenextpbutton, :nextvaluelabel, :intervallabel
	
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
		@submitbutton = finder.call("submitButton")
		@m_panel2 = finder.call("m_panel2")
		@recalculatebutton = finder.call("recalculateButton")
		@additionalbutton = finder.call("additionalButton")
		@drawgraphicsbutton = finder.call("drawGraphicsButton")
		@samplesgrid = finder.call("samplesGrid")
		@m_panel4 = finder.call("m_panel4")
		@checkphisherbutton = finder.call("checkPhisherButton")
		@phisherresult = finder.call("phisherResult")
		@additonalgrid = finder.call("additonalGrid")
		@m_panel41 = finder.call("m_panel41")
		@intervalscalculatebutton = finder.call("intervalsCalculateButton")
		@m_statictext8 = finder.call("m_staticText8")
		@sigmae_label = finder.call("sigmaE_Label")
		@m_statictext10 = finder.call("m_staticText10")
		@sigmab0_label = finder.call("sigmaB0_Label")
		@m_statictext12 = finder.call("m_staticText12")
		@sigmab1_label = finder.call("sigmaB1_Label")
		@ci_b0_label = finder.call("ci_b0_Label")
		@ci_b1_label = finder.call("ci_b1_Label")
		@m_statictext16 = finder.call("m_staticText16")
		@nextpointtextbox = finder.call("nextPointTextBox")
		@calculatenextpbutton = finder.call("calculateNextPButton")
		@nextvaluelabel = finder.call("nextValueLabel")
		@intervallabel = finder.call("intervalLabel")
		if self.class.method_defined? "on_init"
			self.on_init()
		end
	end
end


