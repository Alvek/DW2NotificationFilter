namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            //SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            //this.BackColor = Color.Transparent;
            //this.TransparencyKey = Color.Transparent;
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

    }
}
