#pragma once
#include "Variables.h"
#include < vcclr.h >
#include <cctype>
#include <vector>
#include <algorithm>
#include <list>
#include <map>
#include "Form21.h"
#include "About.h"

// Mine ------------------------Global Variables-------------------------------
//vector<int> vect;
//vector<int> v;
 list<int> lst;
 map<int,int> nums;
int lastx = 0,lasty = 0;

//vector<int> resv;
//vector<int> tr;
		
void Symbol(char ch) {
	int i;
	i = sups.find(ch);
	if(i == string::npos)
		sups += ch;
}


namespace TuringInterpret {

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
	private: System::Windows::Forms::MenuStrip^  menuStrip1;
	protected: 
	private: System::Windows::Forms::ToolStripMenuItem^  toolStripMenuItem1;
	private: System::Windows::Forms::ToolStripMenuItem^  newToolStripMenuItem;
	private: System::Windows::Forms::ToolStripContainer^  toolStripContainer1;
	private: System::Windows::Forms::TabControl^  tabControl1;
	private: System::Windows::Forms::TabPage^  tabPage1;
	private: System::Windows::Forms::TabPage^  tabPage2;
	private: System::Windows::Forms::TextBox^  TextBox1;



	private: System::Windows::Forms::Button^  button1;
	private: System::Windows::Forms::ToolStripMenuItem^  saveCommandsAsTextToolStripMenuItem;
	private: System::Windows::Forms::Button^  button4;
	private: System::Windows::Forms::Button^  button3;
	private: System::Windows::Forms::Button^  button2;
	private: System::Windows::Forms::DataGridView^  Table;
	private: System::Windows::Forms::Label^  label7;
	private: System::Windows::Forms::Label^  label8;
	private: System::Windows::Forms::Label^  label9;
	private: System::Windows::Forms::Label^  label10;
	private: System::Windows::Forms::Label^  label11;
	private: System::Windows::Forms::Label^  label12;
	private: System::Windows::Forms::Label^  label13;
	private: System::Windows::Forms::Label^  label14;
	private: System::Windows::Forms::Label^  label15;
	private: System::Windows::Forms::Label^  label16;
	private: System::Windows::Forms::Label^  label17;





