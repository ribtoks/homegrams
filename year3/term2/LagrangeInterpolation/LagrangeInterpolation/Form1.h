#pragma once
#include "LagrInterpInterface.h"
#include <vector>
#include <algorithm>
#include <numeric>

namespace LagrangeInterpolation 
{

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Drawing::Drawing2D;

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

			bmp = gcnew System::Drawing::Bitmap(this->panel1->Width, this->panel1->Height);			
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
	private: System::Windows::Forms::Label^  label1;
	protected: 
	private: System::Windows::Forms::TextBox^  textBox1;
	private: System::Windows::Forms::TextBox^  textBox2;
	private: System::Windows::Forms::Label^  label2;
	private: System::Windows::Forms::Label^  label3;
	private: System::Windows::Forms::NumericUpDown^  nUD;

	private: System::Windows::Forms::Button^  button1;
	private: System::Windows::Forms::Panel^  panel1;

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;
	private: System::Windows::Forms::FlowLayoutPanel^  flowLayoutPanel1;
	private: System::Windows::Forms::RadioButton^  radioButton1;
	private: System::Windows::Forms::RadioButton^  radioButton2;
	private: System::Windows::Forms::NumericUpDown^  pointsNumberUD;

	private: System::Windows::Forms::Label^  label4;

		System::Drawing::Bitmap ^bmp;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->textBox2 = (gcnew System::Windows::Forms::TextBox());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->label3 = (gcnew System::Windows::Forms::Label());
			this->nUD = (gcnew System::Windows::Forms::NumericUpDown());
			this->button1 = (gcnew System::Windows::Forms::Button());
			this->panel1 = (gcnew System::Windows::Forms::Panel());
			this->flowLayoutPanel1 = (gcnew System::Windows::Forms::FlowLayoutPanel());
			this->radioButton1 = (gcnew System::Windows::Forms::RadioButton());
			this->radioButton2 = (gcnew System::Windows::Forms::RadioButton());
			this->pointsNumberUD = (gcnew System::Windows::Forms::NumericUpDown());
			this->label4 = (gcnew System::Windows::Forms::Label());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->nUD))->BeginInit();
			this->flowLayoutPanel1->SuspendLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pointsNumberUD))->BeginInit();
			this->SuspendLayout();
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(27, 28);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(61, 13);
			this->label1->TabIndex = 0;
			this->label1->Text = L"Left bound:";
			// 
			// textBox1
			// 
			this->textBox1->Location = System::Drawing::Point(100, 25);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(75, 20);
			this->textBox1->TabIndex = 1;
			this->textBox1->Text = L"-5";
			// 
			// textBox2
			// 
			this->textBox2->Location = System::Drawing::Point(100, 51);
			this->textBox2->Name = L"textBox2";
			this->textBox2->Size = System::Drawing::Size(75, 20);
			this->textBox2->TabIndex = 3;
			this->textBox2->Text = L"5";
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(20, 54);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(68, 13);
			this->label2->TabIndex = 2;
			this->label2->Text = L"Right bound:";
			// 
			// label3
			// 
			this->label3->AutoSize = true;
			this->label3->Location = System::Drawing::Point(11, 131);
			this->label3->Name = L"label3";
			this->label3->Size = System::Drawing::Size(77, 13);
			this->label3->TabIndex = 4;
			this->label3->Text = L"Points number:";
			// 
			// nUD
			// 
			this->nUD->Location = System::Drawing::Point(100, 98);
			this->nUD->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {1, 0, 0, 0});
			this->nUD->Name = L"nUD";
			this->nUD->Size = System::Drawing::Size(75, 20);
			this->nUD->TabIndex = 5;
			this->nUD->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {80, 0, 0, 0});
			// 
			// button1
			// 
			this->button1->BackColor = System::Drawing::Color::WhiteSmoke;
			this->button1->Location = System::Drawing::Point(100, 188);
			this->button1->Name = L"button1";
			this->button1->Size = System::Drawing::Size(75, 23);
			this->button1->TabIndex = 6;
			this->button1->Text = L"Draw!";
			this->button1->UseVisualStyleBackColor = false;
			this->button1->Click += gcnew System::EventHandler(this, &Form1::button1_Click);
			// 
			// panel1
			// 
			this->panel1->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom) 
				| System::Windows::Forms::AnchorStyles::Left) 
				| System::Windows::Forms::AnchorStyles::Right));
			this->panel1->BackColor = System::Drawing::Color::White;
			this->panel1->Location = System::Drawing::Point(197, 12);
			this->panel1->Name = L"panel1";
			this->panel1->Size = System::Drawing::Size(455, 344);
			this->panel1->TabIndex = 7;
			this->panel1->Paint += gcnew System::Windows::Forms::PaintEventHandler(this, &Form1::panel1_Paint);
			// 
			// flowLayoutPanel1
			// 
			this->flowLayoutPanel1->Controls->Add(this->radioButton1);
			this->flowLayoutPanel1->Controls->Add(this->radioButton2);
			this->flowLayoutPanel1->Location = System::Drawing::Point(5, 176);
			this->flowLayoutPanel1->Name = L"flowLayoutPanel1";
			this->flowLayoutPanel1->Size = System::Drawing::Size(89, 46);
			this->flowLayoutPanel1->TabIndex = 8;
			// 
			// radioButton1
			// 
			this->radioButton1->AutoSize = true;
			this->radioButton1->Checked = true;
			this->radioButton1->Cursor = System::Windows::Forms::Cursors::Default;
			this->radioButton1->Location = System::Drawing::Point(3, 3);
			this->radioButton1->Name = L"radioButton1";
			this->radioButton1->Size = System::Drawing::Size(77, 17);
			this->radioButton1->TabIndex = 0;
			this->radioButton1->TabStop = true;
			this->radioButton1->Text = L"Equidistant";
			this->radioButton1->UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this->radioButton2->AutoSize = true;
			this->radioButton2->Location = System::Drawing::Point(3, 26);
			this->radioButton2->Name = L"radioButton2";
			this->radioButton2->Size = System::Drawing::Size(78, 17);
			this->radioButton2->TabIndex = 1;
			this->radioButton2->Text = L"Chebyshev";
			this->radioButton2->UseVisualStyleBackColor = true;
			// 
			// pointsNumberUD
			// 
			this->pointsNumberUD->Location = System::Drawing::Point(100, 129);
			this->pointsNumberUD->Minimum = System::Decimal(gcnew cli::array< System::Int32 >(4) {1, 0, 0, 0});
			this->pointsNumberUD->Name = L"pointsNumberUD";
			this->pointsNumberUD->Size = System::Drawing::Size(75, 20);
			this->pointsNumberUD->TabIndex = 10;
			this->pointsNumberUD->Value = System::Decimal(gcnew cli::array< System::Int32 >(4) {20, 0, 0, 0});
			// 
			// label4
			// 
			this->label4->AutoSize = true;
			this->label4->Location = System::Drawing::Point(11, 92);
			this->label4->Name = L"label4";
			this->label4->Size = System::Drawing::Size(83, 26);
			this->label4->TabIndex = 9;
			this->label4->Text = L"Segment points \r\ncount";
			// 
			// Form1
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->BackColor = System::Drawing::Color::WhiteSmoke;
			this->ClientSize = System::Drawing::Size(669, 372);
			this->Controls->Add(this->pointsNumberUD);
			this->Controls->Add(this->label4);
			this->Controls->Add(this->flowLayoutPanel1);
			this->Controls->Add(this->panel1);
			this->Controls->Add(this->button1);
			this->Controls->Add(this->nUD);
			this->Controls->Add(this->label3);
			this->Controls->Add(this->textBox2);
			this->Controls->Add(this->label2);
			this->Controls->Add(this->textBox1);
			this->Controls->Add(this->label1);
			this->MinimumSize = System::Drawing::Size(677, 406);
			this->Name = L"Form1";
			this->StartPosition = System::Windows::Forms::FormStartPosition::CenterScreen;
			this->Text = L"Form1";
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->nUD))->EndInit();
			this->flowLayoutPanel1->ResumeLayout(false);
			this->flowLayoutPanel1->PerformLayout();
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^  >(this->pointsNumberUD))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void button1_Click(System::Object^  sender, System::EventArgs^  e) {
				 bmp = gcnew Bitmap(this->panel1->Width, this->panel1->Height);

				 if (this->textBox1->Text->Length == 0)
				 {
					 MessageBox::Show("No left bound entered!", "Cannot draw", 
						 MessageBoxButtons::OK, MessageBoxIcon::Warning);
					 this->textBox1->Focus();
					 return;
				 }

				 if (this->textBox2->Text->Length == 0)
				 {
					 MessageBox::Show("No right bound entered!", "Cannot draw", 
						 MessageBoxButtons::OK, MessageBoxIcon::Warning);
					 this->textBox2->Focus();
					 return;
				 }

				Graphics^ gc = Graphics::FromImage(bmp);
				gc->Clear(Color::White);
				gc->CompositingQuality = CompositingQuality::HighQuality;
				gc->InterpolationMode = InterpolationMode::HighQualityBicubic;
				gc->SmoothingMode = SmoothingMode::AntiAlias;

				double a = Convert::ToDouble(this->textBox1->Text);
				double b = Convert::ToDouble(this->textBox2->Text);

				double maxY = 0, minY = 0;

				DrawRealRungeFunction(gc, minY, maxY);

				// number of interpolation points
				int pointsNumber = (int)this->pointsNumberUD->Value;
				vector<double> points;

				// equidistant
				if (this->radioButton1->Checked)
					points = GetEquidistantPoints(pointsNumber, a, b);
				else
					points = GetChebyshPoints(pointsNumber);

				DrawInterpolatedFunction(gc, RungeFunction, points, minY, maxY, (int)this->nUD->Value);

				this->panel1->Refresh();
			 }
