using System;
using System.Windows.Forms;

namespace MsCommon.ClickOnce
{
    public class AppProgram
    {
        public static void Start(
            string applicationName,
            string authorName,
            string reportBugEndpoint,
            Action<string[]> mainMethod,
            string[] args)
        {
            try
            {
                // Set some statics
                AppVersion.AppName = applicationName;
                AppVersion.AuthorName = authorName;
                ReportBugForm.ReportBugEndpoint = reportBugEndpoint;

                // If this application is started the ClickOnce-way, arguments are passed in a different way
                try
                {
                    if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null)
                    {
                        args = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
                    }
                }
                catch (Exception ex)
                {
                    // Don't bother with handling
                }

                // Catch unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

                // Run the main method
                mainMethod(args);
            }
            catch (Exception ex)
            {
                var bugform = new ReportBugForm(ex);
                Application.Run(bugform);
            }
        }

        internal static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex == null)
                return;

            try
            {
                if (Application.MessageLoop)
                {
                    new ReportBugForm(ex).ShowDialog();
                }
                else
                {
                    var bugform = new ReportBugForm(ex);
                    Application.Run(bugform);
                }
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Proper error handling failed (" + ex2.Message + "). Please show the following message to the developer:\r\n\r\n" + ex, "Whoops!");
            }
        }
    }
}
