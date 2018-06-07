namespace AdamTool
{
    partial class AdamToolForm
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
            this.btn_GetConfigDifference = new System.Windows.Forms.Button();
            this.cbx = new System.Windows.Forms.ComboBox();
            this.lbl_Environment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_GetConfigDifference
            // 
            this.btn_GetConfigDifference.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_GetConfigDifference.Location = new System.Drawing.Point(12, 38);
            this.btn_GetConfigDifference.Name = "btn_GetConfigDifference";
            this.btn_GetConfigDifference.Size = new System.Drawing.Size(173, 51);
            this.btn_GetConfigDifference.TabIndex = 0;
            this.btn_GetConfigDifference.Text = "获取新老配置差异";
            this.btn_GetConfigDifference.UseVisualStyleBackColor = true;
            // 
            // cbx
            // 
            this.cbx.Font = new System.Drawing.Font("宋体", 9F);
            this.cbx.FormattingEnabled = true;
            this.cbx.Location = new System.Drawing.Point(158, 218);
            this.cbx.Name = "cbx";
            this.cbx.Size = new System.Drawing.Size(250, 20);
            this.cbx.TabIndex = 1;
            // 
            // lbl_Environment
            // 
            this.lbl_Environment.AutoSize = true;
            this.lbl_Environment.Location = new System.Drawing.Point(12, 221);
            this.lbl_Environment.Name = "lbl_Environment";
            this.lbl_Environment.Size = new System.Drawing.Size(41, 12);
            this.lbl_Environment.TabIndex = 2;
            this.lbl_Environment.Text = "label1";
            // 
            // AdamToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 302);
            this.Controls.Add(this.lbl_Environment);
            this.Controls.Add(this.cbx);
            this.Controls.Add(this.btn_GetConfigDifference);
            this.Name = "AdamToolForm";
            this.Text = "Adam工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_GetConfigDifference;
        private System.Windows.Forms.ComboBox cbx;
        private System.Windows.Forms.Label lbl_Environment;
    }
}