	private: System::Windows::Forms::SaveFileDialog^  saveFileDialog1;
	private: System::Windows::Forms::ToolStripMenuItem^  toolStripMenuItem2;
	private: System::Windows::Forms::OpenFileDialog^  openFileDialog1;
	private: System::Windows::Forms::ToolStripMenuItem^  loadCommandListToolStripMenuItem;
	private: System::Windows::Forms::DataGridViewTextBoxColumn^  Column1;
	public: System::Windows::Forms::Button^  button5;

	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  label3;
	private: System::Windows::Forms::Label^  label4;
	private: System::Windows::Forms::Label^  label5;
	private: System::Windows::Forms::Label^  label6;
	private: System::Windows::Forms::Button^  button6;
	private:
		/// <summary>
		/// Required designer variable.
		About ^f;
		Form2 ^f2;		
		/// </summary>
		
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			System::ComponentModel::ComponentResourceManager^  resources = (gcnew System::ComponentModel::ComponentResourceManager(Form1::typeid));
			System::Windows::Forms::DataGridViewCellStyle^  dataGridViewCellStyle31 = (gcnew System::Windows::Forms::DataGridViewCellStyle());
			System::Windows::Forms::DataGridViewCellStyle^  dataGridViewCellStyle33 = (gcnew System::Windows::Forms::DataGridViewCellStyle());
			System::Windows::Forms::DataGridViewCellStyle^  dataGridViewCellStyle32 = (gcnew System::Windows::Forms::DataGridViewCellStyle());
			this->toolStripContainer1 = (gcnew System::Windows::Forms::ToolStripContainer());
			this->button6 = (gcnew System::Windows::Forms::Button());
			this->button5 = (gcnew System::Windows::Forms::Button());
			this->button4 = (gcnew System::Windows::Forms::Button());
			this->button3 = (gcnew System::Windows::Forms::Button());
			this->button2 = (gcnew System::Windows::Forms::Button());
			this->menuStrip1 = (gcnew System::Windows::Forms::MenuStrip());
			this->toolStripMenuItem1 = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->newToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->saveCommandsAsTextToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->loadCommandListToolStripMenuItem = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->toolStripMenuItem2 = (gcnew System::Windows::Forms::ToolStripMenuItem());
			this->tabControl1 = (gcnew System::Windows::Forms::TabControl());
			this->tabPage1 = (gcnew System::Windows::Forms::TabPage());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->TextBox1 = (gcnew System::Windows::Forms::TextBox());
			this->tabPage2 = (gcnew System::Windows::Forms::TabPage());
			this->Table = (gcnew System::Windows::Forms::DataGridView());
			this->Column1 = (gcnew System::Windows::Forms::DataGridViewTextBoxColumn());
			this->saveFileDialog1 = (gcnew System::Windows::Forms::SaveFileDialog());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label3 = (gcnew System::Windows::Forms::Label());
			this->label4 = (gcnew System::Windows::Forms::Label());
			this->label5 = (gcnew System::Windows::Forms::Label());
			this->label6 = (gcnew System::Windows::Forms::Label());
			this->openFileDialog1 = (gcnew System::Windows::Forms::OpenFileDialog());
			this->label7 = (gcnew System::Windows::Forms::Label());
			this->label8 = (gcnew System::Windows::Forms::Label());
			this->label9 = (gcnew System::Windows::Forms::Label());
			this->label10 = (gcnew System::Windows::Forms::Label());
			this->label11 = (gcnew System::Windows::Forms::Label());
			this->label12 = (gcnew System::Windows::Forms::Label());
			this->label13 = (gcnew System::Windows::Forms::Label());
			this->label14 = (gcnew System::Windows::Forms::Label());
			this->label15 = (gcnew System::Windows::Forms::Label());
			this->label16 = (gcnew System::Windows::Forms::Label());
			this->label17 = (gcnew System::Windows::Forms::Label());
			this->toolStripContainer1->ContentPanel->SuspendLayout();
			this->toolStripContainer1->TopToolStripPanel->SuspendLayout();
			this->toolStripContainer1->SuspendLayout();
			this->menuStrip1->SuspendLayout();
			this->tabControl1->SuspendLayout();
			this->tabPage1->SuspendLayout();
			this->tabPage2->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->Table))->BeginInit();
			this->SuspendLayout();
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this->toolStripContainer1->ContentPanel->BackColor = System::Drawing::Color::LightGoldenrodYellow;
			this->toolStripContainer1->ContentPanel->Controls->Add(this->button6);
			this->toolStripContainer1->ContentPanel->Controls->Add(this->button5);
			this->toolStripContainer1->ContentPanel->Controls->Add(this->button4);
			this->toolStripContainer1->ContentPanel->Controls->Add(this->button3);
			this->toolStripContainer1->ContentPanel->Controls->Add(this->button2);
			resources->ApplyResources(this->toolStripContainer1->ContentPanel, L"toolStripContainer1.ContentPanel");
			resources->ApplyResources(this->toolStripContainer1, L"toolStripContainer1");
			this->toolStripContainer1->Name = L"toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this->toolStripContainer1->TopToolStripPanel->Controls->Add(this->menuStrip1);
			// 
			// button6
			// 
			this->button6->BackColor = System::Drawing::Color::LightSteelBlue;
			resources->ApplyResources(this->button6, L"button6");
			this->button6->Name = L"button6";
			this->button6->UseVisualStyleBackColor = false;
			this->button6->Click += gcnew System::EventHandler(this, &Form1::button6_Click);
			// 
			// button5
			// 
			this->button5->BackColor = System::Drawing::Color::SlateGray;
			this->button5->DialogResult = System::Windows::Forms::DialogResult::Cancel;
			resources->ApplyResources(this->button5, L"button5");
			this->button5->Name = L"button5";
			this->button5->UseCompatibleTextRendering = true;
			this->button5->UseVisualStyleBackColor = false;
			this->button5->Click += gcnew System::EventHandler(this, &Form1::button5_Click);
			// 
			// button4
			// 
			this->button4->BackColor = System::Drawing::Color::LightSteelBlue;
			resources->ApplyResources(this->button4, L"button4");
			this->button4->Name = L"button4";
			this->button4->UseVisualStyleBackColor = false;
			this->button4->Click += gcnew System::EventHandler(this, &Form1::button4_Click);
			// 
			// button3
			// 
			this->button3->BackColor = System::Drawing::Color::LightSteelBlue;
			resources->ApplyResources(this->button3, L"button3");
			this->button3->Name = L"button3";
			this->button3->UseVisualStyleBackColor = false;
			this->button3->Click += gcnew System::EventHandler(this, &Form1::button3_Click);
			// 
			// button2
			// 
			this->button2->BackColor = System::Drawing::Color::LightSteelBlue;
			resources->ApplyResources(this->button2, L"button2");
			this->button2->Name = L"button2";
			this->button2->UseVisualStyleBackColor = false;
			this->button2->Click += gcnew System::EventHandler(this, &Form1::button2_Click);
			// 
			// menuStrip1
			// 
			this->menuStrip1->BackColor = System::Drawing::Color::Gainsboro;
			resources->ApplyResources(this->menuStrip1, L"menuStrip1");
			this->menuStrip1->Items->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(2) {this->toolStripMenuItem1, 
				this->toolStripMenuItem2});
			this->menuStrip1->Name = L"menuStrip1";
			// 
			// toolStripMenuItem1
			// 
			this->toolStripMenuItem1->DropDownItems->AddRange(gcnew cli::array< System::Windows::Forms::ToolStripItem^  >(3) {this->newToolStripMenuItem, 
				this->saveCommandsAsTextToolStripMenuItem, this->loadCommandListToolStripMenuItem});
			this->toolStripMenuItem1->Name = L"toolStripMenuItem1";
			resources->ApplyResources(this->toolStripMenuItem1, L"toolStripMenuItem1");
			// 
			// newToolStripMenuItem
			// 
			this->newToolStripMenuItem->Name = L"newToolStripMenuItem";
			resources->ApplyResources(this->newToolStripMenuItem, L"newToolStripMenuItem");
			this->newToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form1::newToolStripMenuItem_Click);
			// 
			// saveCommandsAsTextToolStripMenuItem
			// 
			this->saveCommandsAsTextToolStripMenuItem->Name = L"saveCommandsAsTextToolStripMenuItem";
			resources->ApplyResources(this->saveCommandsAsTextToolStripMenuItem, L"saveCommandsAsTextToolStripMenuItem");
			this->saveCommandsAsTextToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form1::saveCommandsAsTextToolStripMenuItem_Click);
			// 
			// loadCommandListToolStripMenuItem
			// 
			this->loadCommandListToolStripMenuItem->Name = L"loadCommandListToolStripMenuItem";
			resources->ApplyResources(this->loadCommandListToolStripMenuItem, L"loadCommandListToolStripMenuItem");
			this->loadCommandListToolStripMenuItem->Click += gcnew System::EventHandler(this, &Form1::loadCommandListToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this->toolStripMenuItem2->Name = L"toolStripMenuItem2";
			resources->ApplyResources(this->toolStripMenuItem2, L"toolStripMenuItem2");
			this->toolStripMenuItem2->Click += gcnew System::EventHandler(this, &Form1::toolStripMenuItem2_Click);
			// 
			// tabControl1
			// 
			this->tabControl1->Controls->Add(this->tabPage1);
			this->tabControl1->Controls->Add(this->tabPage2);
			resources->ApplyResources(this->tabControl1, L"tabControl1");
			this->tabControl1->Name = L"tabControl1";
			this->tabControl1->SelectedIndex = 0;
			// 
			// tabPage1
			// 
			this->tabPage1->Controls->Add(this->button1);
			this->tabPage1->Controls->Add(this->TextBox1);
			resources->ApplyResources(this->tabPage1, L"tabPage1");
			this->tabPage1->Name = L"tabPage1";
			this->tabPage1->UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			resources->ApplyResources(this->button1, L"button1");
			this->button1->Name = L"button1";
			this->button1->UseVisualStyleBackColor = true;
			this->button1->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// TextBox1
			// 
			this->TextBox1->BackColor = System::Drawing::Color::Khaki;
			resources->ApplyResources(this->TextBox1, L"TextBox1");
			this->TextBox1->Name = L"TextBox1";
			// 
			// tabPage2
			// 
			this->tabPage2->Controls->Add(this->Table);
			resources->ApplyResources(this->tabPage2, L"tabPage2");
			this->tabPage2->Name = L"tabPage2";
			this->tabPage2->UseVisualStyleBackColor = true;
			// 
			// Table
			// 
			dataGridViewCellStyle31->Alignment = System::Windows::Forms::DataGridViewContentAlignment::TopLeft;
			this->Table->AlternatingRowsDefaultCellStyle = dataGridViewCellStyle31;
			resources->ApplyResources(this->Table, L"Table");
			this->Table->AutoSizeRowsMode = System::Windows::Forms::DataGridViewAutoSizeRowsMode::AllCells;
			this->Table->ColumnHeadersHeightSizeMode = System::Windows::Forms::DataGridViewColumnHeadersHeightSizeMode::AutoSize;
			this->Table->Columns->AddRange(gcnew cli::array< System::Windows::Forms::DataGridViewColumn^  >(1) {this->Column1});
			this->Table->Name = L"Table";
			this->Table->ReadOnly = true;
			dataGridViewCellStyle33->Alignment = System::Windows::Forms::DataGridViewContentAlignment::MiddleLeft;
			dataGridViewCellStyle33->BackColor = System::Drawing::SystemColors::Control;
			dataGridViewCellStyle33->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 8.25F, System::Drawing::FontStyle::Regular, 
				System::Drawing::GraphicsUnit::Point, static_cast<System::Byte>(204)));
			dataGridViewCellStyle33->ForeColor = System::Drawing::SystemColors::WindowText;
			dataGridViewCellStyle33->SelectionBackColor = System::Drawing::SystemColors::HotTrack;
			dataGridViewCellStyle33->SelectionForeColor = System::Drawing::SystemColors::HighlightText;
			dataGridViewCellStyle33->WrapMode = System::Windows::Forms::DataGridViewTriState::True;
			this->Table->RowHeadersDefaultCellStyle = dataGridViewCellStyle33;
			this->Table->TabStop = false;
			// 
			// Column1
			// 
			dataGridViewCellStyle32->BackColor = System::Drawing::Color::Beige;
			dataGridViewCellStyle32->Font = (gcnew System::Drawing::Font(L"Microsoft Sans Serif", 8.25F, System::Drawing::FontStyle::Bold, System::Drawing::GraphicsUnit::Point, 
				static_cast<System::Byte>(204)));
			this->Column1->DefaultCellStyle = dataGridViewCellStyle32;
			resources->ApplyResources(this->Column1, L"Column1");
			this->Column1->Name = L"Column1";
			this->Column1->ReadOnly = true;
			this->Column1->Resizable = System::Windows::Forms::DataGridViewTriState::True;
			// 
			// saveFileDialog1
			// 
			this->saveFileDialog1->FileName = L"*.txt";
			resources->ApplyResources(this->saveFileDialog1, L"saveFileDialog1");
			// 
			// label1
			// 
			resources->ApplyResources(this->label1, L"label1");
			this->label1->Name = L"label1";
			// 
			// label2
			// 
			resources->ApplyResources(this->label2, L"label2");
			this->label2->Name = L"label2";
			// 
			// label3
			// 
			resources->ApplyResources(this->label3, L"label3");
			this->label3->Name = L"label3";
			// 
			// label4
			// 
			resources->ApplyResources(this->label4, L"label4");
			this->label4->Name = L"label4";
			// 
			// label5
			// 
			resources->ApplyResources(this->label5, L"label5");
			this->label5->Name = L"label5";
			// 
			// label6
			// 
			resources->ApplyResources(this->label6, L"label6");
			this->label6->Name = L"label6";
			// 
			// openFileDialog1
			// 
			resources->ApplyResources(this->openFileDialog1, L"openFileDialog1");
			// 
			// label7
			// 
			resources->ApplyResources(this->label7, L"label7");
			this->label7->Name = L"label7";
			// 
			// label8
			// 
			resources->ApplyResources(this->label8, L"label8");
			this->label8->Name = L"label8";
			// 
			// label9
			// 
			resources->ApplyResources(this->label9, L"label9");
			this->label9->Name = L"label9";
			// 
			// label10
			// 
			resources->ApplyResources(this->label10, L"label10");
			this->label10->Name = L"label10";
			// 
			// label11
			// 
			resources->ApplyResources(this->label11, L"label11");
			this->label11->Name = L"label11";
			// 
			// label12
			// 
			resources->ApplyResources(this->label12, L"label12");
			this->label12->Name = L"label12";
			// 
			// label13
			// 
			resources->ApplyResources(this->label13, L"label13");
			this->label13->Name = L"label13";
			// 
			// label14
			// 
			resources->ApplyResources(this->label14, L"label14");
			this->label14->Name = L"label14";
			// 
			// label15
			// 
			resources->ApplyResources(this->label15, L"label15");
			this->label15->Name = L"label15";
			// 
			// label16
			// 
			resources->ApplyResources(this->label16, L"label16");
			this->label16->Name = L"label16";
			// 
			// label17
			// 
			resources->ApplyResources(this->label17, L"label17");
			this->label17->Name = L"label17";
			// 
			// Form1
			// 
			resources->ApplyResources(this, L"$this");
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::Color::LightGray;
			this->Controls->Add(this->label17);
			this->Controls->Add(this->label16);
			this->Controls->Add(this->label15);
			this->Controls->Add(this->label14);
			this->Controls->Add(this->label13);
			this->Controls->Add(this->label12);
			this->Controls->Add(this->label11);
			this->Controls->Add(this->label10);
			this->Controls->Add(this->label9);
			this->Controls->Add(this->label8);
			this->Controls->Add(this->label7);
			this->Controls->Add(this->label6);
			this->Controls->Add(this->label5);
			this->Controls->Add(this->label4);
			this->Controls->Add(this->label3);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->tabControl1);
			this->Controls->Add(this->toolStripContainer1);
			this->DoubleBuffered = true;
			this->FormBorderStyle = System::Windows::Forms::FormBorderStyle::Fixed3D;
			this->MainMenuStrip = this->menuStrip1;
			this->MaximizeBox = false;
			this->Name = L"Form1";
			this->FormClosing += gcnew System::Windows::Forms::FormClosingEventHandler(this, &Form1::Form1_FormClosing);
			this->toolStripContainer1->ContentPanel->ResumeLayout(false);
			this->toolStripContainer1->TopToolStripPanel->ResumeLayout(false);
			this->toolStripContainer1->TopToolStripPanel->PerformLayout();
			this->toolStripContainer1->ResumeLayout(false);
			this->toolStripContainer1->PerformLayout();
			this->menuStrip1->ResumeLayout(false);
			this->menuStrip1->PerformLayout();
			this->tabControl1->ResumeLayout(false);
			this->tabPage1->ResumeLayout(false);
			this->tabPage1->PerformLayout();
			this->tabPage2->ResumeLayout(false);
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->Table))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion


	

		
	private: System::Void newToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->tabControl1->Enabled = true;
         		 this->tabControl1->SelectTab(0);
				 this->TextBox1->Clear();
				 if(this->Table->Columns->Count::get() > 0)
					 this->Table->Columns->Remove(this->Table->Columns[ this->Table->Columns->Count::get() - 1 ]);
				 this->Table->Columns->Clear();
				 this->Table->Rows->Clear();
				 this->TextBox1->Focus();
				 this->Text::set("Turing Interpreter");
			 }

