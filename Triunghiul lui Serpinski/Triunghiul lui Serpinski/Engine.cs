using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Triunghiul_lui_Serpinski
{
    public static class Engine
    {
        public static Graphics g;
        public static Bitmap map;
        public static PointF center;
        public static PointF A, B, C;
        public static PictureBox box;
        public static Random rand;

        public static void Init(PictureBox box1)
        {
            box = box1;
            map = new Bitmap(box1.Width,box1.Height);
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            rand = new Random();
        }

        public static void Start()
        {
            center = new PointF(map.Width / 2, map.Height / 2);
            DrawTriangle(new PointF(center.X-300,center.Y+300),600);
            Serpinski(A,B,C);
            box.Image = map;
        }

        public static void DrawTriangle(PointF start,int l)
        {
            
            float aria = (l * l * (float)Math.Sqrt(3)) / 4;
            float h = (aria * 2) / l;
            B = start;
            C = new PointF(start.X + l, start.Y);
            A = new PointF((start.X * 2 + l) / 2, start.Y - h);
           
        }

        public static void Serpinski(PointF a,PointF b,PointF c)
        {
            if(Dist(a,b))
            {
                PointF m1 = new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                PointF m2 = new PointF((a.X + c.X) / 2, (a.Y + c.Y) / 2);
                PointF m3 = new PointF((b.X + c.X) / 2, (b.Y + c.Y) / 2);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), a, b);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), a, c);
                g.DrawLine(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), b, c);
                //  g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))),new PointF[] { m1,m2,m3});
                // g.DrawLine(Pens.Blue, a, b);
                // g.DrawLine(Pens.Blue, a, c);
                // g.DrawLine(Pens.Blue, b, c);
                Serpinski2(m1, m2, m3);
                Serpinski(a, m1, m2);
                Serpinski(b, m1, m3);
                Serpinski(c, m2, m3);
            }
        }

        public static void Serpinski2(PointF a, PointF b, PointF c)
        {
            if (Dist(a, b))
            {
                PointF m1 = new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                PointF m2 = new PointF((a.X + c.X) / 2, (a.Y + c.Y) / 2);
                PointF m3 = new PointF((b.X + c.X) / 2, (b.Y + c.Y) / 2);
                g.DrawLine(Pens.Blue, a, b);
                g.DrawLine(Pens.Blue, a, c);
                g.DrawLine(Pens.Blue, b, c);
                g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), new PointF[] { m1, m2, m3 });
                Serpinski2(a, m1, m2);
                Serpinski2(b, m1, m3);
                Serpinski2(c, m2, m3);
            }
        }

        public static bool Dist(PointF a,PointF b)
        {
            //float dist = (float)Math.Sqrt((a.X-b.X)*(a.X-b.Y)+(a.Y-b.Y)*(a.Y-b.Y));
            if (Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) > 1)
               return true;
            return false;
        }
    }
}
