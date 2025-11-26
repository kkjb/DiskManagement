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
            this.listBoxDisks = new System.Windows.Forms.ListBox();
            this.buttonOffline = new System.Windows.Forms.Button();
            this.buttonOnline = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxDisks
            // 
            this.listBoxDisks.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxDisks.FormattingEnabled = true;
            this.listBoxDisks.ItemHeight = 45;
            this.listBoxDisks.Location = new System.Drawing.Point(34, 34);
            this.listBoxDisks.Name = "listBoxDisks";
            this.listBoxDisks.Size = new System.Drawing.Size(1132, 724);
            this.listBoxDisks.TabIndex = 0;
            // 
            // buttonOffline
            // 
            this.buttonOffline.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOffline.ForeColor = System.Drawing.Color.Red;
            this.buttonOffline.Location = new System.Drawing.Point(1219, 34);
            this.buttonOffline.Name = "buttonOffline";
            this.buttonOffline.Size = new System.Drawing.Size(296, 103);
            this.buttonOffline.TabIndex = 1;
            this.buttonOffline.Text = "OFFLINE";
            this.buttonOffline.UseVisualStyleBackColor = true;
            this.buttonOffline.Click += new System.EventHandler(this.buttonOffline_Click);
            // 
            // buttonOnline
            // 
            this.buttonOnline.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOnline.ForeColor = System.Drawing.Color.Green;
            this.buttonOnline.Location = new System.Drawing.Point(1219, 655);
            this.buttonOnline.Name = "buttonOnline";
            this.buttonOnline.Size = new System.Drawing.Size(296, 103);
            this.buttonOnline.TabIndex = 2;
            this.buttonOnline.Text = "ONLINE";
            this.buttonOnline.UseVisualStyleBackColor = true;
            this.buttonOnline.Click += new System.EventHandler(this.buttonOnline_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefresh.ForeColor = System.Drawing.Color.Blue;
            this.buttonRefresh.Location = new System.Drawing.Point(1219, 332);
            this.buttonRefresh.Name = "Refresh";
            this.buttonRefresh.Size = new System.Drawing.Size(296, 103);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.Form1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1552, 811);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonOnline);
            this.Controls.Add(this.buttonOffline);
            this.Controls.Add(this.listBoxDisks);
            this.Name = "Form1";
            this.Text = "DiskHotplug";
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

