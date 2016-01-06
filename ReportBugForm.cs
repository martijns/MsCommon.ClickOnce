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
    public partial class ReportBugForm : AppForm
    {
        public static string ReportBugEndpoint { get; set; }

        private NameValueCollection _collectedData;

        public ReportBugForm(Exception ex)
        {
            InitializeComponent();
            _collectedData = CollectData(ex);
            tbReport.Text = string.Join("\r\n", _collectedData.Keys.Cast<string>().Select(k => k + ": " + _collectedData[k]).ToArray());
            this.AcceptButton = btnYes;
            this.CancelButton = btnNo;

            if (Debugger.IsAttached)
                Debugger.Break();
        }

        private void HandleFormLoad(object sender, EventArgs e)
        {
            UpdateStatus(Color.Black, "");
            BringToFront();
            btnYes.Focus();
        }

        private void ButtonState(bool enabled)
        {
            btnYes.Enabled = enabled;
            btnNo.Enabled = enabled;
            tbCustomMessage.Enabled = enabled;
        }

        private async void HandleYesClicked(object sender, EventArgs e)
        {
            ButtonState(false);
            _collectedData["custommessage"] = tbCustomMessage.Text;
            UpdateStatus(Color.Blue, "Submitting...");
            using (var client = new WebClient())
            {
                try
                {
                    byte[] response = await client.UploadValuesTaskAsync(ReportBugEndpoint, "POST", _collectedData);
                    string responseStr = Encoding.ASCII.GetString(response);
                    if (responseStr.Equals("ok"))
                    {
                        UpdateStatus(Color.Green, "Submitted, thank you!");
                    }
                    else
                    {
                        UpdateStatus(Color.Red, "Server error: " + responseStr);
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

        private void HandleNoClicked(object sender, EventArgs e)
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

        protected NameValueCollection CollectData(Exception ex)
        {
            var data = new NameValueCollection();
            data["appname"] = AppVersion.AppName;
            data["appversion"] = AppVersion.GetVersion();
            data["errormessage"] = ex.Message ?? "no exception message";
            data["machinename"] = Environment.MachineName;
            data["osversion"] = Environment.OSVersion.VersionString;
            data["os64bit"] = Environment.Is64BitOperatingSystem.ToString();
            data["processorcount"] = Environment.ProcessorCount.ToString();
            data["username"] = Environment.UserName;
            data["runtimeversion"] = Environment.Version.ToString();
            data["workingsetbytes"] = Environment.WorkingSet.ToString();
            data["runningfor"] = (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString("d\\ hh\\:mm\\:ss");
            data["threadcount"] = Process.GetCurrentProcess().Threads.Count.ToString();
            data["fullexception"] = ex.ToString();
            return data;
        }
    }
}
