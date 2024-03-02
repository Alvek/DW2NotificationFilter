using System;
using System.Drawing;
using System.Windows.Forms;

namespace NotificationFilter
{
    public partial class Form1 : Form
    {
        public event EventHandler? Hiden;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            ChangeControlColors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Location = new Point(Location.X + 50, Location.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Location = new Point(Location.X, Location.Y + 50);

        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Location = new Point(Location.X - 50, Location.Y);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Location = new Point(Location.X, Location.Y - 50);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (this.Hiden != null)
            {
                this.Hiden(this, null);
            }
        }

        private void ChangeControlColors()
        {
            foreach (var item in this.Controls)
            {
                if (item is Button)
                {
                    Button btn = (Button)item;
                    btn.BackColor = Color.FromArgb(255,69, 69, 69);
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
    }
}
