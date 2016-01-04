namespace MsCommon.ClickOnce
{
    partial class FeedbackForm
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCustomMessage = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.pbHappy = new System.Windows.Forms.PictureBox();
            this.pbSad = new System.Windows.Forms.PictureBox();
            this.rbHappy = new System.Windows.Forms.RadioButton();
            this.rbSad = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbHappy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSad)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(24, 253);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(232, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit my feedback!";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.HandleSubmitClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(315, 253);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.HandleCancelClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(24, 289);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(386, 23);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "status label...";
            // 
            // lblCustomMessage
            // 
            this.lblCustomMessage.AutoSize = true;
            this.lblCustomMessage.Location = new System.Drawing.Point(21, 122);
            this.lblCustomMessage.Name = "lblCustomMessage";
            this.lblCustomMessage.Size = new System.Drawing.Size(205, 13);
            this.lblCustomMessage.TabIndex = 5;
            this.lblCustomMessage.Text = "What would you like to tell the developer?";
            // 
            // tbMessage
            // 
            this.tbMessage.AcceptsReturn = true;
            this.tbMessage.Location = new System.Drawing.Point(24, 138);
            this.tbMessage.MaxLength = 2000;
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMessage.Size = new System.Drawing.Size(386, 98);
            this.tbMessage.TabIndex = 6;
            // 
            // pbHappy
            // 
            this.pbHappy.Image = global::MsCommon.ClickOnce.Properties.Resources.smiley_icon;
            this.pbHappy.Location = new System.Drawing.Point(124, 12);
            this.pbHappy.Name = "pbHappy";
            this.pbHappy.Size = new System.Drawing.Size(64, 64);
            this.pbHappy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbHappy.TabIndex = 7;
            this.pbHappy.TabStop = false;
            this.pbHappy.Click += new System.EventHandler(this.HandleMoodClicked);
            // 
            // pbSad
            // 
            this.pbSad.Image = global::MsCommon.ClickOnce.Properties.Resources.smiley_sad_icon;
            this.pbSad.Location = new System.Drawing.Point(243, 12);
            this.pbSad.Name = "pbSad";
            this.pbSad.Size = new System.Drawing.Size(64, 64);
            this.pbSad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbSad.TabIndex = 8;
            this.pbSad.TabStop = false;
            this.pbSad.Click += new System.EventHandler(this.HandleMoodClicked);
            // 
            // rbHappy
            // 
            this.rbHappy.AutoSize = true;
            this.rbHappy.Location = new System.Drawing.Point(149, 83);
            this.rbHappy.Name = "rbHappy";
            this.rbHappy.Size = new System.Drawing.Size(14, 13);
            this.rbHappy.TabIndex = 9;
            this.rbHappy.TabStop = true;
            this.rbHappy.UseVisualStyleBackColor = true;
            this.rbHappy.Click += new System.EventHandler(this.HandleMoodClicked);
            // 
            // rbSad
            // 
            this.rbSad.AutoSize = true;
            this.rbSad.Location = new System.Drawing.Point(268, 82);
            this.rbSad.Name = "rbSad";
            this.rbSad.Size = new System.Drawing.Size(14, 13);
            this.rbSad.TabIndex = 10;
            this.rbSad.TabStop = true;
            this.rbSad.UseVisualStyleBackColor = true;
            this.rbSad.Click += new System.EventHandler(this.HandleMoodClicked);
            // 
            // FeedbackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 314);
            this.Controls.Add(this.rbSad);
            this.Controls.Add(this.rbHappy);
            this.Controls.Add(this.pbSad);
            this.Controls.Add(this.pbHappy);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.lblCustomMessage);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeedbackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Submit feedback";
            this.Load += new System.EventHandler(this.HandleFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pbHappy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCustomMessage;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.PictureBox pbHappy;
        private System.Windows.Forms.PictureBox pbSad;
        private System.Windows.Forms.RadioButton rbHappy;
        private System.Windows.Forms.RadioButton rbSad;
    }
}