private: System::Void saveCommandsAsTextToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
			 String ^File;
			 if(this->saveFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK)
			 {
				File = this->saveFileDialog1->FileName;
				File::Delete(File);
				File::AppendAllText(File,this->TextBox1->Text);
				MessageBox::Show("File " + File + " was successfuly saved.", "Turing Interpreter", MessageBoxButtons::OK, MessageBoxIcon::Information);
			 }
		 }

private: System::Void button4_Click(System::Object^  sender, System::EventArgs^  e) {
			  if(this->Table->Columns->Count::get() > 0)
					 this->Table->Columns->Remove(this->Table->Columns[ this->Table->Columns->Count::get() - 1 ]);
				 this->Table->Columns->Clear();
				 this->Table->Rows->Clear();
				 this->tabControl1->Enabled = true;
         		 this->tabControl1->SelectTab(0);
				 this->TextBox1->Focus();
				 l.line.clear();
				 sups = "";
								 //delete [] q;		
				 f2->Close();
				 this->button2->Enabled = false;
				 this->button3->Enabled = false;
				 this->button4->Enabled = false;
				 this->button5->Enabled = false;
				 MessageBox::Show("Run was aborted by user.", "Turing Interpreter", MessageBoxButtons::OK, MessageBoxIcon::Information);
				 
		 }
