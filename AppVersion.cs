using log4net;
using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationDeployment = System.Deployment.Application.ApplicationDeployment;

namespace MsCommon.ClickOnce
{
    public static class AppVersion
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppVersion));

        public static string AuthorName { get; set; }

        public static string AppName { get; set; }


        public static string GetVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                return ToVersionString(ApplicationDeployment.CurrentDeployment.CurrentVersion);
            }
            return "0.0.0.0";
        }

        public static async Task CheckForUpdateAsync()
        {
            await Task.Run(() =>
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    Logger.Info("Checking for updates...");
                    try
                    {
                        var updateInfo = ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate();
                        HandleCheckForUpdateCompleted(updateInfo);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Failed checking for updates: " + ex.GetType().Name + ": " + ex.Message);
                    }
                }
                else
                {
                    Logger.Warn("The application is running standalone instead of using ClickOnce. Automatic updates are therefore not available. Consider updating the shortcut you use to start the application!");
                }
            });
        }

        private static void HandleCheckForUpdateCompleted(UpdateCheckInfo e)
        {
            if (e.UpdateAvailable)
            {
                Logger.InfoFormat("An update is vailable ({0})...", ToVersionString(e.AvailableVersion));
                ApplicationDeployment.CurrentDeployment.Update();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("An update was found and installed. The application needs to be restarted to use the new version.");
                sb.AppendLine();
                sb.AppendLine("Current version: " + GetVersion());
                sb.AppendLine("New version: " + ToVersionString(e.AvailableVersion));
                sb.AppendLine("Update size: " + e.UpdateSizeBytes + " bytes");
                sb.AppendLine();
                sb.AppendLine("Do you want to restart the application to update now?");

                // Get a form so we can invoke on the thread
                Form form = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;
                if (form != null)
                {
                    form.Invoke((Action)(() => {
                        if (MessageBox.Show(form, sb.ToString(), "Update available!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            Application.Restart();
                    }));
                }
            }
            else
            {
                Logger.InfoFormat("No update available...");
            }
        }

        private static string ToVersionString(System.Version version)
        {
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public static void DisplayAbout()
        {
            MessageBox.Show(AppName + " v" + GetVersion() + " by " + AuthorName);
        }

        public static void DisplayChanges()
        {
            string changes = string.Empty;
            using (Stream s = Assembly.GetEntryAssembly().GetManifestResourceStream(Assembly.GetEntryAssembly().GetManifestResourceNames().First(e => e.ToLower().Contains("changes.txt"))))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    changes = sr.ReadToEnd();
                }
            }

            Button b = new Button();
            b.Text = "Close";
            b.Dock = DockStyle.Bottom;
            b.Click += (src, evt) =>
            {
                ((Form)b.Parent).Close();
            };

            TextBox tb = new TextBox();
            tb.Text = changes;
            tb.Dock = DockStyle.Fill;
            tb.ReadOnly = true;
            tb.Multiline = true;
            tb.Select(0, 0);
            tb.ScrollBars = ScrollBars.Both;
            tb.WordWrap = true;

            Form f = new Form();
            f.Size = new Size(400, 400);
            f.Text = "Changelog";
            f.Controls.Add(tb);
            f.Controls.Add(b);
            f.ShowDialog();
        }
    }
}
