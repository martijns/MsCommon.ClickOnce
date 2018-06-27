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
    public partial class SelectableMessageBox : AppForm
    {
        public SelectableMessageBox()
        {
            InitializeComponent();
            //tbMessage.Text = string.Join("\r\n", _collectedData.Keys.Cast<string>().Select(k => k + ": " + _collectedData[k]).ToArray());
            AcceptButton = btnYes;
            CancelButton = btnYes;
        }

        private void HandleFormLoad(object sender, EventArgs e)
        {
            BringToFront();
            btnYes.Focus();
        }

        private void HandleYesClicked(object sender, EventArgs e)
        {
            Close();
        }

        public static DialogResult Show(Form owner, string text, string caption = "Message")
        {
            var box = new SelectableMessageBox
            {
                tbMessage = {Text = text},
                Text = caption,
                Owner = owner
            };
            box.CenterToParent();
            return box.ShowDialog(owner);
        }
    }
}