private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
			 this->button5->Enabled = true;
          if(this->Table->Columns->Count::get() > 0)
					 this->Table->Columns->Remove(this->Table->Columns[ this->Table->Columns->Count::get() - 1 ]);
				 this->Table->Columns->Clear();
				 this->Table->Rows->Clear();			 
			 bool is_zero = false;
			 int temp = 1;
			 int res = 1;
			 int i = 0;
			 int num = 0;
			 String ^s1;
			 char new_[100];
			 			 
			 Command com;
			 String ^tmmp;
			 tmmp = this->TextBox1->Text->ToString();
			


			 			
			 while(i <= TextBox1->Lines->Length - 1){
				 s1 = TextBox1->Lines[i]->ToString();
				 
				 //Translation to std::string::::::::::::::::::::::::::
				 pin_ptr<const wchar_t> wch = PtrToStringChars(s1);
				 size_t sz = wcslen(wch) + 1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 string s(new_);
				 
				 
				 
				 
				  
				 com.Parse(s);
				 
				 if(com.error == true){
					 int y = i + 1;
					 MessageBox::Show("Wrong command in line " + y, "Error!", MessageBoxButtons::OK, MessageBoxIcon::Warning);
					 this->TextBox1->Focus();
					 if(tmmp->Contains(s1)){
					 this->TextBox1->Select(tmmp->IndexOf(s1), s1->Length::get());
					 }
					 
				  return;
				 }
				 
				//vet.push_back(com.q_curr);
				//vet.push_back(com.q_res);
                 
				 Symbol(com.ch_curr);
				 Symbol(com.ch_res);

				 lst.push_back(com.q_curr);
				/* if(com.q_res>temp){
					 temp = com.q_res;
					 num++;
				 }
				 */

				 if(com.q_res == 0)
					 is_zero = true;
				 i++;
			 }
			 if(is_zero ==false)
			 {
				MessageBox::Show("There're no result state" ,"Error!", MessageBoxButtons::OK, MessageBoxIcon::Warning);
				return;
			 }
             lst.sort();
			 lst.unique();
			 num = lst.size();
			 num++;
			 
			 list<int>::iterator p = lst.end();
			 p--;
			 temp = *p;

			 temp++;
			 
			 
			 int post = 0;		 
			 list<int>::iterator tt = lst.begin();
			 while(tt != lst.end()){
				 nums.insert(pair<int, int>(*tt, post));
				 post++;
				 tt++;
			 }

			 for(i = 0; i < sups.length(); i++){
				 this->Table->Columns->Add("d", "s");				 
			 }
			 
			 for(i = 0; i <= num; i++){
				 this->Table->Rows->Add("  ");				 
			 }

			 for(int i = 0; i < sups.length(); i++){
				 const char *a = 0;
				 string ts = "";
				 ts += sups[i];
				 a = ts.c_str();
				 String ^stw = gcnew String(a);
				 this->Table->Columns[i]->HeaderText = stw;
			 }

			 /*for(int i=0;i<=temp;i++) {				 
				 this->Table->Rows[i]->HeaderCell->Value = ("q"+resv[i]);
				 q1 1->^Rq1
q1 ^->RRq0

q1 0->0Rq2
q2 0->1Rq1
q2 1->1Rq1
q1 1->0Rq2
q2 ^->^Sq0
q1 ^->^Sq0
				 */
			 
			 int pos = 0;
			 delete [] q;
			 q = new Stemp [temp + 1];
			 int k = 0, x = 0;		
			 string tmp = "";
			 int r = 0,index = 1;
			 int rows = 0;
			// int kp = 0;

			// p = lst.begin();
			 
		 while(r <= TextBox1->Lines->Length - 1)
		 {

				 s1 = TextBox1->Lines[r]->ToString();
				 //Translation to std::string::::::::::::::::::::::::::
				 pin_ptr<const wchar_t> wch = PtrToStringChars(s1);
				 size_t sz = wcslen(wch)+1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 string s(new_);
				 				 
				 com.Parse(s);
				 tmp = s;
				 int ew = tmp.find("->");
				 tmp.erase(0, ew);

				/* if(vect[com.q_res] ==-1){
				 vect[com.q_res] = k;
				 k++;
				 }
				 if(v[com.q_curr] ==-1){
				 v[com.q_curr] = x;
				 x++;
				 }
				 */


				 try{

				if(isgraph(q[com.q_curr].NextLetter(com.ch_curr))){
					 int u = r+1;
					 MessageBox::Show("This letter was used before!", "Error in line " + u, MessageBoxButtons::OK, MessageBoxIcon::Warning);
					  this->TextBox1->Focus();
					  if(tmmp->Contains(s1))
						  this->TextBox1->Select(tmmp->IndexOf(s1), s1->Length::get());					 
					 return;
				 }
				 }
				 catch(...)
				 {
				 }
				 
				// nums.insert(pair<int,int>(com.q_curr,kp));
				// kp++;

				 q[com.q_curr].SetNextState(com.ch_curr, com.q_res);
				 q[com.q_curr].SetNextLetter(com.ch_curr, com.ch_res);
				 q[com.q_curr].SetNextDir(com.ch_curr, com.d);
								
				/* p = lst.begin();
				 int dec = 0;
				 while(*p != com.q_curr){
					 p++;
					 dec++;
				 }
				 */
				 
				 map<int,int>::iterator it;
				 it = nums.find(com.q_curr);
				 
				 pos = sups.find(com.ch_curr);
				 try {
					 this->Table->Rows[it->second]->HeaderCell->Value = "q" + com.q_curr;
				 this->Table->Rows[it->second]->Cells[pos]->Value = gcnew String(tmp.c_str());
				 }
				 catch(...)
				 {
				 }				 
				 r++;
			 }	 	
			 this->tabControl1->SelectTab(1);
			 
			 MessageBox::Show("Now enter input memory line!", "Turing interpretation", MessageBoxButtons::OK, MessageBoxIcon::Information);
		     button2->Enabled = false;
			 button3->Enabled = false;
			 button4->Enabled = false;
			 f2 = gcnew Form2();
			 f2->Show();
			 f2->button1->Enabled = true;			 
		}


	private: System::Void button2_Click(System::Object^  sender, System::EventArgs^  e) {
				map<int,int>::iterator p;
				 if(lastx < 0 || !(lastx>=this->Table->Rows->Count - 1)  || lasty == std::string::npos)
					 try{			
						 this->Table->Rows[lastx]->Cells[lasty]->Selected = false;
				 }
				 catch(...)
				 {
				 }
					 //if(l.cstate==1){
					//	 this->Table->Rows[0]->Cells[sups.find(l.line[l.curr_num])]->Selected=true;
					 //}
                 int ppos = 0;

				 try {	  
				 Stemp *ar;
					  ar = &q[l.cstate];
					  l.MakeCommand(ar);
					  string h = "";
					  string tmp_l = l.line;
					  tmp_l.insert(l.curr_num + 1,"]");
					  tmp_l.insert(l.curr_num, "[");
					  int not1 = tmp_l.find_first_not_of('^', 0);
					  int note = tmp_l.find_last_not_of('^', tmp_l.length() - 1);
					  h.insert(0, tmp_l, not1, note - not1 + 1);
					  f2->textBox1->Clear();
					  f2->textBox1->Text = gcnew String(h.c_str());

					  if(l.cstate == 0)
						  goto ef;
					
						 
					 p = nums.find(l.cstate);
					 if(p == nums.end())
						 throw 1;
					
					  				 
					 ppos = sups.find(l.line[l.curr_num]);
				 }
				 catch(int f)
				 {
					 switch(f)
					 {
						 case 1: MessageBox::Show("State is not defined!", "State error!", MessageBoxButtons::OK, MessageBoxIcon::Error);
							 return;
							 break;
						 case 2: MessageBox::Show("Letter is not defined!", "Letter error!", MessageBoxButtons::OK, MessageBoxIcon::Error);
							 return;
							 break;
						 case 3: MessageBox::Show("Direction is not defined!", "Direction error!", MessageBoxButtons::OK, MessageBoxIcon::Error);
							 return;
							 break;
					 }
				 }
				  try 
				  {
					  this->Table->Rows[p->second]->Cells[ppos]->Selected = true;
				  }
					  catch(...){
					  }

					  lastx = p->second;
					  lasty = ppos;

ef:				  if(l.cstate == 0) 
				  {
					  MessageBox::Show(f2->textBox1->Text->ToString(), "Result", MessageBoxButtons::OK, MessageBoxIcon::Asterisk);
					  f2->Close();					
					  l.cstate = 1;
					  this->button2->Enabled = false;
					  this->button3->Enabled = false;
					  this->button4->Enabled = false;
					  l.line = "";
				  }
						  
	  
				  }  			
