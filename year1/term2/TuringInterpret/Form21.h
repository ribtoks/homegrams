#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


namespace TuringInterpret {

	/// <summary>
	/// Summary for Form2
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Form2 : public System::Windows::Forms::Form
	{
	public:
		Form2(void)
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
		~Form2()
		{
			if (components)
			{
				delete components;
			}
		}
	public: System::Windows::Forms::MenuStrip^  menuStrip1;
	protected: 
	private: System::Windows::Forms::ToolStripMenuItem^  optionsToolStripMenuItem;
	public: System::Windows::Forms::ToolStripMenuItem^  stayOnTopToolStripMenuItem;
	public: System::Windows::Forms::ToolStripMenuItem^  dontStayOnTopToolStripMenuItem;
	public: System::Windows::Forms::TextBox^  textBox1;
	public: System::Windows::Forms::Button^  button1;

	public:
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
			this->menuStrip1 = (gcnew System::Windows::Forms::MenuStrip());
			this->optionsToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->stayOnTopToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->dontStayOnTopToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->menuStrip1->SuspendLayout();
			this->SuspendLayout();
			// 
			// menuStrip1
			// 
			this->menuStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(1) {this->optionsToolStripMenuItem});
			this->menuStrip1->Location = System::Drawing::Point(0, 0);
			this->menuStrip1->Name = L"menuStrip1";
			this->menuStrip1->Size = System::Drawing::Size(561, 24);
			this->menuStrip1->TabIndex = 0;
			this->menuStrip1->Text = L"menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this->optionsToolStripMenuItem->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(2) {this->stayOnTopToolStripMenuItem, 
				this->dontStayOnTopToolStripMenuItem});
			this->optionsToolStripMenuItem->Name = L"optionsToolStripMenuItem";
			this->optionsToolStripMenuItem->Size = System::Drawing::Size(56, 20);
			this->optionsToolStripMenuItem->Text = L"Options";
			// 
			// stayOnTopToolStripMenuItem
			// 
			this->stayOnTopToolStripMenuItem->Name = L"stayOnTopToolStripMenuItem";
			this->stayOnTopToolStripMenuItem->Size = System::Drawing::Size(168, 22);
			this->stayOnTopToolStripMenuItem->Text = L"Stay on top";
			this->stayOnTopToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form2::stayOnTopToolStripMenuItem_Click);
			// 
			// dontStayOnTopToolStripMenuItem
			// 
			this->dontStayOnTopToolStripMenuItem->Name = L"dontStayOnTopToolStripMenuItem";
			this->dontStayOnTopToolStripMenuItem->Size = System::Drawing::Size(168, 22);
			this->dontStayOnTopToolStripMenuItem->Text = L"Don\'t stay on top";
			this->dontStayOnTopToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form2::dontStayOnTopToolStripMenuItem_Click);
			// 
			// textBox1
			// 
			this->textBox1->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 15.75F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->textBox1->Location = System::Drawing::Point(12, 27);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(500, 31);
			this->textBox1->TabIndex = 1;
			this->textBox1->Enter += gcnew System::EventHandler(this, &Form2::textBox1_Enter);
			this->textBox1->KeyDown += gcnew System::Windows::Forms::KeyEventHandler(this, &Form2::textBox1_KeyDown);
			// 
			// button1
			// 
			this->button1->BackColor = System::Drawing::Color::Red;
			this->button1->Location = System::Drawing::Point(518, 24);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(36, 34);
			this->button1->TabIndex = 2;
			this->button1->Text = L"Go";
			this->button1->UseVisualStyleBackColor = false;
			this->button1->Click += gcnew System::EventHandler(this, &Form2::button1_Click);
			// 
			// Form2
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->AutoSize = true;
			this->AutoSizeMode = System::Windows::Forms::AutoSizeMode::GrowAndShrink;
			this->ClientSize = System::Drawing::Size(561, 69);
			this->Controls->Add(this->button1);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->menuStrip1);
			this->DoubleBuffered = true;
			this->MainMenuStrip = this->menuStrip1;
			this->MaximizeBox = false;
			this->MinimizeBox = false;
			this->Name = L"Form2";
			this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
			this->Text = L"Running Line (current position)";
			this->menuStrip1->ResumeLayout(false);
			this->menuStrip1->PerformLayout();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

  	public: System::Void stayOnTopToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->TopMost = true;

			 }
	public: System::Void dontStayOnTopToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->TopMost = false;
			 }
	public: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->button1->Enabled = false;
				 this->textBox1->Enabled = false;
				 this->TopMost = true;	

	 
			 }
private: System::Void textBox1_Enter(System::Object^  sender, System::EventArgs^  e) {
			 	 
			
		 }
private: System::Void textBox1_KeyDown(System::Object^  sender, System::Windows::Forms::KeyEventArgs^  e) {
			  if(e->KeyCode == Keys::Enter){
				 this->TopMost = true;
				 this->button1->Enabled = false;
				 this->textBox1->Enabled = false;
				 }

	 
		 }
};
}
