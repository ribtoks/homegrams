#pragma once
#include <vcclr.h>
#include <cctype>
#include <cstdlib>
#include <string>


namespace Source_mover {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::IO;
	using namespace System::Text;

	/// <summary>
	/// Summary for Form1
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class Form1 : public System::Windows::Forms::Form
	{
	public:
		Form1(void)
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
		~Form1()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::TextBox^  textBox1;
	protected: 
	private: System::Windows::Forms::TextBox^  textBox2;
	private: System::Windows::Forms::Button^  button1;
	private: System::Windows::Forms::Button^  button2;
	private: System::Windows::Forms::Button^  button3;
	private: System::Windows::Forms::Button^  button4;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::OpenFileDialog^  openFileDialog1;
	private: System::Windows::Forms::SaveFileDialog^  saveFileDialog1;
	private: System::Windows::Forms::Button^  button5;
	private: System::Windows::Forms::Button^  button6;
	private: System::Windows::Forms::OpenFileDialog^  openFileDialog2;
	private: System::Windows::Forms::SaveFileDialog^  saveFileDialog2;
	private: System::Windows::Forms::Button^  button7;
	private: System::Windows::Forms::Button^  button8;

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
			this->textBox2 = (gcnew System::Windows::Forms::TextBox());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->button2 = (gcnew System::Windows::Forms::Button());
			this->button3 = (gcnew System::Windows::Forms::Button());
			this->button4 = (gcnew System::Windows::Forms::Button());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->openFileDialog1 = (gcnew System::Windows::Forms::OpenFileDialog());
			this->saveFileDialog1 = (gcnew System::Windows::Forms::SaveFileDialog());
			this->button5 = (gcnew System::Windows::Forms::Button());
			this->button6 = (gcnew System::Windows::Forms::Button());
			this->openFileDialog2 = (gcnew System::Windows::Forms::OpenFileDialog());
			this->saveFileDialog2 = (gcnew System::Windows::Forms::SaveFileDialog());
			this->button7 = (gcnew System::Windows::Forms::Button());
			this->button8 = (gcnew System::Windows::Forms::Button());
			this->SuspendLayout();
			// 
			// textBox1
			// 
			this->textBox1->Location = System::Drawing::Point(12, 26);
			this->textBox1->Multiline = true;
			this->textBox1->Name = L"textBox1";
			this->textBox1->ScrollBars = System::Windows::Forms::ScrollBars::Vertical;
			this->textBox1->Size = System::Drawing::Size(226, 280);
			this->textBox1->TabIndex = 0;
			// 
			// textBox2
			// 
			this->textBox2->Location = System::Drawing::Point(254, 26);
			this->textBox2->Multiline = true;
			this->textBox2->Name = L"textBox2";
			this->textBox2->ScrollBars = System::Windows::Forms::ScrollBars::Vertical;
			this->textBox2->Size = System::Drawing::Size(233, 280);
			this->textBox2->TabIndex = 1;
			// 
			// button1
			// 
			this->button1->BackColor = System::Drawing::Color::LightGray;
			this->button1->Location = System::Drawing::Point(163, 312);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(75, 23);
			this->button1->TabIndex = 2;
			this->button1->Text = L"-------->";
			this->button1->UseVisualStyleBackColor = false;
			this->button1->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// button2
			// 
			this->button2->BackColor = System::Drawing::Color::LightGray;
			this->button2->Location = System::Drawing::Point(254, 312);
			this->button2->Name = L"button2";
			this->button2->Size = System::Drawing::Size(79, 23);
			this->button2->TabIndex = 3;
			this->button2->Text = L"<--------";
			this->button2->UseVisualStyleBackColor = false;
			this->button2->Click += gcnew System::EventHandler(this, &Form1::button2_Click);
			// 
			// button3
			// 
			this->button3->BackColor = System::Drawing::Color::LightGreen;
			this->button3->Location = System::Drawing::Point(12, 332);
			this->button3->Name = L"button3";
			this->button3->Size = System::Drawing::Size(51, 23);
			this->button3->TabIndex = 4;
			this->button3->Text = L"Load";
			this->button3->UseVisualStyleBackColor = false;
			this->button3->Click += gcnew System::EventHandler(this, &Form1::button3_Click);
			// 
			// button4
			// 
			this->button4->BackColor = System::Drawing::Color::LightGreen;
			this->button4->Location = System::Drawing::Point(442, 332);
			this->button4->Name = L"button4";
			this->button4->Size = System::Drawing::Size(45, 23);
			this->button4->TabIndex = 5;
			this->button4->Text = L"Load";
			this->button4->UseVisualStyleBackColor = false;
			this->button4->Click += gcnew System::EventHandler(this, &Form1::button4_Click);
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 8.25F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->label1->Location = System::Drawing::Point(80, 8);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(117, 13);
			this->label1->TabIndex = 6;
			this->label1->Text = L"TarasK\' commands:";
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 8.25F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->label2->Location = System::Drawing::Point(277, 8);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(118, 13);
			this->label2->TabIndex = 7;
			this->label2->Text = L"TaraSh\' commands:";
			// 
			// openFileDialog1
			// 
			this->openFileDialog1->FileName = L"*.txt";
			this->openFileDialog1->Filter = L"Text Files(*.txt) | *.txt";
			// 
			// saveFileDialog1
			// 
			this->saveFileDialog1->FileName = L"*.txt";
			this->saveFileDialog1->Filter = L"Text Files(*.txt) | *.txt";
			// 
			// button5
			// 
			this->button5->BackColor = System::Drawing::Color::DarkOliveGreen;
			this->button5->Location = System::Drawing::Point(69, 332);
			this->button5->Name = L"button5";
			this->button5->Size = System::Drawing::Size(41, 23);
			this->button5->TabIndex = 8;
			this->button5->Text = L"Save";
			this->button5->UseVisualStyleBackColor = false;
			this->button5->Click += gcnew System::EventHandler(this, &Form1::button5_Click);
			// 
			// button6
			// 
			this->button6->BackColor = System::Drawing::Color::DarkOliveGreen;
			this->button6->Location = System::Drawing::Point(392, 332);
			this->button6->Name = L"button6";
			this->button6->Size = System::Drawing::Size(44, 23);
			this->button6->TabIndex = 9;
			this->button6->Text = L"Save";
			this->button6->UseVisualStyleBackColor = false;
			this->button6->Click += gcnew System::EventHandler(this, &Form1::button6_Click);
			// 
			// openFileDialog2
			// 
			this->openFileDialog2->FileName = L"*.txt";
			this->openFileDialog2->Filter = L"Text Files(*.txt) | *.txt";
			// 
			// saveFileDialog2
			// 
			this->saveFileDialog2->FileName = L"*.txt";
			this->saveFileDialog2->Filter = L"Text Files(*.txt) | *.txt";
			// 
			// button7
			// 
			this->button7->Location = System::Drawing::Point(423, 1);
			this->button7->Name = L"button7";
			this->button7->Size = System::Drawing::Size(64, 21);
			this->button7->TabIndex = 10;
			this->button7->Text = L"Clear";
			this->button7->UseVisualStyleBackColor = true;
			this->button7->Click += gcnew System::EventHandler(this, &Form1::button7_Click);
			// 
			// button8
			// 
			this->button8->Location = System::Drawing::Point(13, 1);
			this->button8->Name = L"button8";
			this->button8->Size = System::Drawing::Size(61, 20);
			this->button8->TabIndex = 11;
			this->button8->Text = L"Clear";
			this->button8->UseVisualStyleBackColor = true;
			this->button8->Click += gcnew System::EventHandler(this, &Form1::button8_Click);
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::Color::DarkSeaGreen;
			this->ClientSize = System::Drawing::Size(499, 367);
			this->Controls->Add(this->button8);
			this->Controls->Add(this->button7);
			this->Controls->Add(this->button6);
			this->Controls->Add(this->button5);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->button4);
			this->Controls->Add(this->button3);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->button1);
			this->Controls->Add(this->textBox2);
			this->Controls->Add(this->textBox1);
			this->Name = L"Form1";
			this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
			this->Text = L"Source mover";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void button3_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->textBox1->Clear();

				 String ^tmp;
			 if(this->openFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK){
			 String ^path = this->openFileDialog1->FileName;
             char new_[100];
 		  //Translation to std::string::::::::::::::::::::::::::
			 cli::pin_ptr<const wchar_t> wch = PtrToStringChars(path);
				 size_t sz = wcslen(wch) + 1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 std::string s(new_);
				 int note = s.find_last_of('\\', s.length()-1);
				
				 if(note == std::string::npos)
				 {
					 tmp = File::ReadAllText(path);
					 this->textBox1->Clear();
					 this->textBox1->Text = tmp;
					 this->Text::set("Source mover :: " + path);
					 return;			 
				 }				 
				 std::string tr = "";
				 tr.insert(0, s, note + 1, s.length() - 1 - note);
				 String ^qw = gcnew String(tr.c_str());

			 tmp = File::ReadAllText(path);
			 this->textBox1->Clear();
			 this->textBox1->Text = tmp;
			 this->Text::set("Source mover :: " + qw);
			 }			 				
			 }
