using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NotificationFilter
{
    public partial class fFilterSettings : Form
    {
        private nint _gameWindowHandle;

        public event EventHandler? Hiden;

        public fFilterSettings(nint gameWindowHandle)
        {
            InitializeComponent();

            this.TopMost = true;
            ChangeControlColors();
            _gameWindowHandle = gameWindowHandle;

            Rules t = new Rules();
            BaseRule rule = new BaseRule();
            rule.DefaultAction = DefaultMessageAction.Yes;
            rule.MessageType = DistantWorlds.Types.EmpireMessageType.Research;
            rule.ObjectRules.Add(new() { RelatedObjectType = DistantWorlds.Types.ItemType.FleetTemplate });
            rule.ItemRules.Add(new() { Id = 1, RelatedItemType = DistantWorlds.Types.ItemType.Artifact });
            t.RuleList.Add(rule);


            TypeDescriptor.AddProvider(new MyClassTypeDescProvider<BaseRule>(), typeof(Rules));
            TypeDescriptor.AddProvider(new MyClassTypeDescProvider<RelatedItemRules>(), typeof(BaseRule));
            TypeDescriptor.AddProvider(new MyClassTypeDescProvider<RelatedObjectRules>(), typeof(BaseRule));
            bindingSource1.DataSource = t;

            dataGridView1.DataSource = bindingSource1;
        }

        private void ChangeControlColors()
        {
            foreach (var item in this.Controls)
            {
                if (item is Button)
                {
                    Button btn = (Button)item;
                    btn.BackColor = Color.FromArgb(255, 69, 69, 69);
                    btn.ForeColor = Color.FromArgb(255, 247, 247, 247);
                }
            }
        }

        /// <summary>
        /// Leave empty to make transparent
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void fFilterSettings_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            { this.Hiden?.Invoke(this, null); }
        }

        /// <summary>
        /// Hide borderless form from Alt+Tab
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages            
            switch ((uint)m.Msg)
            {
                case Windows.Win32.PInvoke.WM_ACTIVATE:

                    // Notify the form that this message was received.
                    // Application is activated or deactivated, 
                    // based upon the WParam parameter.
                    if (m.WParam == 0 && m.LParam != _gameWindowHandle)
                    {
                        this.Hide();
                    }
                    break;
            }
            base.WndProc(ref m);
        }
    }
}
