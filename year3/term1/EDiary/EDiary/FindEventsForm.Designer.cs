namespace EDiary
{
    partial class FindEventsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.findButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.findByTextRB = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.findByDateRB = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lessThanRB = new System.Windows.Forms.RadioButton();
            this.equalRB = new System.Windows.Forms.RadioButton();
            this.greaterThanRB = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.findByAppRB = new System.Windows.Forms.RadioButton();
            this.appComboBox = new System.Windows.Forms.ComboBox();
            this.findByPersonRB = new System.Windows.Forms.RadioButton();
            this.personComboBox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.searchAllRB = new System.Windows.Forms.RadioButton();
            this.searchLoadedRB = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.personComboBox);
            this.panel1.Controls.Add(this.findByPersonRB);
            this.panel1.Controls.Add(this.appComboBox);
            this.panel1.Controls.Add(this.findByAppRB);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.findByDateRB);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.findByTextRB);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(286, 342);
            this.panel1.TabIndex = 0;
            // 
            // findButton
            // 
            this.findButton.Location = new System.Drawing.Point(42, 360);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(84, 32);
            this.findButton.TabIndex = 1;
            this.findButton.Text = "Find";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(182, 360);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(84, 32);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // findByTextRB
            // 
            this.findByTextRB.AutoSize = true;
            this.findByTextRB.Checked = true;
            this.findByTextRB.Location = new System.Drawing.Point(3, 3);
            this.findByTextRB.Name = "findByTextRB";
            this.findByTextRB.Size = new System.Drawing.Size(79, 17);
            this.findByTextRB.TabIndex = 0;
            this.findByTextRB.TabStop = true;
            this.findByTextRB.Text = "Find by text";
            this.findByTextRB.UseVisualStyleBackColor = true;
            this.findByTextRB.CheckedChanged += new System.EventHandler(this.findByTextRB_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(36, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 20);
            this.textBox1.TabIndex = 1;
            // 
            // findByDateRB
            // 
            this.findByDateRB.AutoSize = true;
            this.findByDateRB.Location = new System.Drawing.Point(3, 54);
            this.findByDateRB.Name = "findByDateRB";
            this.findByDateRB.Size = new System.Drawing.Size(83, 17);
            this.findByDateRB.TabIndex = 2;
            this.findByDateRB.Text = "Find by date";
            this.findByDateRB.UseVisualStyleBackColor = true;
            this.findByDateRB.CheckedChanged += new System.EventHandler(this.findByDateRB_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(36, 77);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(235, 75);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.greaterThanRB);
            this.panel3.Controls.Add(this.equalRB);
            this.panel3.Controls.Add(this.lessThanRB);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(103, 69);
            this.panel3.TabIndex = 0;
            // 
            // lessThanRB
            // 
            this.lessThanRB.AutoSize = true;
            this.lessThanRB.Location = new System.Drawing.Point(3, 3);
            this.lessThanRB.Name = "lessThanRB";
            this.lessThanRB.Size = new System.Drawing.Size(71, 17);
            this.lessThanRB.TabIndex = 0;
            this.lessThanRB.Text = "Less than";
            this.lessThanRB.UseVisualStyleBackColor = true;
            // 
            // equalRB
            // 
            this.equalRB.AutoSize = true;
            this.equalRB.Checked = true;
            this.equalRB.Location = new System.Drawing.Point(3, 23);
            this.equalRB.Name = "equalRB";
            this.equalRB.Size = new System.Drawing.Size(52, 17);
            this.equalRB.TabIndex = 1;
            this.equalRB.TabStop = true;
            this.equalRB.Text = "Equal";
            this.equalRB.UseVisualStyleBackColor = true;
            // 
            // greaterThanRB
            // 
            this.greaterThanRB.AutoSize = true;
            this.greaterThanRB.Location = new System.Drawing.Point(3, 46);
            this.greaterThanRB.Name = "greaterThanRB";
            this.greaterThanRB.Size = new System.Drawing.Size(84, 17);
            this.greaterThanRB.TabIndex = 2;
            this.greaterThanRB.Text = "Greater than";
            this.greaterThanRB.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(142, 24);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(90, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // findByAppRB
            // 
            this.findByAppRB.AutoSize = true;
            this.findByAppRB.Location = new System.Drawing.Point(3, 164);
            this.findByAppRB.Name = "findByAppRB";
            this.findByAppRB.Size = new System.Drawing.Size(120, 17);
            this.findByAppRB.TabIndex = 4;
            this.findByAppRB.Text = "Find by appointment";
            this.findByAppRB.UseVisualStyleBackColor = true;
            this.findByAppRB.CheckedChanged += new System.EventHandler(this.findByAppRB_CheckedChanged);
            // 
            // appComboBox
            // 
            this.appComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.appComboBox.Enabled = false;
            this.appComboBox.FormattingEnabled = true;
            this.appComboBox.Location = new System.Drawing.Point(36, 187);
            this.appComboBox.Name = "appComboBox";
            this.appComboBox.Size = new System.Drawing.Size(217, 21);
            this.appComboBox.TabIndex = 5;
            // 
            // findByPersonRB
            // 
            this.findByPersonRB.AutoSize = true;
            this.findByPersonRB.Location = new System.Drawing.Point(3, 227);
            this.findByPersonRB.Name = "findByPersonRB";
            this.findByPersonRB.Size = new System.Drawing.Size(94, 17);
            this.findByPersonRB.TabIndex = 6;
            this.findByPersonRB.Text = "Find by person";
            this.findByPersonRB.UseVisualStyleBackColor = true;
            this.findByPersonRB.CheckedChanged += new System.EventHandler(this.findByPersonRB_CheckedChanged);
            // 
            // personComboBox
            // 
            this.personComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.personComboBox.Enabled = false;
            this.personComboBox.FormattingEnabled = true;
            this.personComboBox.Location = new System.Drawing.Point(36, 250);
            this.personComboBox.Name = "personComboBox";
            this.personComboBox.Size = new System.Drawing.Size(217, 21);
            this.personComboBox.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.searchAllRB);
            this.flowLayoutPanel1.Controls.Add(this.searchLoadedRB);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(61, 284);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(102, 45);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // searchAllRB
            // 
            this.searchAllRB.AutoSize = true;
            this.searchAllRB.Checked = true;
            this.searchAllRB.Location = new System.Drawing.Point(3, 3);
            this.searchAllRB.Name = "searchAllRB";
            this.searchAllRB.Size = new System.Drawing.Size(70, 17);
            this.searchAllRB.TabIndex = 0;
            this.searchAllRB.TabStop = true;
            this.searchAllRB.Text = "all events";
            this.searchAllRB.UseVisualStyleBackColor = true;
            // 
            // searchLoadedRB
            // 
            this.searchLoadedRB.AutoSize = true;
            this.searchLoadedRB.Location = new System.Drawing.Point(3, 26);
            this.searchLoadedRB.Name = "searchLoadedRB";
            this.searchLoadedRB.Size = new System.Drawing.Size(92, 17);
            this.searchLoadedRB.TabIndex = 1;
            this.searchLoadedRB.Text = "loaded events";
            this.searchLoadedRB.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Search in:";
            // 
            // FindEventsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(308, 396);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.findButton);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindEventsForm";
            this.Opacity = 0.9;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Events";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton findByAppRB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton greaterThanRB;
        private System.Windows.Forms.RadioButton equalRB;
        private System.Windows.Forms.RadioButton lessThanRB;
        private System.Windows.Forms.RadioButton findByDateRB;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton findByTextRB;
        private System.Windows.Forms.ComboBox appComboBox;
        private System.Windows.Forms.ComboBox personComboBox;
        private System.Windows.Forms.RadioButton findByPersonRB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton searchAllRB;
        private System.Windows.Forms.RadioButton searchLoadedRB;
    }
}