private: System::Void button3_Click(System::Object^  sender, System::EventArgs^  e) {	
			  map<int,int>::iterator p1;
			 
			 while(l.cstate != 0){
				  if(l.line.length() > 800) {
					 MessageBox::Show("You probably have infinite loop in your commands.", "Error!!!", MessageBoxButtons::OK, MessageBoxIcon::Error);
					 return;
				 }
			
				  try
				  {
					  this->Table->Rows[lastx]->Cells[lasty]->Selected = false;
				  }
				  catch(...)
				  {
				  }
					 //if(l.cstate==1){
					//	 this->Table->Rows[0]->Cells[sups.find(l.line[l.curr_num])]->Selected=true;
					 //}
                    int ppos;
				  try {
					  Stemp *ar;
					  ar = &q[l.cstate];
					  l.MakeCommand(ar);
					  string h = "";
					   string tmp_l = l.line;
					  tmp_l.insert(l.curr_num + 1, "]");
					  tmp_l.insert(l.curr_num, "[");
					  int not1 = tmp_l.find_first_not_of('^', 0);
					  int note = tmp_l.find_last_not_of('^', tmp_l.length() - 1);
					  h.insert(0, tmp_l, not1, note - not1 + 1);
					  f2->textBox1->Clear();
					  f2->textBox1->Text = gcnew String(h.c_str());					  
					  
					  if(l.cstate == 0)
						  goto ef1;
					
					 
						  p1 = nums.find(l.cstate);
						  if(p1 == nums.end())
							  throw 1;
					  
					  ppos=sups.find(l.line[l.curr_num]);
				  }
				  catch(int f){
					  switch(f){
						 case 1: MessageBox::Show("State is not defined!", "State error!", MessageBoxButtons::OK, MessageBoxIcon::Error);
							 return;
							 break;
						 case 2: MessageBox::Show("Letter is not defined!", "Letter error!", MessageBoxButtons::OK, MessageBoxIcon::Error);
							 return;
							 break;
						 case 3: MessageBox::Show("Direction is not defined!", "Direction error!", MessageBoxButtons::OK,MessageBoxIcon::Error);
							 return;
							 break;
					 }
				  }
					   
					 try {
						 this->Table->Rows[p1->second]->Cells[ppos]->Selected = true;
				  }
					  catch(...){
					  }

					  lastx = p1->second;
					  lasty = ppos;

			  }
ef1:	 MessageBox::Show(f2->textBox1->Text->ToString(), "Result",MessageBoxButtons::OK, MessageBoxIcon::Asterisk);
			 f2->Close();
			 l.cstate = 1;
			 this->button2->Enabled = false;
			 this->button3->Enabled = false;
			 this->button4->Enabled = false;
			 //l.line="";
		 }

