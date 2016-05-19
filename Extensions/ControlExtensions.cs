using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsCommon.ClickOnce.Extensions
{
    public static class ControlExtensions
    {
        #region Busy & ControlState

        private static ConcurrentDictionary<Control, ConcurrentDictionary<Control, bool>> controlEnabledState = new ConcurrentDictionary<Control, ConcurrentDictionary<Control, bool>>();
        private static ConcurrentDictionary<Control, bool> controlBusyState = new ConcurrentDictionary<Control, bool>();
        private static ConcurrentDictionary<Control, int> controlBusyCount = new ConcurrentDictionary<Control, int>();

        private static T Get<T>(ConcurrentDictionary<Control, T> dict, Control control, T defaultValue)
        {
            if (!dict.ContainsKey(control))
                dict.TryAdd(control, defaultValue);
            return dict[control];
        }

        private static void Set<T>(ConcurrentDictionary<Control, T> dict, Control control, T value)
        {
            dict[control] = value;
        }

        private static bool IsBusyInternal(Control c)
        {
            return Get(controlBusyState, c, false);
        }

        private static void SetBusyInternal(Control c, bool busy)
        {
            Set(controlBusyState, c, busy);
        }

        private static int GetBusyCountInternal(Control c)
        {
            return Get(controlBusyCount, c, 0);
        }

        private static void IncrementBusyCountInternal(Control c)
        {
            Set(controlBusyCount, c, Get(controlBusyCount, c, 0) + 1);
        }

        private static void DecrementBusyCountInternal(Control c)
        {
            Set(controlBusyCount, c, Get(controlBusyCount, c, 1) - 1); // (default 1, so we end up at 0 if unset)
        }


        private static ConcurrentDictionary<Control, bool> GetControlDictionary(Control p)
        {
            return Get(controlEnabledState, p, new ConcurrentDictionary<Control, bool>());
        }

        private static bool HasControlStateInternal(Control p, Control c)
        {
            return GetControlDictionary(p).ContainsKey(c);
        }

        private static bool GetControlStateInternal(Control p, Control c)
        {
            return Get(GetControlDictionary(p), c, c.Enabled);
        }

        private static void SetControlStateInternal(Control p, Control c, bool enabled)
        {
            Set(GetControlDictionary(p), c, enabled);
        }


        public static bool IsBusy(this Control c)
        {
            return IsBusyInternal(c);
        }

        public static void SetBusy(this Control c, bool busy)
        {
            if (busy)
                IncrementBusyCountInternal(c);
            else
                DecrementBusyCountInternal(c);

            if (IsBusyInternal(c) && GetBusyCountInternal(c) == 0)
                DisableBusy(c);
            else if (!IsBusyInternal(c) && GetBusyCountInternal(c) == 1)
                EnableBusy(c);
        }

        public static void EnableBusy(this Control c)
        {
            SetBusyInternal(c, true);
            GetControlDictionary(c).Clear();
            RecursiveDisable(c, c);
            Cursor.Current = Cursors.WaitCursor;
        }

        private static void RecursiveDisable(Control p, Control c)
        {
            if (c.Tag is string && (string)c.Tag == "KEEP_ENABLED_NO_PROPAGATE")
                return;

            foreach (Control inner in c.Controls)
                RecursiveDisable(p, inner);
            SaveControlStateAndDisable(p, c);
        }

        public static void DisableBusy(this Control c)
        {
            SetBusyInternal(c, false);
            RecursiveRestore(c, c);
            Cursor.Current = Cursors.Default;
        }

        private static void RecursiveRestore(Control p, Control c)
        {
            foreach (Control inner in c.Controls)
                RecursiveRestore(p, inner);
            RestoreControlState(p, c);

            // Controls could be removed in the meantime. To prevent odd behaviour, we explicitely restore the state of each recorded control
            foreach (Control control in GetControlDictionary(p).Keys)
            {
                RestoreControlState(p, control);
            }
        }

        public static void UpdateControlEnabledState(this Control control, Control child, bool enabled)
        {
            if (IsBusyInternal(control))
            {
                foreach (var key in controlEnabledState.Keys)
                    SetControlStateInternal(key, child, enabled);
            }
            else
            {
                child.Enabled = enabled;
            }
        }


        private static void SaveControlStateAndDisable(Control parent, Control control)
        {
            if (control is Form)
                return;
            if (control.Parent != null && !control.Parent.Enabled) // No need to store disable if the parent is already disabled
                return;
            SetControlStateInternal(parent, control, control.Enabled);
            if (control.Tag is string && (string)control.Tag == "KEEP_ENABLED")
            {
                SetControlStateInternal(parent, control, true);
                return;
            }
            control.Enabled = false;
            if (control is DataGridView)
            {
                var dgv = (DataGridView)control;
                dgv.ForeColor = Color.Gray;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private static void RestoreControlState(Control parent, Control control)
        {
            if (control is Form)
                return;
            if (HasControlStateInternal(parent, control))
            {
                control.Enabled = GetControlStateInternal(parent, control);
                if (control is DataGridView)
                {
                    var dgv = (DataGridView)control;
                    dgv.ForeColor = Control.DefaultForeColor;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Control.DefaultForeColor;
                    foreach (Control scrollbar in dgv.Controls)
                    {
                        scrollbar.Enabled = control.Enabled; // Enable if parent is enabled
                    }
                }
            }
        }

        #endregion

        #region Suspend & Resume Draw

        private const int WM_SETREDRAW = 0x000B;

        public static void SuspendDraw(this Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        public static void ResumeDraw(this Control control)
        {
            // Create a C "true" boolean as an IntPtr
            IntPtr wparam = new IntPtr(1);
            Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam, IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);

            control.Invalidate();
        }

        #endregion
    }
}
