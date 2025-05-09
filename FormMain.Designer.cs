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
            button11 = new Button();
            button6 = new Button();
            dataGridView2 = new DataGridView();
            tabPage3 = new TabPage();
            panel4 = new Panel();
            dataGridView1 = new DataGridView();
            panel3 = new Panel();
            button12 = new Button();
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
            button1.Location = new Point(11, 202);
            button1.Margin = new Padding(1, 2, 1, 2);
            button1.Name = "button1";
            button1.Size = new Size(106, 42);
            button1.TabIndex = 1;
            button1.Text = "TCPSim";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(11, 78);
            button2.Margin = new Padding(1, 2, 1, 2);
            button2.Name = "button2";
            button2.Size = new Size(106, 42);
            button2.TabIndex = 2;
            button2.Text = "StartApp";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(11, 148);
            button3.Margin = new Padding(1, 2, 1, 2);
            button3.Name = "button3";
            button3.Size = new Size(106, 42);
            button3.TabIndex = 3;
            button3.Text = "RS232Sim";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(11, 22);
            button4.Margin = new Padding(1, 2, 1, 2);
            button4.Name = "button4";
            button4.Size = new Size(106, 42);
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
            panel1.Controls.Add(button12);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(1049, 10);
            panel1.Margin = new Padding(1, 2, 1, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(133, 479);
            panel1.TabIndex = 6;
            // 
            // button5
            // 
            button5.Location = new Point(11, 295);
            button5.Margin = new Padding(1, 2, 1, 2);
            button5.Name = "button5";
            button5.Size = new Size(106, 42);
            button5.TabIndex = 6;
            button5.Text = "RESET";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(listBox1);
            tabPage2.Location = new Point(4, 34);
            tabPage2.Margin = new Padding(1, 2, 1, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(1, 2, 1, 2);
            tabPage2.Size = new Size(1011, 479);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Console";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(1, 2);
            listBox1.Margin = new Padding(1, 2, 1, 2);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(1009, 475);
            listBox1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(dataGridView2);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Margin = new Padding(1, 2, 1, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(1, 2, 1, 2);
            tabPage1.Size = new Size(1011, 479);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Data";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(button11);
            panel2.Controls.Add(button6);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(1, 395);
            panel2.Margin = new Padding(1, 2, 1, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1009, 82);
            panel2.TabIndex = 6;
            // 
            // button11
            // 
            button11.Location = new Point(681, 20);
            button11.Margin = new Padding(1, 2, 1, 2);
            button11.Name = "button11";
            button11.Size = new Size(161, 42);
            button11.TabIndex = 8;
            button11.Text = "Clear DB";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button6
            // 
            button6.Location = new Point(846, 20);
            button6.Margin = new Padding(1, 2, 1, 2);
            button6.Name = "button6";
            button6.Size = new Size(161, 42);
            button6.TabIndex = 7;
            button6.Text = "Re-post Data";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(1, 2);
            dataGridView2.Margin = new Padding(1, 2, 1, 2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 72;
            dataGridView2.RowTemplate.Height = 37;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Size = new Size(1009, 475);
            dataGridView2.TabIndex = 5;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.CellMouseDoubleClick += dataGridView2_CellMouseDoubleClick;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel4);
            tabPage3.Controls.Add(panel3);
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1011, 479);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Machines";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.Controls.Add(dataGridView1);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 121);
            panel4.Name = "panel4";
            panel4.Size = new Size(1005, 355);
            panel4.TabIndex = 6;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(1, 2, 1, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.RowTemplate.Height = 37;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1005, 355);
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
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1005, 118);
            panel3.TabIndex = 5;
            // 
            // button12
            // 
            button12.Location = new Point(11, 353);
            button12.Name = "button12";
            button12.Size = new Size(106, 44);
            button12.TabIndex = 20;
            button12.Text = "Upload ";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button10
            // 
            button10.Location = new Point(784, 62);
            button10.Margin = new Padding(4, 5, 4, 5);
            button10.Name = "button10";
            button10.Size = new Size(60, 35);
            button10.TabIndex = 19;
            button10.Text = "Help";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button9
            // 
            button9.Location = new Point(851, 57);
            button9.Name = "button9";
            button9.Size = new Size(150, 40);
            button9.TabIndex = 18;
            button9.Text = "ServerLog";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.Location = new Point(851, 7);
            button8.Name = "button8";
            button8.Size = new Size(150, 44);
            button8.TabIndex = 17;
            button8.Text = "Logfile";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.Location = new Point(594, 7);
            button7.Name = "button7";
            button7.Size = new Size(150, 102);
            button7.TabIndex = 16;
            button7.Text = "Reload";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // txtAPIURL
            // 
            txtAPIURL.Location = new Point(151, 77);
            txtAPIURL.MaxLength = 2000;
            txtAPIURL.Name = "txtAPIURL";
            txtAPIURL.Size = new Size(437, 31);
            txtAPIURL.TabIndex = 15;
            txtAPIURL.Text = "/openclinic/app/equipment/index3.php";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 82);
            label4.Name = "label4";
            label4.Size = new Size(75, 25);
            label4.TabIndex = 14;
            label4.Text = "API URL";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(151, 42);
            txtHost.MaxLength = 200;
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(437, 31);
            txtHost.TabIndex = 13;
            txtHost.Text = "https://tamduc.vnem.com";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 42);
            label3.Name = "label3";
            label3.Size = new Size(50, 25);
            label3.TabIndex = 12;
            label3.Text = "Host";
            // 
            // txtAppID
            // 
            txtAppID.Location = new Point(407, 7);
            txtAppID.MaxLength = 2;
            txtAppID.Name = "txtAppID";
            txtAppID.Size = new Size(183, 31);
            txtAppID.TabIndex = 11;
            txtAppID.Text = "1";
            txtAppID.TextAlign = HorizontalAlignment.Right;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(276, 7);
            label2.Name = "label2";
            label2.Size = new Size(125, 25);
            label2.TabIndex = 10;
            label2.Text = "Application ID";
            // 
            // txtClinicID
            // 
            txtClinicID.Location = new Point(151, 3);
            txtClinicID.MaxLength = 2;
            txtClinicID.Name = "txtClinicID";
            txtClinicID.Size = new Size(104, 31);
            txtClinicID.TabIndex = 9;
            txtClinicID.Text = "1";
            txtClinicID.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 7);
            label1.Name = "label1";
            label1.Size = new Size(76, 25);
            label1.TabIndex = 8;
            label1.Text = "Clinic ID";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(11, 10);
            tabControl1.Margin = new Padding(1, 2, 1, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1019, 517);
            tabControl1.TabIndex = 0;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 538);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Margin = new Padding(1, 2, 1, 2);
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
        private Button button12;
    }
}