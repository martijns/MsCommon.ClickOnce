using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MsCommon.ClickOnce.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsCommon.ClickOnce
{
    public class AppForm : Form, IAppender
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppForm));

        private System.ComponentModel.IContainer components;
        private ContextMenuStrip lbLogContextMenuStrip;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ListBox lbLog;

        public AppForm()
        {
            InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(GetType());
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            ((Hierarchy)LogManager.GetRepository()).Root.AddAppender(this);
            this.Load += HandleAppFormLoad;
        }

        private void HandleAppFormLoad(object sender, EventArgs e)
        {
            BringToFront();
        }

        #region PerformWork & PerformWorkAsync

        public void PerformWork(Action bgwork, Action fgwork, Action<Exception> customErrorHandler = null)
        {
            this.SetBusy(true);
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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task PerformWorkAsync(Func<Task> bgwork, Func<Task> fgwork, Action<Exception> customErrorHandler = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
                Delegate method = (Func<Task>)(async () =>
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
                });
                if (InvokeRequired)
                    Invoke(method);
                else
                    method.DynamicInvoke();
            });
        }

        #endregion

        #region Logging

        private string _LastLogMessage = null;
        private int _LastLogMessageCount = 0;

        protected void SetLoggingListBox(ListBox loggingListBox)
        {
            lbLog = loggingListBox;
            if (lbLog == null)
                return;

            lbLog.DrawMode = DrawMode.OwnerDrawFixed;
            lbLog.DrawItem += HandleDrawLogItem;
            lbLog.ContextMenuStrip = lbLogContextMenuStrip;
        }

        private void AddToLog(LogItem logitem)
        {
            if (lbLog == null)
                return;

            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke((Action<LogItem>)AddToLog, logitem);
                return;
            }

            if (_LastLogMessage != logitem.Message)
            {
                _LastLogMessage = logitem.Message;
                _LastLogMessageCount = 1;
            }
            else
            {
                _LastLogMessageCount++;
            }

            if (_LastLogMessageCount > 1)
            {
                logitem.Message += " (message repeated " + _LastLogMessageCount + " times)";
                lbLog.Items.RemoveAt(0);
            }

            lbLog.Items.Insert(0, logitem);
            while (lbLog.Items.Count > 250)
            {
                lbLog.Items.RemoveAt(lbLog.Items.Count - 1);
            }
        }

        private void HandleDrawLogItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (e.Index < 0 || e.Index >= lb.Items.Count)
                return;

            Color backColor = lb.BackColor;
            LogItem li = lb.Items[e.Index] as LogItem;
            if (li != null)
            {
                if (li.Level == Level.Warn)
                    backColor = Color.Yellow;
                if (li.Level == Level.Error)
                    backColor = Color.LightPink;
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = ChangeColorBrightness(backColor, -0.1f);
            }

            Graphics g = e.Graphics;

            string msg = lb.Items[e.Index].ToString();

            int hzSize = (int)g.MeasureString(msg, lb.Font).Width;
            if (lb.HorizontalExtent < hzSize)
                lb.HorizontalExtent = hzSize;

            e.DrawBackground();
            g.FillRectangle(new SolidBrush(backColor), e.Bounds);
            g.DrawString(msg, e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// http://www.pvladov.com/2012/09/make-color-lighter-or-darker.html
        /// </summary>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level >= Level.Info)
            {
                AddToLog(new LogItem { TimeStamp = loggingEvent.TimeStamp, Level = loggingEvent.Level, Message = loggingEvent.RenderedMessage });
            }
        }

        public class LogItem
        {
            public DateTime TimeStamp { get; set; }
            public Level Level { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return string.Format("{0}: {1}", TimeStamp.ToString("HH:mm:ss"), Message);
            }

            public string ToStringWithLevel()
            {
                return string.Format("{0} [{2}] {1}", TimeStamp.ToString("HH:mm:ss"), Message, Level.ToString());
            }
        }

        private void HandleLogCopyClicked(object sender, EventArgs e)
        {
            LogMethodEntry();

            if (lbLog == null)
                return;

            if (lbLog.SelectedItems.Count == 0)
                return;

            string[] selectedLog = lbLog.SelectedItems.Cast<LogItem>().Select(li => li.ToStringWithLevel()).ToArray();
            Clipboard.SetText(string.Join("\r\n", selectedLog));
        }

        private void HandleLogClearClicked(object sender, EventArgs e)
        {
            if (lbLog == null)
                return;

            lbLog.Items.Clear();
        }

        protected void LogMethodEntry([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            Logger.DebugFormat("=> {0}.{1}:{2}", Path.GetFileNameWithoutExtension(file), member, line);
        }

        #endregion

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbLogContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbLogContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLogContextMenuStrip
            // 
            this.lbLogContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.clearToolStripMenuItem});
            this.lbLogContextMenuStrip.Name = "lbLogContextMenuStrip";
            this.lbLogContextMenuStrip.Size = new System.Drawing.Size(153, 76);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.HandleLogCopyClicked);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.HandleLogClearClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // AppForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "AppForm";
            this.lbLogContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