private: System::Void panel1_Paint(System::Object^  sender, System::Windows::Forms::PaintEventArgs^  e) {

			 Graphics^ gc = e->Graphics;
			 gc->DrawImage(bmp, 0, 0);
		 }

private: System::Void DrawRealRungeFunction(Graphics^ gc, double& minY, double& maxY)
		 {
			 double a = Convert::ToDouble(this->textBox1->Text);
			 double b = Convert::ToDouble(this->textBox2->Text);

			 int n = 10000;
			 vector<double> points = GetEquidistantPoints(n, a, b);
			 vector<double> values = ApplyFunction(RungeFunction, points);

			 maxY = *(max_element(values.begin(), values.end()));
			 minY = *(min_element(values.begin(), values.end()));

			 double stepX = this->bmp->Width / (b - a);
			 double stepY = this->bmp->Height / (maxY - minY);

			 for (int i = 0; i < points.size() - 1; ++i)
			 {
				 double x1 = points[i];
				 double y1 = values[i];

				 double x2 = points[i + 1];
				 double y2 = values[i + 1];

				 x1 = (x1 - a)*stepX;
				 x2 = (x2 - a)*stepX;

				 y1 = (maxY - y1)*stepY;
				 y2 = (maxY - y2)*stepY;

				 gc->DrawLine(Pens::RoyalBlue, (float)x1, (float)y1, (float)x2, (float)y2);
			 }
		 }

