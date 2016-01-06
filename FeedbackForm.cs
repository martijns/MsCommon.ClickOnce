using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MsCommon.ClickOnce
{
    public partial class FeedbackForm : AppForm
    {
        public static string FeedbackEndpoint { get; set; }

        private string _mood;

        public FeedbackForm()
        {
            InitializeComponent();
            this.AcceptButton = btnSubmit;
            this.CancelButton = btnCancel;
            if (string.IsNullOrEmpty(FeedbackEndpoint))
                throw new ApplicationException("Programmers error: FeedbackEndpoint is not set");
        }

        private void HandleFormLoad(object sender, EventArgs e)
        {
            UpdateStatus(Color.Black, "");
            BringToFront();
            btnSubmit.Focus();
        }

        private void ButtonState(bool enabled)
        {
            btnSubmit.Enabled = enabled;
            btnCancel.Enabled = enabled;
            tbMessage.ReadOnly = !enabled;
            rbHappy.Enabled = enabled;
            rbSad.Enabled = enabled;
        }

        private async void HandleSubmitClicked(object sender, EventArgs e)
        {
            ButtonState(false);
            var data = new NameValueCollection();
            data["appname"] = AppVersion.AppName;
            data["appversion"] = AppVersion.GetVersion();
            data["mood"] = _mood ?? "no mood";
            data["message"] = tbMessage.Text ?? "no message";
            UpdateStatus(Color.Blue, "Submitting...");
            using (var client = new WebClient())
            {
                try
                {
                    byte[] response = await client.UploadValuesTaskAsync(FeedbackEndpoint, "POST", data);
                    string responseStr = Encoding.ASCII.GetString(response);
                    if (responseStr.Equals("ok"))
                    {
                        UpdateStatus(Color.Green, "Submitted, thank you!");
                    }
                    else
                    {
                        UpdateStatus(Color.Red, "Oops: " + responseStr);
                        ButtonState(true);
                    }
                }
                catch (Exception uploadex)
                {
                    UpdateStatus(Color.Red, "Failed to submit: " + uploadex.Message);
                    ButtonState(true);
                }
            }
        }

        private void HandleCancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateStatus(Color c, string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke((Action<Color, string>)UpdateStatus, c, status);
                return;
            }
            lblStatus.Font = new Font(Label.DefaultFont, FontStyle.Bold);
            lblStatus.ForeColor = c;
            lblStatus.Text = status;
        }

        private void HandleMoodClicked(object sender, EventArgs e)
        {
            if (tbMessage.ReadOnly)
                return;

            pbHappy.BackColor = Control.DefaultBackColor;
            pbHappy.BorderStyle = BorderStyle.None;
            rbHappy.Checked = false;
            pbSad.BackColor = Control.DefaultBackColor;
            pbSad.BorderStyle = BorderStyle.None;
            rbSad.Checked = false;

            if (sender == pbHappy || sender == rbHappy)
            {
                pbHappy.BorderStyle = BorderStyle.FixedSingle;
                pbHappy.BackColor = Color.LightGreen;
                rbHappy.Checked = true;
                _mood = "Happy";
            }

            if (sender == pbSad || sender == rbSad)
            {
                pbSad.BorderStyle = BorderStyle.FixedSingle;
                pbSad.BackColor = Color.LightGreen;
                rbSad.Checked = true;
                _mood = "Sad";
            }
        }
    }
}
