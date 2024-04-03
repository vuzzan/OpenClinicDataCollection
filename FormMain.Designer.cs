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
            listBox1 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            dataGridView1 = new DataGridView();
            button4 = new Button();
            notifyIcon1 = new NotifyIcon(components);
            panel1 = new Panel();
            button5 = new Button();
            splitContainer1 = new SplitContainer();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            panel2 = new Panel();
            button6 = new Button();
            dataGridView2 = new DataGridView();
            tabPage2 = new TabPage();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.HorizontalScrollbar = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(2, 2);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(813, 316);
            listBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(10, 162);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(84, 33);
            button1.TabIndex = 1;
            button1.Text = "TCPSim";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(10, 63);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(84, 33);
            button2.TabIndex = 2;
            button2.Text = "StartApp";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(10, 118);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(84, 33);
            button3.TabIndex = 3;
            button3.Text = "RS232Sim";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.RowTemplate.Height = 37;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(825, 130);
            dataGridView1.TabIndex = 4;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.SizeChanged += dataGridView1_SizeChanged;
            // 
            // button4
            // 
            button4.Location = new Point(10, 17);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(84, 33);
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
            panel1.Location = new Point(839, 8);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(107, 305);
            panel1.TabIndex = 6;
            // 
            // button5
            // 
            button5.Location = new Point(10, 236);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(84, 33);
            button5.TabIndex = 6;
            button5.Text = "SendTest";
            button5.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(825, 492);
            splitContainer1.SplitterDistance = 130;
            splitContainer1.SplitterWidth = 9;
            splitContainer1.TabIndex = 7;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(825, 353);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(dataGridView2);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(2);
            tabPage1.Size = new Size(817, 320);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Data";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(button6);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(2, 252);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(813, 66);
            panel2.TabIndex = 6;
            // 
            // button6
            // 
            button6.Location = new Point(676, 16);
            button6.Margin = new Padding(2);
            button6.Name = "button6";
            button6.Size = new Size(129, 33);
            button6.TabIndex = 7;
            button6.Text = "Re-post Data";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(2, 2);
            dataGridView2.Margin = new Padding(2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 72;
            dataGridView2.RowTemplate.Height = 37;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Size = new Size(813, 316);
            dataGridView2.TabIndex = 5;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.CellMouseDoubleClick += dataGridView2_CellMouseDoubleClick;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(listBox1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(2);
            tabPage2.Size = new Size(817, 320);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Console";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 492);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "FormMain";
            Text = "OpenClinic Data Collection 2.1";
            MinimumSizeChanged += FormMain_MinimumSizeChanged;
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private DataGridView dataGridView1;
        private Button button4;
        private NotifyIcon notifyIcon1;
        private Panel panel1;
        private SplitContainer splitContainer1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dataGridView2;
        private Panel panel2;
        private Button button5;
        private Button button6;
    }
}