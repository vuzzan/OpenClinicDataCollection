namespace HL7.Dotnetcore
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
            components = new System.ComponentModel.Container();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            notifyIcon1 = new NotifyIcon(components);
            panel1 = new Panel();
            button5 = new Button();
            tabPage2 = new TabPage();
            listBox1 = new ListBox();
            tabPage1 = new TabPage();
            panel2 = new Panel();
            button6 = new Button();
            dataGridView2 = new DataGridView();
            tabPage3 = new TabPage();
            panel4 = new Panel();
            dataGridView1 = new DataGridView();
            panel3 = new Panel();
            button10 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            txtAPIURL = new TextBox();
            label4 = new Label();
            txtHost = new TextBox();
            label3 = new Label();
            txtAppID = new TextBox();
            label2 = new Label();
            txtClinicID = new TextBox();
            label1 = new Label();
            tabControl1 = new TabControl();
            button11 = new Button();
            panel1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tabPage3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(8, 121);
            button1.Margin = new Padding(1);
            button1.Name = "button1";
            button1.Size = new Size(74, 25);
            button1.TabIndex = 1;
            button1.Text = "TCPSim";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(8, 47);
            button2.Margin = new Padding(1);
            button2.Name = "button2";
            button2.Size = new Size(74, 25);
            button2.TabIndex = 2;
            button2.Text = "StartApp";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(8, 89);
            button3.Margin = new Padding(1);
            button3.Name = "button3";
            button3.Size = new Size(74, 25);
            button3.TabIndex = 3;
            button3.Text = "RS232Sim";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(8, 13);
            button4.Margin = new Padding(1);
            button4.Name = "button4";
            button4.Size = new Size(74, 25);
            button4.TabIndex = 5;
            button4.Text = "SendTest";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "OpenClinic Data Collection";
            notifyIcon1.Visible = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(734, 6);
            panel1.Margin = new Padding(1);
            panel1.Name = "panel1";
            panel1.Size = new Size(93, 229);
            panel1.TabIndex = 6;
            // 
            // button5
            // 
            button5.Location = new Point(8, 177);
            button5.Margin = new Padding(1);
            button5.Name = "button5";
            button5.Size = new Size(74, 25);
            button5.TabIndex = 6;
            button5.Text = "RESET";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(listBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(1);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(1);
            tabPage2.Size = new Size(705, 282);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Console";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(1, 1);
            listBox1.Margin = new Padding(1);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(703, 280);
            listBox1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(dataGridView2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(1);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(1);
            tabPage1.Size = new Size(705, 282);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Data";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(button11);
            panel2.Controls.Add(button6);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(1, 232);
            panel2.Margin = new Padding(1);
            panel2.Name = "panel2";
            panel2.Size = new Size(703, 49);
            panel2.TabIndex = 6;
            // 
            // button6
            // 
            button6.Location = new Point(592, 12);
            button6.Margin = new Padding(1);
            button6.Name = "button6";
            button6.Size = new Size(113, 25);
            button6.TabIndex = 7;
            button6.Text = "Re-post Data";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(1, 1);
            dataGridView2.Margin = new Padding(1);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 72;
            dataGridView2.RowTemplate.Height = 37;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Size = new Size(703, 280);
            dataGridView2.TabIndex = 5;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.CellMouseDoubleClick += dataGridView2_CellMouseDoubleClick;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel4);
            tabPage3.Controls.Add(panel3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(2);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(2);
            tabPage3.Size = new Size(705, 282);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Machines";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.Controls.Add(dataGridView1);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(2, 73);
            panel4.Margin = new Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new Size(701, 207);
            panel4.TabIndex = 6;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(1);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.RowTemplate.Height = 37;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(701, 207);
            dataGridView1.TabIndex = 4;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.SizeChanged += dataGridView1_SizeChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(button10);
            panel3.Controls.Add(button9);
            panel3.Controls.Add(button8);
            panel3.Controls.Add(button7);
            panel3.Controls.Add(txtAPIURL);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(txtHost);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(txtAppID);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(txtClinicID);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(2, 2);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(701, 71);
            panel3.TabIndex = 5;
            // 
            // button10
            // 
            button10.Location = new Point(549, 37);
            button10.Name = "button10";
            button10.Size = new Size(42, 21);
            button10.TabIndex = 19;
            button10.Text = "Help";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button9
            // 
            button9.Location = new Point(596, 34);
            button9.Margin = new Padding(2);
            button9.Name = "button9";
            button9.Size = new Size(105, 26);
            button9.TabIndex = 18;
            button9.Text = "ServerLog";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.Location = new Point(596, 4);
            button8.Margin = new Padding(2);
            button8.Name = "button8";
            button8.Size = new Size(105, 26);
            button8.TabIndex = 17;
            button8.Text = "Logfile";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Location = new Point(416, 4);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new Size(105, 61);
            button7.TabIndex = 16;
            button7.Text = "Reload";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // txtAPIURL
            // 
            txtAPIURL.Location = new Point(106, 46);
            txtAPIURL.Margin = new Padding(2);
            txtAPIURL.MaxLength = 2000;
            txtAPIURL.Name = "txtAPIURL";
            txtAPIURL.Size = new Size(307, 23);
            txtAPIURL.TabIndex = 15;
            txtAPIURL.Text = "/openclinic/app/equipment/index3.php";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 49);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(49, 15);
            label4.TabIndex = 14;
            label4.Text = "API URL";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(106, 25);
            txtHost.Margin = new Padding(2);
            txtHost.MaxLength = 200;
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(307, 23);
            txtHost.TabIndex = 13;
            txtHost.Text = "https://tamduc.vnem.com";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 25);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 12;
            label3.Text = "Host";
            // 
            // txtAppID
            // 
            txtAppID.Location = new Point(285, 4);
            txtAppID.Margin = new Padding(2);
            txtAppID.MaxLength = 2;
            txtAppID.Name = "txtAppID";
            txtAppID.Size = new Size(129, 23);
            txtAppID.TabIndex = 11;
            txtAppID.Text = "1";
            txtAppID.TextAlign = HorizontalAlignment.Right;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(193, 4);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(82, 15);
            label2.TabIndex = 10;
            label2.Text = "Application ID";
            // 
            // txtClinicID
            // 
            txtClinicID.Location = new Point(106, 2);
            txtClinicID.Margin = new Padding(2);
            txtClinicID.MaxLength = 2;
            txtClinicID.Name = "txtClinicID";
            txtClinicID.Size = new Size(74, 23);
            txtClinicID.TabIndex = 9;
            txtClinicID.Text = "1";
            txtClinicID.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 4);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 8;
            label1.Text = "Clinic ID";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(8, 6);
            tabControl1.Margin = new Padding(1);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(713, 310);
            tabControl1.TabIndex = 0;
            // 
            // button11
            // 
            button11.Location = new Point(477, 12);
            button11.Margin = new Padding(1);
            button11.Name = "button11";
            button11.Size = new Size(113, 25);
            button11.TabIndex = 8;
            button11.Text = "Clear DB";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 323);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Margin = new Padding(1);
            MaximizeBox = false;
            Name = "FormMain";
            Text = "OpenClinic Data Collection 2.1";
            MinimumSizeChanged += FormMain_MinimumSizeChanged;
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            panel1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tabPage3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private NotifyIcon notifyIcon1;
        private Panel panel1;
        private Button button5;
        private TabPage tabPage2;
        private ListBox listBox1;
        private TabPage tabPage1;
        private Panel panel2;
        private Button button6;
        private DataGridView dataGridView2;
        private TabPage tabPage3;
        private Panel panel4;
        private DataGridView dataGridView1;
        private Panel panel3;
        private TextBox txtAPIURL;
        private Label label4;
        private TextBox txtHost;
        private Label label3;
        private TextBox txtAppID;
        private Label label2;
        private TextBox txtClinicID;
        private Label label1;
        private TabControl tabControl1;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
    }
}