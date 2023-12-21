namespace OpenClinicDataCollection
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 30;
            listBox1.Location = new Point(12, 337);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(1476, 334);
            listBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(1089, 12);
            button1.Name = "button1";
            button1.Size = new Size(92, 50);
            button1.TabIndex = 1;
            button1.Text = "TCPSim";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 12);
            button2.Name = "button2";
            button2.Size = new Size(126, 50);
            button2.TabIndex = 2;
            button2.Text = "StartApp";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(999, 12);
            button3.Name = "button3";
            button3.Size = new Size(84, 50);
            button3.TabIndex = 3;
            button3.Text = "RS232Sim";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column6 });
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.RowTemplate.Height = 37;
            dataGridView1.Size = new Size(1476, 319);
            dataGridView1.TabIndex = 4;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "MachineCode";
            Column1.HeaderText = "Column1";
            Column1.MinimumWidth = 9;
            Column1.Name = "Column1";
            Column1.Width = 175;
            // 
            // Column6
            // 
            Column6.DataPropertyName = "MachineName";
            Column6.HeaderText = "Column6";
            Column6.MinimumWidth = 9;
            Column6.Name = "Column6";
            Column6.Width = 175;
            // 
            // button4
            // 
            button4.Location = new Point(1043, 68);
            button4.Name = "button4";
            button4.Size = new Size(126, 50);
            button4.TabIndex = 5;
            button4.Text = "SendTest";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1500, 683);
            Controls.Add(dataGridView1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(button4);
            MaximizeBox = false;
            Name = "FormMain";
            Text = "OpenClinic Data Collection 2.1";
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private DataGridView dataGridView1;
        private Button button4;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column6;
    }
}