private: System::Void button4_Click(System::Object^  sender, System::EventArgs^  e) {

			 this->textBox2->Clear();
			 String ^tmp;
			 if(this->openFileDialog2->ShowDialog() == System::Windows::Forms::DialogResult::OK){
			 String ^path = this->openFileDialog2->FileName;
             char new_[100];
 		  //Translation to std::string::::::::::::::::::::::::::
			 cli::pin_ptr<const wchar_t> wch = PtrToStringChars(path);
			 size_t sz = wcslen(wch) + 1;
			 size_t y = 0;
			 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
			 std::string s(new_);
			 int note = s.find_last_of('\\', s.length() - 1);
				
			 if(note == std::string::npos)
			 {
				 tmp = File::ReadAllText(path);
				 this->textBox2->Clear();
				 this->textBox2->Text = tmp;
				 this->Text::set("Source mover :: " + path);
				 return;			 
			 }				 
				 std::string tr = "";
				 tr.insert(0, s, note + 1, s.length() - 1 - note);
				 String ^qw = gcnew String(tr.c_str());

			 tmp = File::ReadAllText(path);
			 this->textBox2->Clear();
			 this->textBox2->Text = tmp;
			 this->Text::set("Source mover :: " + qw);
			 }			 				

		 }
private: System::Void button5_Click(System::Object^  sender, System::EventArgs^  e) {
			 String ^File;
			 if(this->saveFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK){
			 File = this->saveFileDialog1->FileName;
			 File::Delete(File);
			 File::AppendAllText(File, this->textBox1->Text);
			 MessageBox::Show("File " + File + " was successfuly saved.", "Source mover", MessageBoxButtons::OK, MessageBoxIcon::Information);
			 }
		 }
