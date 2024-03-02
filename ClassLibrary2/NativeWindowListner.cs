using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32;

namespace NotificationFilter
{
    public class NativeWindowListener : NativeWindow
    {
        public bool HandleSet { get; set; }
        public event EventHandler? Activating;
        public event EventHandler? Deactivating;

        public void SetHandle(nint handle)
        {
            if (nint.Zero == handle || HandleSet) return;

            HandleSet = true;
            AssignHandle(handle);
        }

        //// Listen for the control's window creation and then hook into it.
        //internal void OnHandleCreated(object sender, EventArgs e)
        //{
        //    // Window is now created, assign handle to NativeWindow.
        //    AssignHandle(((Form1)sender).Handle);
        //}
        //internal void OnHandleDestroyed(object sender, EventArgs e)
        //{
        //    // Window was destroyed, release hook.
        //    ReleaseHandle();
        //}

        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages            
            switch ((uint)m.Msg)
            {
                case PInvoke.WM_ACTIVATE:

                    // Notify the form that this message was received.
                    // Application is activated or deactivated, 
                    // based upon the WParam parameter.
                    if(m.WParam != 0)
                        Activating?.Invoke(this, new EventArgs());
                    else
                        Deactivating?.Invoke(this, new EventArgs());
                    break;
            }
            base.WndProc(ref m);
        }
    }
}
