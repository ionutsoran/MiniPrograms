using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FractalTree
{
    public static class Engine
    {
        public static Graphics g;
        public static Bitmap map;
        public static PointF center;
        public static PictureBox pb;
        public static Random rand;
        

        public static void Init(PictureBox pb1)
        {
            pb = pb1;
            map = new Bitmap(pb1.Width, pb1.Height);
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            rand = new Random();
        }

        public static void Start()
        {
            center = new PointF(map.Width / 2, map.Height / 2);
            g.DrawLine(Pens.Brown, new PointF(center.X, center.Y + 150), center);
            CalcTree(center, 100, 0,Color.Brown);
            //g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256),rand.Next(256),0)), center.X - 250, center.Y - 250, 498,498);
            //CalcFractal(new PointF(center.X-250,center.Y-250), 500);
            pb.Image = map;
        }

        public static void CalcTree(PointF start,float x,float alfa,Color color)
        {
            if(x>1)
            {
                float alfa1 = (float)(2 * Math.PI * 30) / 360;
                PointF p1 = new PointF(start.X +(float)Math.Cos(Math.PI + Math.PI/2 + alfa1 + alfa) * x, start.Y + (float)Math.Sin(Math.PI + Math.PI/2 + alfa1 + alfa) * x);
                PointF p2 = new PointF(start.X + (float)Math.Cos(Math.PI+Math.PI/2 - alfa1 + alfa) * x, start.Y + (float)Math.Sin(Math.PI + Math.PI/2 - alfa1 + alfa) * x);
                g.DrawLine(new Pen(color), start, p1);
                g.DrawLine(new Pen(color), start, p2);
              if(x<35)
              {
                    //g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256),0)), new PointF[] { start, p1, p2 });
                    CalcTree(p1, x / 1.5f, alfa + alfa1, Color.Green);
                    CalcTree(p2, x / 1.5f, alfa - alfa1, Color.Green);
              }
              else
              {
                    //g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), new PointF[] { start, p1, p2 });
                    CalcTree(p1, x / 1.5f, alfa + alfa1, Color.Brown);
                    CalcTree(p2, x / 1.5f, alfa - alfa1, Color.Brown);
              }
                 
            }
            
            
        }

        public static void DrawTree(PointF start,float x,float y)
        {
            g.FillRectangle(Brushes.LawnGreen,start.X-x/2,start.Y-y/2,x,y);
        }

        public static void CalcFractal(PointF start,int l)
        {
            if(l>1)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), start.X, start.Y, l / 3, l / 3);
                g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), start.X+2*(l/3), start.Y, l / 3, l / 3);
                g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), start.X+l/3, start.Y+l/3, l / 3, l / 3);
                g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), start.X, start.Y+2*(l/3), l / 3, l / 3);
                g.FillRectangle(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), start.X + 2 * (l / 3), start.Y + 2 * (l/3), l / 3, l / 3);
                CalcSerpinski(new PointF(start.X + l / 3, start.Y+l/3), new PointF(start.X + 2 * (l / 3), start.Y+l/3), new PointF((start.X * 2 + 3 * (l / 3)) / 2, start.Y ));
                CalcSerpinski(new PointF(start.X + l / 3, start.Y + 2 * (l / 3)), new PointF(start.X + (l / 3), start.Y + (l / 3)), new PointF(start.X, start.Y + l / 3 + l / 6));
                CalcSerpinski(new PointF(start.X + 2 * (l / 3), start.Y + 2 * (l / 3)), new PointF(start.X + 2 * (l / 3), start.Y + l / 3), new PointF(start.X + l, start.Y + l / 3 + l / 6));
                CalcSerpinski(new PointF(start.X +(l / 3), start.Y + 2*(l / 3)), new PointF(start.X +2*(l/3), start.Y +2*(l / 3)), new PointF((start.X * 2 + 3 * (l / 3)) / 2, start.Y +l));
                CalcFractal(new PointF(start.X, start.Y), l / 3);
                CalcFractal(new PointF(start.X+2*(l/3), start.Y), l / 3);
                CalcFractal(new PointF(start.X + l / 3, start.Y + l / 3), l / 3);
                CalcFractal(new PointF(start.X, start.Y + 2 * (l/3)), l / 3);
                CalcFractal(new PointF(start.X+2*(l/3), start.Y+2*(l/3)), l / 3);
            }
        }

        public static void CalcSerpinski(PointF p1,PointF p2,PointF p3)
        {
            if(Dist(p1,p2)&&Dist(p2,p3)&&Dist(p1,p3))
            {
                PointF m1 = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                PointF m2 = new PointF((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);
                PointF m3 = new PointF((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256),0)), p1, p2);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), p1, p3);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), 0)), p2, p3);
                CalcSerpinski(p1, m1, m2);
                CalcSerpinski(p2, m1, m3);
                CalcSerpinski(p3, m2, m3);
            }
        }
        

        public static bool Dist(PointF p1,PointF p2)
        {
            if (Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)) > 1)
                return true;
            return false;
        }
    }
}