private: System::Void button6_Click(System::Object^  sender, System::EventArgs^  e) 
		 {
			 String ^File;
			 if(this->saveFileDialog2->ShowDialog() == System::Windows::Forms::DialogResult::OK)
			 {
				File = this->saveFileDialog2->FileName;
				File::Delete(File);
				File::AppendAllText(File, this->textBox2->Text);
				MessageBox::Show("File " + File + " was successfuly saved.", "Source mover", MessageBoxButtons::OK, MessageBoxIcon::Information);
			 }
		 }
private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
			 this->textBox2->Clear();
			 int i = 0;
			 int q_curr,q_res;
	         char ch_curr, ch_res, dir;
			 String ^s1;
			 char new_[100];

			  while(i <= textBox1->Lines->Length - 1){
				 s1 = textBox1->Lines[i]->ToString();
				 
				 //Translation to std::string::::::::::::::::::::::::::
				 pin_ptr<const wchar_t> wch = PtrToStringChars(s1);
				 size_t sz = wcslen(wch) + 1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 std::string s(new_);
				  
				 String ^tmmp;
				 tmmp = this->textBox1->Text->ToString();
				 
				 const char *a = 0;
				 a = s.data();
				 int y3 = sscanf(a, "q%d %c->%c%cq%d", &q_curr, &ch_curr, &ch_res, &dir, &q_res);
				 if(y3 != 5)
				 {
					 int y = i+1;
					 MessageBox::Show("Wrong command in line " + y, "Error!", MessageBoxButtons::OK, MessageBoxIcon::Warning);
					 this->textBox1->Focus();
					 if(tmmp->Contains(s1))
					 {
						this->textBox1->Select(tmmp->IndexOf(s1), s1->Length::get());
					 }					 
				 }
				 if(ch_curr == '^')
						 ch_curr = '#';
					 if(ch_res == '^')
						 ch_res = '#';
					 if(dir == 'S')
						 dir = 'N';
				 std::string tm = "";
				 std::string tm1 = "";
				 std::string tm2 = "";
				 tm += ch_curr;
				 tm1 += ch_res;
				 tm2 += dir;
				 String ^newstr;

				 String ^ ch_c = gcnew String(tm.c_str());
				 String ^ ch_r = gcnew String(tm1.c_str());
					 
				// String ^q_c = gcnew String(q_curr);
				// String ^q_r = gcnew String(q_res);
				 String ^ d = gcnew String(tm2.c_str());

				 newstr = "q" + q_curr + " " + ch_c + " - q" + q_res + " " + d + " " + ch_r;
				 if(i < textBox1->Lines->Length - 1)
					 this->textBox2->AppendText(newstr + Environment::NewLine);					
					 else
						 this->textBox2->AppendText(newstr);					

					 i++;
			  }	 			 
		 }

