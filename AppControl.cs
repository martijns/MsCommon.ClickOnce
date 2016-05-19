using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.IO;
using log4net;
using System.Threading;
using MsCommon.ClickOnce.Extensions;

namespace MsCommon.ClickOnce
{
    public partial class AppControl : UserControl
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppForm));

        public AppControl()
        {
            InitializeComponent();
        }

        protected void LogMethodEntry([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            Logger.DebugFormat("=> {0}.{1}:{2}", Path.GetFileNameWithoutExtension(file), member, line);
        }

        #region PerformWork & PerformWorkAsync

        public void PerformWork(Action bgwork, Action fgwork, Action<Exception> customErrorHandler = null)
        {
            new Thread(() =>
            {
                Exception exception = null;
                try
                {
                    if (bgwork != null)
                        bgwork();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Invoke((Action)(() =>
                {
                    if (exception != null)
                    {
                        if (customErrorHandler != null)
                            customErrorHandler(exception);
                        else
                            new ReportBugForm(exception).ShowDialog(this);
                        this.SetBusy(false);
                        return;
                    }
                    try
                    {
                        if (fgwork != null)
                            fgwork();
                    }
                    catch (Exception ex)
                    {
                        new ReportBugForm(ex).ShowDialog(this);
                    }
                    finally
                    {
                        this.SetBusy(false);
                    }
                }));
            }).Start();
        }

        public async Task PerformWorkAsyncOld(Func<Task> bgwork, Func<Task> fgwork, Action<Exception> customErrorHandler = null)
        {
            this.SetBusy(true);
            await Task.Factory.StartNew(async () =>
            {
                Exception exception = null;
                try
                {
                    if (bgwork != null)
                        await bgwork();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                Invoke((Func<Task>)(async () =>
                {
                    if (exception != null)
                    {
                        if (customErrorHandler != null)
                            customErrorHandler(exception);
                        else
                            new ReportBugForm(exception).ShowDialog(this);
                        this.SetBusy(false);
                        return;
                    }
                    try
                    {
                        if (fgwork != null)
                            await fgwork();
                    }
                    catch (Exception ex)
                    {
                        new ReportBugForm(ex).ShowDialog(this);
                    }
                    finally
                    {
                        this.SetBusy(false);
                    }
                }));
            });
        }

        public async Task PerformWorkAsync(Func<Task> bgwork, Func<Task> fgwork, Action<Exception> customErrorHandler = null)
        {
            this.SetBusy(true);
            try
            {

                Exception exception = null;
                try
                {
                    if (bgwork != null)
                    {
                        await Task.Factory.StartNew(async () =>
                        {
                            await bgwork();
                        });
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                if (exception != null)
                {
                    if (customErrorHandler != null)
                        customErrorHandler(exception);
                    else
                        new ReportBugForm(exception).ShowDialog(this);
                    return;
                }

                try
                {
                    if (fgwork != null)
                        await fgwork();
                }
                catch (Exception ex)
                {
                    new ReportBugForm(ex).ShowDialog(this);
                }
            }
            finally
            {
                this.SetBusy(false);
            }
        }

        #endregion

    }
}
