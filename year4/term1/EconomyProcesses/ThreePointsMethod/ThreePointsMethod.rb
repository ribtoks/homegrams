
# This class was automatically generated from XRC source. It is not
# recommended that this file is edited directly; instead, inherit from
# this class and extend its behaviour there.  
#
# Source file: ThreePointsMethod.xrc 
# Generated at: Thu Nov 25 14:39:24 +0200 2010

class MainThreePointsFrame < Wx::Frame
	
	attr_reader :m_statictext2, :xsampletextbox, :m_statictext3,
              :ysampletextbox, :submitsamplesbutton,
              :modelradiobutton, :m_statictext6, :alphalabel,
              :betalabel, :gammalabel, :m_statictext8,
              :correlationlabel
	
	def initialize(parent = nil)
		super()
		xml = Wx::XmlResource.get
		xml.flags = 2 # Wx::XRC_NO_SUBCLASSING
		xml.init_all_handlers
		xml.load("ThreePointsMethod.xrc")
		xml.load_frame_subclass(self, parent, "TheePointsFrame")

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
		
		@m_statictext2 = finder.call("m_staticText2")
		@xsampletextbox = finder.call("xSampleTextBox")
		@m_statictext3 = finder.call("m_staticText3")
		@ysampletextbox = finder.call("ySampleTextBox")
		@submitsamplesbutton = finder.call("submitSamplesButton")
		@modelradiobutton = finder.call("modelRadioButton")
		@m_statictext6 = finder.call("m_staticText6")
		@alphalabel = finder.call("alphaLabel")
		@betalabel = finder.call("betaLabel")
		@gammalabel = finder.call("gammaLabel")
		@m_statictext8 = finder.call("m_staticText8")
		@correlationlabel = finder.call("correlationLabel")
		if self.class.method_defined? "on_init"
			self.on_init()
		end
	end
end


