namespace Labda
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();
        DrawArea rajz;
        DrawArea rajz2;
        private Point CursorPoint;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            DateTime d = DateTime.Now;
            rajz.Repaint();
            //rajz2.Repaint(pictureBox1);
            //label1.Text = (1000 / (DateTime.Now - d).TotalMilliseconds).ToString("0.0") + "FPS";
            timer1.Start();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //525x285
            panel6.BackgroundImage = Image.FromFile(@"C:\Users\3263783\Downloads\billiard_table5.jpg");
            panel6.BackgroundImageLayout = ImageLayout.Stretch;
            int meret = 15;
            int meretpow = meret * 2;
            Point start1 = new(pictureBox2.ClientSize.Width / 2, pictureBox2.ClientSize.Height / 2);
            Point start2 = new(pictureBox2.ClientSize.Width / 4 * 3, pictureBox2.ClientSize.Height / 2);
            int two = start2.X - meret * 2;
            int four = start2.X + meret * 2;
            int five = start2.X + meret * 4;
            int yplusheight = start2.Y + meret * 2;
            int yminusheight = start2.Y - meret * 2;
            Ball ball1 = new(new Point(start2.X - meret * 4, start2.Y), false, 1, meret);
            Ball ball2 = new(new Point(two, start2.Y - meretpow / 2), true, 2, meret);
            Ball ball3 = new(new Point(four, start2.Y + meretpow / 2), true, 3, meret);
            Ball ball4 = new(new Point(five, start2.Y), true, 4, meret);
            Ball ball5 = new(new Point(start2.X, yminusheight - 1), true, 5, meret);
            Ball ball7 = new(new Point(five, start2.Y + meret * 4), true, 7, meret);
            Ball ball6 = new(new Point(four, yminusheight - meretpow / 2), true, 6, meret);
            Ball ball8 = new(start2, true, 8, meret);
            Ball ball9 = new(new Point(five, yminusheight), true, 9, meret);
            Ball ball10 = new(new Point(two, start2.Y + meretpow / 2), true, 10, meret);
            Ball ball11 = new(new Point(five, start2.Y - meret * 4), true, 11, meret);
            Ball ball12 = new(new Point(four, yplusheight + meretpow / 2), true, 12, meret);
            Ball ball13 = new(new Point(four, start2.Y - meretpow / 2), true, 13, meret);
            Ball ball14 = new(new Point(start2.X, yplusheight + 1), true, 14, meret);
            Ball ball15 = new(new Point(five, yplusheight), true, 15, meret);
            Ball feher = new(start1, 0, 0, Color.WhiteSmoke, false, meret);

            rajz = new(new List<Ball>() { feher, ball1/*, ball2, ball3, ball4, ball5, ball6, ball7, ball8, ball9, ball10, ball11, ball12, ball13, ball14, ball15 */}, 1, 1,pictureBox2,label1);
            //rajz2 = new(new List<Ball>() { feher, ball1, ball2 }, new Size(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height), 1, 1);
        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            //label2.Text = ClientSize.Width + "," + ClientSize.Height;
        }
        private void pictureBox2_MouseMove_1(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //    label1.Text = e.Location.X + "," + e.Location.Y;
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //CursorPoint = new Point(e.Location.X, e.Location.Y);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int originalPanelwidth = 50;
            int originalPanelheight = 70;
            int originalPicBoxwidth = 640;
            int originalPicBoxheight = 365;
            panel9.Height = panel10.Height = (int)1.0 * panel6.Height / originalPicBoxheight * originalPanelheight;
            panel7.Width = panel8.Width = (int)1.0 * panel6.Width / originalPicBoxwidth * originalPanelwidth;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            Form1_Resize(null, null);
        }
    }
}