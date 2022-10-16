using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labda
{
    public class DrawArea
    {
        private readonly List<Ball> balls = new List<Ball>();
        private readonly Size size;
        public readonly double Fmultiplier = 0.8;
        public double AlfaMultiplier { get; set; }
        private readonly PictureBox PictureBox2;

        public bool IsBallsMoving = false;
        private Point Mousepoint;
        private bool Mouseleave;
        private Label labelbox;
        private bool downbutton = false;

        public DrawArea(List<Ball> balls, double fmultiplier, double alfaMultiplier, PictureBox picbox2, Label lbl)
        {
            this.balls = balls;
            this.size = picbox2.Size;
            Fmultiplier = fmultiplier;
            AlfaMultiplier = alfaMultiplier;
            PictureBox2 = picbox2;
            labelbox = lbl;
            PictureBox2.MouseClick += Picbox2_MouseClick;
            PictureBox2.MouseMove += PictureBox2_MouseMove;
            PictureBox2.MouseLeave += PictureBox2_MouseLeave;
            PictureBox2.MouseDown += PictureBox2_MouseDown;
        }
        Bitmap stick = new Bitmap(@"C:\Users\3263783\Downloads\bilStick3.png");
        public void Repaint()
        {
            Bitmap kep = new(size.Width, size.Height);
            Graphics g = Graphics.FromImage(kep);
            //g.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, kep.Width, kep.Height));
            //g.DrawRectangle(new Pen(Brushes.Black, 3), new Rectangle(0, 0, kep.Width, kep.Height));
            CollisionWithBall();
            foreach (var item in balls)
            {
                if (!Mouseleave && balls[0].IsMoving())
                {
                    GetCollisionWall(balls[0].Location, Mousepoint);
                    //DrawDirectionVector(balls[0], g);
                    //g.DrawLine(new Pen(Brushes.DarkRed, 3), Mousepoint, balls[0].Location);
                    //g.DrawEllipse(new Pen(Brushes.DarkRed, 3), new Rectangle(Mousepoint.X-15, Mousepoint.Y-15, 30, 30));
                    //g.DrawLine(new Pen(Brushes.Black, 4), 2 * balls[0].Location.X - Mousepoint.X, 2 * balls[0].Location.Y - Mousepoint.Y, balls[0].Location.X, balls[0].Location.Y);
                    //StickRotate(balls[0]);
                    g.DrawImage(stick, 2 * balls[0].Location.X - Mousepoint.X, 2 * balls[0].Location.Y - Mousepoint.Y, balls[0].Location.X, balls[0].Location.Y);

                }
                item.Paint(Fmultiplier, g, new Size(kep.Width, kep.Height), AlfaMultiplier);
            }
            g.Dispose();
            PictureBox2.Image = kep;
            GC.Collect();
        }

        private void StickRotate(Ball ball)
        {
            stick.RotateFlip(RotateFlipType.Rotate90FlipY);
        }

        private Point GetCollisionWall(Point ball, Point mouselocation)
        {
            BorderType wall1 = default;
            BorderType wall2 = default;
            if (Mousepoint.X - ball.X > 0)
                wall1 = BorderType.Right;
            else if (Mousepoint.X - ball.X < 0)
                wall2 = BorderType.Left;
            else if (Mousepoint.Y - ball.Y > 0)
                wall1 = BorderType.Bottom;
            else if (Mousepoint.Y - ball.Y < 0)
                wall2 = BorderType.Top;

            Point p1 = MetszoPont(ball, mouselocation, wall1);
            Point p2 = MetszoPont(ball, mouselocation, wall2);

            float dist1 = Distance(ball, p1);
            float dist2 = Distance(ball, p2);
            if (dist1 > dist2)
                return p1;
            else return p2;
        }
        private Point MetszoPont(Point ball, Point mouselocation, BorderType border)
        {
            Point topleft = new(0,0);
            Point topright = new(size.Width,0);
            Point bottomleft = new(0,size.Height);
            Point bottomright = new(size.Width,size.Height);
            Point wallLocation = new Point();
            switch (border)
            {
                case BorderType.Top:
                    {
                        
                        break;
                    }
                case BorderType.Left:
                    {
                        
                        break;
                    }
                case BorderType.Bottom:
                    {
                        
                        break;
                    }
                case BorderType.Right:
                    {
                        
                        break;
                    }
            }
            return wallLocation;
        }
        private int Distance(Point ball, Point wallLocation)
        {
            return (int)Math.Sqrt(Math.Pow(ball.X - wallLocation.X, 2)
                + Math.Pow(ball.Y - wallLocation.Y, 2));
        }
        private void DrawDirectionVector(Ball ball, Graphics g)
        {
            //falon pattanás után halványul a vonal
            //2 egyenes metszéspontja
            int wallx = Mousepoint.X;
            int wally = Mousepoint.Y;
            int k1 = wallx - ball.Location.X;
            int k2 = wally - ball.Location.Y;
            while (wallx > 0 && wallx < size.Width && wally > 0 && wally < size.Height)
            {
                wallx += k1;
                wally += k2;
            }
            g.DrawLine(new Pen(Brushes.DarkRed, 3), wallx, wally, balls[0].Location.X, balls[0].Location.Y);
            g.DrawEllipse(new Pen(Brushes.DarkRed, 3), new Rectangle(wallx - 15, wally - 15, 30, 30));
        }
        private void PictureBox2_MouseLeave(object? sender, EventArgs e) => Mouseleave = true;
        private void Picbox2_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                balls[0].F = Math.Sqrt(Math.Pow(e.X - balls[0].Location.X, 2) + Math.Pow(e.Y - balls[0].Location.Y, 2)) * 0.10;
                balls[0].Alfa = 90 - Math.Atan2(e.Y - balls[0].Location.Y, e.X - balls[0].Location.X) * (180 / Math.PI);
            }
        }
        private void PictureBox2_MouseMove(object? sender, MouseEventArgs e)
        {
            bool inball2 = balls[0].InBall(e.Location);
            if (inball2)
            {
                Mousepoint = Point.Empty;
                if (e.Button == MouseButtons.Left && downbutton)
                    balls[0].SetLocation(e.Location, size);
                labelbox.Text = "Belepett";
            }
            else if (!Mouseleave && !inball2)
            {
                labelbox.Text = "";
                Mousepoint = new(e.X, e.Y);
            }
        }
        private void PictureBox2_MouseDown(object? sender, MouseEventArgs e) => downbutton = true;
        public void CollisionWithBall()
        {
            for (int i = 0; i < balls.Count - 1; i++)
                for (int j = i + 1; j < balls.Count; j++)
                    if (balls[i].IsCollisionWithBall(balls[j]))
                        balls[i].CollisionWithBall(balls[j]);
        }
    }
}
