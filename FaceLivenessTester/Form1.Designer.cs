namespace FaceLivenessTester {
    partial class fmMain {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.btnExcute = new System.Windows.Forms.Button();
            this.lbResult = new System.Windows.Forms.ListBox();
            this.btnGetFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.openFD = new System.Windows.Forms.FolderBrowserDialog();
            this.btnExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExcute
            // 
            this.btnExcute.Location = new System.Drawing.Point(632, 24);
            this.btnExcute.Name = "btnExcute";
            this.btnExcute.Size = new System.Drawing.Size(75, 23);
            this.btnExcute.TabIndex = 0;
            this.btnExcute.Text = "執行";
            this.btnExcute.UseVisualStyleBackColor = true;
            this.btnExcute.Visible = false;
            this.btnExcute.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // lbResult
            // 
            this.lbResult.FormattingEnabled = true;
            this.lbResult.ItemHeight = 12;
            this.lbResult.Location = new System.Drawing.Point(12, 60);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(776, 376);
            this.lbResult.TabIndex = 1;
            // 
            // btnGetFolder
            // 
            this.btnGetFolder.Location = new System.Drawing.Point(551, 24);
            this.btnGetFolder.Name = "btnGetFolder";
            this.btnGetFolder.Size = new System.Drawing.Size(75, 23);
            this.btnGetFolder.TabIndex = 2;
            this.btnGetFolder.Text = "瀏覽";
            this.btnGetFolder.UseVisualStyleBackColor = true;
            this.btnGetFolder.Click += new System.EventHandler(this.btnGetFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Enabled = false;
            this.txtFolder.Location = new System.Drawing.Point(12, 24);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(533, 22);
            this.txtFolder.TabIndex = 3;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(713, 24);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "匯出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnGetFolder);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.btnExcute);
            this.Name = "fmMain";
            this.Text = "FaceLivenessTester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExcute;
        private System.Windows.Forms.ListBox lbResult;
        private System.Windows.Forms.Button btnGetFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.FolderBrowserDialog openFD;
        private System.Windows.Forms.Button btnExport;
    }
}

