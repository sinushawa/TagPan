using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TagPan
{
    public class MaxEditableTreeView : TreeView
    {
        public MaxEditableTreeView()
        {
        }

        private const int TVM_GETEDITCONTROL = 0x110F;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd,
        int msg, int wParam, int lParam);

        private class LabelEditWindowHook : NativeWindow
        {
            private const int WM_GETDLGCODE = 135;
            private const int DLGC_WANTALLKEYS = 0x0004;

            public LabelEditWindowHook()
            {
            }
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_GETDLGCODE)
                    m.Result = (IntPtr)DLGC_WANTALLKEYS;
                else
                    base.WndProc(ref m);
            }
        }

        private LabelEditWindowHook m_Hook = new LabelEditWindowHook();

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            base.OnBeforeLabelEdit(e);
            if (!e.CancelEdit)
            {
                IntPtr handle = SendMessage(this.Handle, TVM_GETEDITCONTROL, 0, 0);
                if (handle != IntPtr.Zero)
                    m_Hook.AssignHandle(handle);
            }
        }
    }
}