private: System::Void DrawInterpolatedFunction(Graphics^ gc, UsualFunction func, const vector<double>& points, double minY, double maxY, int n)
		 {
			 double a = Convert::ToDouble(this->textBox1->Text);
			 double b = Convert::ToDouble(this->textBox2->Text);

			 vector<double> pointsAll = GetEquidistantPoints(n, a, b);
			 vector<double> values;
			 for (int i = 0; i < pointsAll.size(); ++i)
				 values.push_back(InterpolateByLagrange(func, points, pointsAll[i]));

			 double maxYinner = *(max_element(values.begin(), values.end()));
			 double minYinner = *(min_element(values.begin(), values.end()));


			 double stepX = this->bmp->Width / (b - a);
			 double stepY = this->bmp->Height / (maxY - minY);

			 for (int i = 0; i < pointsAll.size() - 1; ++i)
			 {
				 double x1 = pointsAll[i];
				 double y1 = values[i];

				 double x2 = pointsAll[i + 1];
				 double y2 = values[i + 1];

				 x1 = (x1 - a)*stepX;
				 x2 = (x2 - a)*stepX;

				 y1 = (maxY - y1)*stepY;
				 y2 = (maxY - y2)*stepY;

				 try
				 {
					gc->DrawLine(Pens::ForestGreen, (float)x1, (float)y1, (float)x2, (float)y2);
				}
				 catch(...)
				 {
				 }
			 }
		 }
};
}

