namespace OpenClinicDataCollection
{
    partial class FormRS232
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
            button1 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            comboBox1 = new ComboBox();
            textBox2 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(142, 12);
            button1.Name = "button1";
            button1.Size = new Size(118, 37);
            button1.TabIndex = 0;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(670, 12);
            button2.Name = "button2";
            button2.Size = new Size(118, 37);
            button2.TabIndex = 1;
            button2.Text = "SendData";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 30;
            listBox1.Location = new Point(12, 224);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(776, 214);
            listBox1.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 11);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(124, 38);
            comboBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 56);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(776, 162);
            textBox2.TabIndex = 6;
            textBox2.Text = "Test message from COM PORT";
            // 
            // FormRS232
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox2);
            Controls.Add(comboBox1);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "FormRS232";
            Text = "FormRS232";
            FormClosing += FormRS232_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private ListBox listBox1;
        private ComboBox comboBox1;
        private TextBox textBox2;
    }
}