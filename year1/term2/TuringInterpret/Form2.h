#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


namespace TuringInterpret {

	/// <summary>
	/// Summary for RunningLineCurrentposition
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class RunningLineCurrentposition : public System::Windows::Forms::Form
	{
	public:
		RunningLineCurrentposition(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~RunningLineCurrentposition()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::TextBox^  textBox1;
	private: System::Windows::Forms::MenuStrip^  menuStrip1;
	private: System::Windows::Forms::ToolStripMenuItem^  optionsToolStripMenuItem;
	private: System::Windows::Forms::ToolStripMenuItem^  stayToolStripMenuItem;
	private: System::Windows::Forms::ToolStripMenuItem^  dontStayOnTopToolStripMenuItem;

	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->menuStrip1 = (gcnew System::Windows::Forms::MenuStrip());
			this->optionsToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->stayToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->dontStayOnTopToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->menuStrip1->SuspendLayout();
			this->SuspendLayout();
			// 
			// textBox1
			// 
			this->textBox1->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->textBox1->Location = System::Drawing::Point(3, 33);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(539, 31);
			this->textBox1->TabIndex = 0;
			// 
			// menuStrip1
			// 
			this->menuStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(1) {this->optionsToolStripMenuItem});
			this->menuStrip1->Location = System::Drawing::Point(0, 0);
			this->menuStrip1->Name = L"menuStrip1";
			this->menuStrip1->Size = System::Drawing::Size(543, 24);
			this->menuStrip1->TabIndex = 1;
			this->menuStrip1->Text = L"menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this->optionsToolStripMenuItem->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(2) {this->stayToolStripMenuItem, 
				this->dontStayOnTopToolStripMenuItem});
			this->optionsToolStripMenuItem->Name = L"optionsToolStripMenuItem";
			this->optionsToolStripMenuItem->Size = System::Drawing::Size(56, 20);
			this->optionsToolStripMenuItem->Text = L"Options";
			// 
			// stayToolStripMenuItem
			// 
			this->stayToolStripMenuItem->Name = L"stayToolStripMenuItem";
			this->stayToolStripMenuItem->Size = System::Drawing::Size(168, 22);
			this->stayToolStripMenuItem->Text = L"Stay on top";
			this->stayToolStripMenuItem->Click += gcnew System::EventHandler(this, &RunningLineCurrentposition::stayToolStripMenuItem_Click);
			// 
			// dontStayOnTopToolStripMenuItem
			// 
			this->dontStayOnTopToolStripMenuItem->Name = L"dontStayOnTopToolStripMenuItem";
			this->dontStayOnTopToolStripMenuItem->Size = System::Drawing::Size(168, 22);
			this->dontStayOnTopToolStripMenuItem->Text = L"Don\'t stay on top";
			this->dontStayOnTopToolStripMenuItem->Click += gcnew System::EventHandler(this, &RunningLineCurrentposition::dontStayOnTopToolStripMenuItem_Click);
			// 
			// RunningLineCurrentposition
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(543, 76);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->menuStrip1);
			this->MainMenuStrip = this->menuStrip1;
			this->Name = L"RunningLineCurrentposition";
			this->Text = L"RunningLine(Currentposition)";
			this->menuStrip1->ResumeLayout(false);
			this->menuStrip1->PerformLayout();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void stayToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->TopMost=true;				 
			 }
	private: System::Void dontStayOnTopToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->TopMost=false;
			 }
};
}
