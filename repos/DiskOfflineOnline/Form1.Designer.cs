namespace DiskOfflineOnline
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBoxDisks = new System.Windows.Forms.ListBox();
            this.buttonOffline = new System.Windows.Forms.Button();
            this.buttonOnline = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxDisks
            // 
            this.listBoxDisks.BackColor = System.Drawing.SystemColors.WindowText;
            resources.ApplyResources(this.listBoxDisks, "listBoxDisks");
            this.listBoxDisks.ForeColor = System.Drawing.Color.Lime;
            this.listBoxDisks.FormattingEnabled = true;
            this.listBoxDisks.Name = "listBoxDisks";
            // 
            // buttonOffline
            // 
            this.buttonOffline.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonOffline.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.buttonOffline, "buttonOffline");
            this.buttonOffline.ForeColor = System.Drawing.Color.Red;
            this.buttonOffline.Name = "buttonOffline";
            this.buttonOffline.UseVisualStyleBackColor = false;
            this.buttonOffline.Click += new System.EventHandler(this.buttonOffline_Click);
            // 
            // buttonOnline
            // 
            this.buttonOnline.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.buttonOnline, "buttonOnline");
            this.buttonOnline.ForeColor = System.Drawing.Color.Lime;
            this.buttonOnline.Name = "buttonOnline";
            this.buttonOnline.UseVisualStyleBackColor = false;
            this.buttonOnline.Click += new System.EventHandler(this.buttonOnline_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.buttonRefresh, "buttonRefresh");
            this.buttonRefresh.ForeColor = System.Drawing.Color.Blue;
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.Form1_Load);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonOnline);
            this.Controls.Add(this.buttonOffline);
            this.Controls.Add(this.listBoxDisks);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDisks;
        private System.Windows.Forms.Button buttonOffline;
        private System.Windows.Forms.Button buttonOnline;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

