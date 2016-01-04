namespace MsCommon.ClickOnce
{
    partial class ReportBugForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbReport = new System.Windows.Forms.TextBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCustomMessage = new System.Windows.Forms.Label();
            this.tbCustomMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Yikes, an error occurred within the application. I\'d like to submit the following" +
    " report to the developer. Will you allow me?";
            // 
            // tbReport
            // 
            this.tbReport.Location = new System.Drawing.Point(19, 61);
            this.tbReport.Multiline = true;
            this.tbReport.Name = "tbReport";
            this.tbReport.ReadOnly = true;
            this.tbReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReport.Size = new System.Drawing.Size(386, 121);
            this.tbReport.TabIndex = 1;
            this.tbReport.TabStop = false;
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(19, 255);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(232, 23);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "Yes, send the report!";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.HandleYesClicked);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(305, 255);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(100, 23);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "No, thank you.";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.HandleNoClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(19, 291);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(386, 23);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "status label...";
            // 
            // lblCustomMessage
            // 
            this.lblCustomMessage.AutoSize = true;
            this.lblCustomMessage.Location = new System.Drawing.Point(16, 199);
            this.lblCustomMessage.Name = "lblCustomMessage";
            this.lblCustomMessage.Size = new System.Drawing.Size(265, 13);
            this.lblCustomMessage.TabIndex = 5;
            this.lblCustomMessage.Text = "Include a custom message (i.e. contextual information):";
            // 
            // tbCustomMessage
            // 
            this.tbCustomMessage.Location = new System.Drawing.Point(19, 218);
            this.tbCustomMessage.Name = "tbCustomMessage";
            this.tbCustomMessage.Size = new System.Drawing.Size(386, 20);
            this.tbCustomMessage.TabIndex = 6;
            // 
            // ReportBugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 315);
            this.Controls.Add(this.tbCustomMessage);
            this.Controls.Add(this.lblCustomMessage);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.tbReport);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportBugForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unexpected error!";
            this.Load += new System.EventHandler(this.HandleFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbReport;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCustomMessage;
        private System.Windows.Forms.TextBox tbCustomMessage;
    }
}