private: System::Void button2_Click(System::Object^  sender, System::EventArgs^  e) {
             this->textBox1->Clear();
			 int i = 0;
			 int q_curr, q_res;
	         char ch_curr, ch_res, dir;
			 String ^s1;
			 char new_[100];

			  while(i <= textBox2->Lines->Length - 1){
				 s1 = textBox2->Lines[i]->ToString();
				 
				 //Translation to std::string::::::::::::::::::::::::::
				 pin_ptr<const wchar_t> wch = PtrToStringChars(s1);
				 size_t sz = wcslen(wch) + 1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 std::string s(new_);
				  
				 String ^tmmp;
				 tmmp = this->textBox2->Text->ToString();
				 
				 const char *a = 0;
				 a = s.data();
				 int y3 = sscanf(a, "q%d %c - q%d %c %c", &q_curr, &ch_curr, &q_res, &dir, &ch_res);
				 if(y3 != 5)
				 {
					 int y = i + 1;
					 MessageBox::Show("Wrong command in line " + y, "Error!", MessageBoxButtons::OK, MessageBoxIcon::Warning);
					 this->textBox2->Focus();
					 if(tmmp->Contains(s1))
					 {
						this->textBox2->Select(tmmp->IndexOf(s1), s1->Length::get());
					 }					 
				 }
				 if(ch_curr == '#')
						 ch_curr = '^';
				 if(ch_res == '#')
						 ch_res = '^';
					 if(dir == 'N')
						 dir = 'S';

				 std::string tm = "";
				 std::string tm1 = "";
				 std::string tm2 = "";
				 tm += ch_curr;
				 tm1 += ch_res;
				 tm2 += dir;
					 String ^newstr;

					 String ^ ch_c = gcnew String(tm.c_str());
					 String ^ ch_r = gcnew String(tm1.c_str());
					 
					// String ^q_c = gcnew String(q_curr);
					// String ^q_r = gcnew String(q_res);
					 String ^ d = gcnew String(tm2.c_str());

					 newstr = "q" + q_curr + " " + ch_c + "->" + ch_r + d + "q" + q_res;
					 if(i < textBox2->Lines->Length - 1)
						this->textBox1->AppendText(newstr + Environment::NewLine);					
						else
							 this->textBox1->AppendText(newstr);					
					 i++;
			  }	 
		 }

private: System::Void button8_Click(System::Object^  sender, System::EventArgs^  e) {
			 this->textBox1->Clear();
		 }
private: System::Void button7_Click(System::Object^  sender, System::EventArgs^  e) {
			 this->textBox2->Clear();
		 }
};
}

