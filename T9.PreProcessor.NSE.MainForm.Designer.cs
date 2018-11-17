namespace T9.PreProcessor.NSE
{
    partial class FormMain
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.receiversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nSECMMCASTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nSEFOMCASTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nSECFMCASTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.netMQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbEcho = new System.Windows.Forms.TextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.checkBoxShow = new System.Windows.Forms.CheckBox();
            this.timerCheck = new System.Windows.Forms.Timer(this.components);
            this.checkBoxChunk = new System.Windows.Forms.CheckBox();
            this.checkBoxTraceStats = new System.Windows.Forms.CheckBox();
            this.externalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.receiversToolStripMenuItem,
            this.sendersToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(389, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // receiversToolStripMenuItem
            // 
            this.receiversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nSECMMCASTToolStripMenuItem,
            this.nSEFOMCASTToolStripMenuItem,
            this.nSECFMCASTToolStripMenuItem});
            this.receiversToolStripMenuItem.Name = "receiversToolStripMenuItem";
            this.receiversToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.receiversToolStripMenuItem.Text = "Raw In";
            // 
            // nSECMMCASTToolStripMenuItem
            // 
            this.nSECMMCASTToolStripMenuItem.Name = "nSECMMCASTToolStripMenuItem";
            this.nSECMMCASTToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.nSECMMCASTToolStripMenuItem.Text = "NSE CM - MCAST";
            this.nSECMMCASTToolStripMenuItem.Click += new System.EventHandler(this.nSECMMCASTToolStripMenuItem_Click);
            // 
            // nSEFOMCASTToolStripMenuItem
            // 
            this.nSEFOMCASTToolStripMenuItem.Name = "nSEFOMCASTToolStripMenuItem";
            this.nSEFOMCASTToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.nSEFOMCASTToolStripMenuItem.Text = "NSE FO - MCAST";
            this.nSEFOMCASTToolStripMenuItem.Click += new System.EventHandler(this.nSEFOMCASTToolStripMenuItem_Click);
            // 
            // nSECFMCASTToolStripMenuItem
            // 
            this.nSECFMCASTToolStripMenuItem.Name = "nSECFMCASTToolStripMenuItem";
            this.nSECFMCASTToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.nSECFMCASTToolStripMenuItem.Text = "NSE CF - MCAST";
            this.nSECFMCASTToolStripMenuItem.Click += new System.EventHandler(this.nSECFMCASTToolStripMenuItem_Click);
            // 
            // sendersToolStripMenuItem
            // 
            this.sendersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.netMQToolStripMenuItem});
            this.sendersToolStripMenuItem.Name = "sendersToolStripMenuItem";
            this.sendersToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.sendersToolStripMenuItem.Text = "PP Out";
            // 
            // netMQToolStripMenuItem
            // 
            this.netMQToolStripMenuItem.Name = "netMQToolStripMenuItem";
            this.netMQToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.netMQToolStripMenuItem.Text = "MCast Sender";
            this.netMQToolStripMenuItem.Click += new System.EventHandler(this.netMQToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.externalToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.aboutToolStripMenuItem.Text = "Credits";
            // 
            // tbEcho
            // 
            this.tbEcho.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEcho.Location = new System.Drawing.Point(0, 82);
            this.tbEcho.Multiline = true;
            this.tbEcho.Name = "tbEcho";
            this.tbEcho.Size = new System.Drawing.Size(389, 162);
            this.tbEcho.TabIndex = 23;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(317, 53);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(59, 23);
            this.buttonClear.TabIndex = 30;
            this.buttonClear.TabStop = false;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // checkBoxShow
            // 
            this.checkBoxShow.AutoSize = true;
            this.checkBoxShow.Location = new System.Drawing.Point(12, 27);
            this.checkBoxShow.Name = "checkBoxShow";
            this.checkBoxShow.Size = new System.Drawing.Size(88, 17);
            this.checkBoxShow.TabIndex = 29;
            this.checkBoxShow.TabStop = false;
            this.checkBoxShow.Text = "Show Details";
            this.checkBoxShow.UseVisualStyleBackColor = true;
            // 
            // timerCheck
            // 
            this.timerCheck.Interval = 200;
            this.timerCheck.Tick += new System.EventHandler(this.timerCheck_Tick);
            // 
            // checkBoxChunk
            // 
            this.checkBoxChunk.AutoSize = true;
            this.checkBoxChunk.Checked = true;
            this.checkBoxChunk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxChunk.Location = new System.Drawing.Point(202, 27);
            this.checkBoxChunk.Name = "checkBoxChunk";
            this.checkBoxChunk.Size = new System.Drawing.Size(101, 17);
            this.checkBoxChunk.TabIndex = 31;
            this.checkBoxChunk.TabStop = false;
            this.checkBoxChunk.Text = "Send in Chunks";
            this.checkBoxChunk.UseVisualStyleBackColor = true;
            this.checkBoxChunk.CheckedChanged += new System.EventHandler(this.checkBoxChunk_CheckedChanged);
            // 
            // checkBoxTraceStats
            // 
            this.checkBoxTraceStats.AutoSize = true;
            this.checkBoxTraceStats.Checked = true;
            this.checkBoxTraceStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTraceStats.Location = new System.Drawing.Point(12, 53);
            this.checkBoxTraceStats.Name = "checkBoxTraceStats";
            this.checkBoxTraceStats.Size = new System.Drawing.Size(81, 17);
            this.checkBoxTraceStats.TabIndex = 72;
            this.checkBoxTraceStats.TabStop = false;
            this.checkBoxTraceStats.Text = "Trace Stats";
            this.checkBoxTraceStats.UseVisualStyleBackColor = true;
            this.checkBoxTraceStats.CheckedChanged += new System.EventHandler(this.checkBoxTraceStats_CheckedChanged);
            // 
            // externalToolStripMenuItem
            // 
            this.externalToolStripMenuItem.Name = "externalToolStripMenuItem";
            this.externalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.externalToolStripMenuItem.Text = "External";
            this.externalToolStripMenuItem.Click += new System.EventHandler(this.externalToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 243);
            this.Controls.Add(this.checkBoxTraceStats);
            this.Controls.Add(this.checkBoxChunk);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.checkBoxShow);
            this.Controls.Add(this.tbEcho);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PrepProcessor.NSE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem receiversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nSECMMCASTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nSEFOMCASTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nSECFMCASTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendersToolStripMenuItem;
        internal System.Windows.Forms.TextBox tbEcho;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.CheckBox checkBoxShow;
        private System.Windows.Forms.Timer timerCheck;
        private System.Windows.Forms.ToolStripMenuItem netMQToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxChunk;
        private System.Windows.Forms.CheckBox checkBoxTraceStats;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem externalToolStripMenuItem;
    }
}

