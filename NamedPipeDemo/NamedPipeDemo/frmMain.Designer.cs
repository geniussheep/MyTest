﻿namespace NamedPipeDemo
{
    partial class frmMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lsbMsg = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lsbMsg
            // 
            this.lsbMsg.FormattingEnabled = true;
            this.lsbMsg.ItemHeight = 12;
            this.lsbMsg.Location = new System.Drawing.Point(12, 12);
            this.lsbMsg.Name = "lsbMsg";
            this.lsbMsg.Size = new System.Drawing.Size(260, 208);
            this.lsbMsg.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 233);
            this.Controls.Add(this.lsbMsg);
            this.Name = "frmMain";
            this.Text = "命名管道服务端";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsbMsg;
    }
}

