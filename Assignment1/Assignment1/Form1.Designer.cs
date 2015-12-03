namespace Assignment1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWordlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCrozzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crozzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.scoreContainer = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.timeContainer = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dGV1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.validContainer = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.levelContainer = new System.Windows.Forms.Label();
            this.wordListContainer = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.crozzleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(735, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWordlistToolStripMenuItem,
            this.openCrozzleToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openWordlistToolStripMenuItem
            // 
            this.openWordlistToolStripMenuItem.Name = "openWordlistToolStripMenuItem";
            this.openWordlistToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.openWordlistToolStripMenuItem.Text = "Open Wordlist";
            this.openWordlistToolStripMenuItem.Click += new System.EventHandler(this.openWordlistToolStripMenuItem_Click);
            // 
            // openCrozzleToolStripMenuItem
            // 
            this.openCrozzleToolStripMenuItem.Name = "openCrozzleToolStripMenuItem";
            this.openCrozzleToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.openCrozzleToolStripMenuItem.Text = "Open Crozzle";
            this.openCrozzleToolStripMenuItem.Click += new System.EventHandler(this.openCrozzleToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // crozzleToolStripMenuItem
            // 
            this.crozzleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.crozzleToolStripMenuItem.Name = "crozzleToolStripMenuItem";
            this.crozzleToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.crozzleToolStripMenuItem.Text = "Crozzle";
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 267);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Score:";
            // 
            // scoreContainer
            // 
            this.scoreContainer.AutoSize = true;
            this.scoreContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scoreContainer.Location = new System.Drawing.Point(56, 267);
            this.scoreContainer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.scoreContainer.Name = "scoreContainer";
            this.scoreContainer.Size = new System.Drawing.Size(45, 17);
            this.scoreContainer.TabIndex = 3;
            this.scoreContainer.Text = "            ";
            this.scoreContainer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 267);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Time Left:";
            // 
            // timeContainer
            // 
            this.timeContainer.AutoSize = true;
            this.timeContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.timeContainer.Location = new System.Drawing.Point(231, 265);
            this.timeContainer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.timeContainer.Name = "timeContainer";
            this.timeContainer.Size = new System.Drawing.Size(33, 17);
            this.timeContainer.TabIndex = 5;
            this.timeContainer.Text = "time";
            this.timeContainer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV1.Location = new System.Drawing.Point(414, 29);
            this.dGV1.Name = "dataGridView1";
            this.dGV1.Size = new System.Drawing.Size(314, 228);
            this.dGV1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Validity";
            // 
            // validContainer
            // 
            this.validContainer.AutoSize = true;
            this.validContainer.Location = new System.Drawing.Point(63, 297);
            this.validContainer.Name = "validContainer";
            this.validContainer.Size = new System.Drawing.Size(85, 15);
            this.validContainer.TabIndex = 9;
            this.validContainer.Text = "validContainer";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(308, 267);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(415, 53);
            this.progressBar1.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(166, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Difficulty";
            // 
            // levelContainer
            // 
            this.levelContainer.AutoSize = true;
            this.levelContainer.Location = new System.Drawing.Point(223, 297);
            this.levelContainer.Name = "levelContainer";
            this.levelContainer.Size = new System.Drawing.Size(41, 15);
            this.levelContainer.TabIndex = 16;
            this.levelContainer.Text = "label5";
            // 
            // wordListContainer
            // 
            this.wordListContainer.FormattingEnabled = true;
            this.wordListContainer.Location = new System.Drawing.Point(308, 32);
            this.wordListContainer.Name = "wordListContainer";
            this.wordListContainer.Size = new System.Drawing.Size(100, 225);
            this.wordListContainer.TabIndex = 17;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(289, 222);
            this.textBox1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 330);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.wordListContainer);
            this.Controls.Add(this.levelContainer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.validContainer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dGV1);
            this.Controls.Add(this.timeContainer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.scoreContainer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = " ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWordlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCrozzleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label scoreContainer;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label timeContainer;
        private System.Windows.Forms.ToolStripMenuItem crozzleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridView dGV1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label validContainer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label levelContainer;
        private System.Windows.Forms.ListBox wordListContainer;
        private System.Windows.Forms.TextBox textBox1;
    }
}

