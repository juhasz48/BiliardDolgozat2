using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labda
{
    public class Ball
    {
        public Point Location { get; set; }
        public Size BallSize;
        public Point Vector { get; set; }

        private List<Point> Linepoints = new();
        private readonly Pen pen = new(Brushes.DarkRed, 2);
        public readonly int Number;
        public int radius;

        private List<Color> BallColors = new List<Color>() {
            Color.Yellow,
            Color.Blue,
            Color.Red,
            Color.Purple,
            Color.OrangeRed,
            Color.Green,
            Color.Brown,
            Color.Black
        };
        private double alfa = 0;
        public double Alfa
        {
            get { return alfa; }
            set
            {
                alfa = value;
                CalculateVector();
            }
        }
        private readonly Color color;
        private double f = 0;
        public double F
        {
            get { return f; }
            set
            {
                f = value < 0.1 ? 0 : value > 10 ? 10 : value;
                CalculateVector();
            }
        }
        private int Fdiv = 3;
        private void CalculateVector() => Vector = new Point((int)(F / Fdiv * Math.Sin(Alfa * 1.0 / 180 * Math.PI)), (int)(F / Fdiv * Math.Cos(Alfa / 180 * Math.PI)));
        private bool ReqDrawWay { get; set; }
        public Ball(Point location, double alfa, int f, Color color, bool reqdrawway, int radius)
        {
            Location = location;
            Alfa = alfa;
            F = f;
            this.color = color;
            ReqDrawWay = reqdrawway;
            this.radius = radius;
        }
        public Ball(Point location, bool reqdrawway, int number, int radius)
        {
            Location = location;
            ReqDrawWay = reqdrawway;
            Alfa = 0;
            F = 0;
            Number = number;
            color = number == 0 ? Color.White : BallColors[number > 8 ? number - 9 : number - 1];
            this.radius = radius;
        }
        private bool IsStriped => Number > 8;
        public bool IsCollisionWithBall(Ball b)
            => Math.Sqrt(Math.Pow(Location.X - b.Location.X, 2)
                + Math.Pow(Location.Y - b.Location.Y, 2)) < radius + b.radius;

        public void CollisionWithBall(Ball b)
        {
            Point v1 = new(Location.X - b.NewLocation.X, Location.Y - b.NewLocation.Y);
            Point v2 = new(b.Location.X - NewLocation.X, b.Location.Y - NewLocation.Y);
            double tmpF = F;
            b.Alfa = 180 + RadToAng(GetRadian(v1.X, v1.Y));
            F = b.F; //* 0.95;
            b.F = tmpF; //<= 0 ? F * 0.2 : tmpF * 0.8;
            Alfa = -(180 + RadToAng(GetRadian(v2.X, v2.Y)));
        }
        public double RadToAng(double radians) => 90 - radians * (180 / Math.PI);
        public double GetRadian(int x, int y) => Math.Atan2(y, x);
        private Point NewLocation => new Point(Location.X + Vector.X, Location.Y + Vector.Y);
        private bool IsInArea(Size size) => NewLocation.X + radius < size.Width
            && NewLocation.X - radius > 0 && NewLocation.Y - radius > 0 && NewLocation.Y + radius < size.Height;
        public void Step(double Fmult)
        {
            Location = NewLocation;
            F *= Fmult;
        }
        public void Collision(double AlfaMult, BorderType border)
        {
            switch (border)
            {
                case BorderType.Top:
                    {
                        double beta = -90 + Alfa;
                        Alfa = 90 - beta;// * AlfaMult;
                        F *= 0.8;
                        break;
                    }

                case BorderType.Left:
                    {
                        double beta = -Alfa;
                        Alfa = beta;// * AlfaMult;
                        F *= 0.8;
                        break;
                    }

                case BorderType.Bottom:
                    {
                        double beta = -90 + Alfa;
                        Alfa = 90 - beta;//* AlfaMult;
                        F *= 0.8;
                        break;
                    }

                case BorderType.Right:
                    {
                        double beta = -Alfa;
                        Alfa = beta;// * AlfaMult;
                        F *= 0.8;
                        break;
                    }
            }
        }
        public void Paint(double Fmult, Graphics kep, Size size, double alfamult)
        {
            if (!IsInArea(size))
            {
                BallToWall(size);
                Collision(alfamult, GetBorderType(size));
            }
            Step(Fmult);
            if (ReqDrawWay && Linepoints.Count > 1)
                DrawWay(kep);
            Linepoints.Add(new Point(Location.X, Location.Y));
            DrawBall(kep);
        }
        private void BallToWall(Size size)
        {
            int newx, newy;
            if (NewLocation.X - radius < 0 || NewLocation.X + radius > size.Width)
            {
                newx = NewLocation.X < size.Width / 2 ? radius : size.Width - radius;
                Location = new Point(newx, Location.Y + (int)((newx - Location.X * 1.0) / Vector.Y * Vector.X));
                Linepoints.Add(new Point(Location.X, Location.Y));
            }
            if (NewLocation.Y - radius < 0 || NewLocation.Y + radius > size.Height)
            {
                newy = NewLocation.Y < size.Height / 2 ? radius : size.Height - radius;
                Location = new Point(Location.X + (int)((newy - Location.Y * 1.0) / Vector.Y * Vector.X), newy);
                Linepoints.Add(new Point(Location.X, Location.Y));
            }
        }
        private void DrawBall(Graphics kep)
        {
            var font = new Font(new FontFamily("Arial"), 10, FontStyle.Bold);
            if (!IsStriped)
            {
                kep.FillEllipse(new SolidBrush(color), new Rectangle(Location.X - radius, Location.Y - radius, radius * 2, radius * 2));
                if (Number > 0)
                    kep.DrawString(Number.ToString(), font, Brushes.White, new PointF(Location.X - radius + 10, Location.Y - radius + 7));
            }
            else
            {
                kep.FillEllipse(new SolidBrush(color), new Rectangle(Location.X - radius, Location.Y - radius, 2 * radius, 2 * radius));
                kep.FillPie(Brushes.White, Location.X - radius, Location.Y - radius - 3, radius * 2 - 3, radius * 2 - 10, 180, 180);
                kep.FillPie(Brushes.White, Location.X - radius, Location.Y - radius + 11, radius * 2 - 2, radius * 2 - 11, 0, 180);
                kep.DrawString(Number.ToString(), font, Brushes.White, new PointF(Location.X - radius + 6, Location.Y - radius + 7));
            }
            kep.DrawEllipse(Pens.White, new Rectangle(Location.X - radius + radius / 2, Location.Y - radius + radius * 2 / 4, radius, radius));
        }
        private BorderType GetBorderType(Size size)
        {
            if (NewLocation.Y + radius > size.Height - 2)
                return BorderType.Bottom;
            if (NewLocation.X + radius > size.Width - 2)
                return BorderType.Right;
            if (NewLocation.X - radius < 0)
                return BorderType.Left;
            if (NewLocation.Y - radius < 0)
                return BorderType.Top;
            else return 0;
        }
        public bool InBall(Point mouselocation) =>  mouselocation.X <= Location.X + radius && mouselocation.X >= Location.X - radius
                && mouselocation.Y <= Location.Y + radius && mouselocation.Y >= Location.Y - radius;
        public void SetLocation(Point mouselocation, Size size)
        {
            //ha mozog nem engedem modosítani
            if (IsMoving() && mouselocation.X > 0 && mouselocation.X < size.Width && mouselocation.Y > 0 && mouselocation.Y < size.Height)
                Location = new(mouselocation.X,mouselocation.Y);
            //labda loc modositas
            //csak akkor modosítom ha az a size-on belül van
        }
        public bool IsMoving() => F == 0;
        private void DrawWay(Graphics kep) => kep.DrawLines(pen, Linepoints.ToArray());
    }
}