private: System::Void button5_Click(System::Object^  sender, System::EventArgs^  e) {
			if(this->TextBox1->Text == "") {
				 MessageBox::Show("Please, enter some commands.", "Command list is empty", MessageBoxButtons::OK, MessageBoxIcon::Warning);
				 return;
			 }
			 if(f2->textBox1->Text == "") {
				 f2->Close();
				 MessageBox::Show("Please, enter input memory. (''^'' for example)", "Input memory line is empty", MessageBoxButtons::OK, MessageBoxIcon::Warning);
				 f2 = gcnew Form2();
				 f2->Show();
			 button2->Enabled = false;
			 button3->Enabled = false;
			 button4->Enabled = false;
			  return;
			 }
			
		     button2->Enabled = true;
			 button3->Enabled = true;
			 button4->Enabled = true;
			 Table->Rows[0]->Cells[0]->Selected = false;

			 String ^ss = f2->textBox1->Text->ToString();
             char new_[100];
			 pin_ptr<const wchar_t> wch = PtrToStringChars(ss);
			 size_t sz = wcslen(wch) + 1;
			 size_t y = 0;
			 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
			 string s12(new_);
			 l.MakeLine(s12);
		 }

private: System::Void button6_Click(System::Object^  sender, System::EventArgs^  e) {
			 TextBox1->Clear();
			 TextBox1->Focus();
		 }
private: System::Void Form1_FormClosing(System::Object^  sender, System::Windows::Forms::FormClosingEventArgs^  e) {
			 System::Windows::Forms::DialogResult result;
			 result = MessageBox::Show("Save current command list?", "Turing Interpreter", MessageBoxButtons::YesNo, MessageBoxIcon::Question);
			 if(result == ::DialogResult::Yes)
			 {
				 String ^File = gcnew String("");
				//ult =  this->saveFileDialog1->ShowDialog();	
				if(this->saveFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK){
				File = this->saveFileDialog1->FileName;		
				File::Delete(File);
				File::AppendAllText(File,this->TextBox1->Text);
				MessageBox::Show("File " + File + " was successfuly saved.", "Turing Interpreter", MessageBoxButtons::OK, MessageBoxIcon::Information);				
			 }		
			 }
		 }
private: System::Void loadCommandListToolStripMenuItem_Click(System::Object^  sender, System::EventArgs^  e) {
			 this->tabControl1->Enabled = true;
         	 this->tabControl1->SelectTab(0);
			 this->TextBox1->Clear();
			 if(this->Table->Columns->Count::get() > 0)
				 this->Table->Columns->Remove(this->Table->Columns[this->Table->Columns->Count::get() - 1]);
			 this->Table->Columns->Clear();
			 this->Table->Rows->Clear();
			 this->TextBox1->Focus();
			 String ^tmp;
			 if(this->openFileDialog1->ShowDialog() == System::Windows::Forms::DialogResult::OK)
			 {
				String ^path = this->openFileDialog1->FileName;
				char new_[100];
 		  //Translation to std::string::::::::::::::::::::::::::
				 pin_ptr<const wchar_t> wch = PtrToStringChars(path);
				 size_t sz = wcslen(wch) + 1;
				 size_t y = 0;
				 wcstombs_s(&y, new_, sz, wch, _TRUNCATE);
				 string s(new_);
				 int note = s.find_last_of('\\', s.length() - 1);
				
				 if(note == std::string::npos)
				 {
					 tmp = File::ReadAllText(path);
					 this->TextBox1->Clear();
					 this->TextBox1->Text = tmp;
					 this->Text::set("Turing Interpreter :: " + path);
					 return;			 
				 }
					 
				 string tr = "";
				 tr.insert(0, s, note + 1, s.length() - 1 - note);
				 String ^qw = gcnew String(tr.c_str());

			 tmp = File::ReadAllText(path);
			 this->TextBox1->Clear();
			 this->TextBox1->Text = tmp;
			 this->Text::set("Turing Interpreter :: " + qw);
			 }			
		 }
private: System::Void toolStripMenuItem2_Click(System::Object^  sender, System::EventArgs^  e) {
			 f = gcnew About();
			 f->Show();			
		 }
};
};


