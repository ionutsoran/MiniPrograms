using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Catmull_Rom_for_surfaces
{
    public static class Engine
    {
        
        public static  Form1 parent;
        public static PictureBox background;
        private static Graphics g;
        private static Bitmap map;
        private static Random rand;
        private static List<PointF> l;

        private static int[] angles;
        private static double[] t;
        private static double t0,t1,t2;
        private static PointF[] coords;
        private static int nr;
        public static int scaler;



        public static void Initialize(ContainerControl container)//aici initializez tot ce nu depind de val run-time
        {
            parent = container as Form1;
            background = parent.pictureBox1;
            map = new Bitmap(background.Width, background.Height);
            g = Graphics.FromImage(map);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            l = new List<PointF>();
            rand = new Random();
        }

        public static void Post_Initialize()//aici doar instantiezi valori care depind de valori run-time
        {
            nr =get_n(parent.textBox1.Text);
            angles = new int[nr];
            t = new double[nr];

          
        }

        public static void Start()//punctul de pornire al algoritmului
        {
           // MessageBox.Show((float)NextDouble(0,1f)+" "+(float)NextDouble(0,0.5f)+" "+(float)NextDouble(0,0.33f)+" "+ (float)NextDouble(0,0.25f));
            Post_Initialize();
            Set_rand_angles(nr);
            Calc_points(nr);
            Clear_Background(map);
            Clear_list(l);
            Calc_knot_points(nr);
            DrawPoints();
            Catmull_Rom();
            Catmull_Rom_rest();
            //g.DrawCurve(Pens.Red, l.ToArray());
            //for (int i = 0; i < nr; i++)
              //  g.DrawLine(Pens.Red, background.Width / 2, background.Height / 2, coords[i].X,coords[i].Y);
           // Catmull_Rom_test();
            Update_background(map);
        }

        public static void Update_background(Bitmap back)
        {
            background.BackgroundImage = back;
            background.Refresh();
        }

        
        public static void Set_rand_angles(int nr)
        {
            
            int v = 0;
            int c = 360 / nr;
            int r = 360 % nr;
            
            for(int i=0;i<nr-1;i++)
            {
                angles[i] = rand.Next(v,v+c+1);
                v += c;
            }
            angles[nr - 1] = r + rand.Next(v,v+c + 1);
      
            
        }

        public static void Calc_points(int nr)
        {
            coords = new PointF[nr];
            for (int i = 0; i < nr; i++)
            {
                float radian = angles[i] * ((float)Math.PI / 180);
                int x = rand.Next(120, 200);
                coords[i] = new PointF(background.Width / 2 + (float)Math.Sin(radian) * x, background.Height / 2 + (float)Math.Cos(radian) * x);
            }
        }

        public static void Calc_knot_points(int nr)
        {
            t[0] = 0f;
            for (int i = 1; i < nr; i++)
            {
                t[i] = Math.Pow((Math.Sqrt(Math.Pow(coords[i].X - coords[i-1].X, 2) + Math.Pow(coords[i].Y - coords[i-1].Y, 2))), 0.5f) + t[i-1];
            }
            // t[nr - 1] = Math.Pow((Math.Sqrt(Math.Pow(coords[0].X - coords[nr - 2].X, 2) + Math.Pow(coords[0].Y - coords[nr - 1].Y, 2))), 0.5f) + t[nr - 2];
            t0 = Math.Pow((Math.Sqrt(Math.Pow(coords[0].X - coords[nr - 1].X, 2) + Math.Pow(coords[0].Y - coords[nr - 1].Y, 2))), 0.5f) + t[nr - 1];
            t1 = Math.Pow((Math.Sqrt(Math.Pow(coords[1].X - coords[0].X, 2) + Math.Pow(coords[1].Y - coords[0].Y, 2))), 0.5f) + t0;
            t2 = Math.Pow((Math.Sqrt(Math.Pow(coords[2].X - coords[1].X, 2) + Math.Pow(coords[2].Y - coords[1].Y, 2))), 0.5f) + t1;

        }

        public static void Catmull_Rom()
        {

            for (int i=1;i<nr-2;i++)
            {
                //l.Add(coords[k - 1]);
                for (double ti=t[i];ti<t[i+1];ti+=0.0002f)
                {
                    
                    double a1y = ((t[i] - ti) / (t[i] - t[i-1])) * coords[i-1].Y + ((ti - t[i-1]) / (t[i] - t[i-1])) * coords[i].Y;
                    double a2y = ((t[i + 1] - ti) / (t[i + 1] - t[i])) * coords[i].Y + ((ti - t[i]) / (t[i + 1] - t[i])) * coords[i + 1].Y;
                    double a3y = ((t[i + 2] - ti) / (t[i + 2] - t[i + 1])) * coords[i + 1].Y + ((ti - t[i + 1]) / (t[i + 2] - t[i + 1])) * coords[i + 2].Y;
                    double b1y = ((t[i + 1] - ti) / (t[i + 1] - t[i-1])) * a1y + ((ti - t[i-1]) / (t[i + 1] - t[i-1])) * a2y;
                    double b2y = ((t[i + 2] - ti) / (t[i + 2] - t[i])) * a2y + ((ti - t[i]) / (t[i + 2] - t[i])) * a3y;
                    double cy = ((t[i + 1] - ti) / (t[i + 1] - t[i ])) * b1y + ((ti - t[i]) / (t[i+1] - t[i])) * b2y;

                    double a1x = ((t[i] - ti) / (t[i] - t[i - 1])) * coords[i - 1].X + ((ti - t[i - 1]) / (t[i] - t[i - 1])) * coords[i].X;
                    double a2x = ((t[i + 1] - ti) / (t[i + 1] - t[i])) * coords[i].X + ((ti - t[i]) / (t[i + 1] - t[i])) * coords[i + 1].X;
                    double a3x = ((t[i + 2] - ti) / (t[i + 2] - t[i + 1])) * coords[i + 1].X + ((ti - t[i + 1]) / (t[i + 2] - t[i + 1])) * coords[i + 2].X;
                    double b1x = ((t[i + 1] - ti) / (t[i + 1] - t[i - 1])) * a1x + ((ti - t[i - 1]) / (t[i + 1] - t[i - 1])) * a2x;
                    double b2x = ((t[i + 2] - ti) / (t[i + 2] - t[i])) * a2x + ((ti - t[i]) / (t[i + 2] - t[i])) * a3x;
                    double cx = ((t[i + 1] - ti) / (t[i + 1] - t[i])) * b1x + ((ti - t[i]) / (t[i + 1] - t[i])) * b2x;

                    l.Add(new PointF((float)cx, (float)cy));
                    g.FillEllipse(Brushes.Red,(float)cx,(float)cy, 1, 1);
                    //    Update_background(map);

                }
                
            }

           
        }

        public static void Catmull_Rom_test()
        {
            Point p0 = new Point(background.Width / 4 + 80, background.Height / 2 + rand.Next(-60, 60));
            Point p1 = new Point(p0.X + rand.Next(20,80), background.Height / 2 + rand.Next(-60, 60));
            Point p2 = new Point(p1.X + rand.Next(20, 80), background.Height / 2 + rand.Next(-60, 60));
            Point p3 = new Point(p2.X + rand.Next(20, 80), background.Height / 2 + rand.Next(-60, 60));
            Point p4 = new Point(p3.X - rand.Next(20, 80), background.Height / 2 + rand.Next(-60, 60));
            Point p5 = new Point(p3.X + rand.Next(20, 80), background.Height / 2 + rand.Next(-60, 60));
            Point p6 = new Point(p5.X + rand.Next(20, 80), background.Height / 2 + rand.Next(-60, 60));

            double t0 = 0f;
            double t1 = Math.Pow((Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2))), 0.5f) + t0;
            double t2 = Math.Pow((Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2))), 0.5f) + t1;
            double t3 = Math.Pow((Math.Sqrt(Math.Pow(p3.X - p2.X, 2) + Math.Pow(p3.Y - p2.Y, 2))), 0.5f) + t2;
            double t4 = Math.Pow((Math.Sqrt(Math.Pow(p4.X - p3.X, 2) + Math.Pow(p4.Y - p3.Y, 2))), 0.5f) + t3;
            double t5 = Math.Pow((Math.Sqrt(Math.Pow(p5.X - p4.X, 2) + Math.Pow(p5.Y - p4.Y, 2))), 0.5f) + t4;
            double t6 = Math.Pow((Math.Sqrt(Math.Pow(p6.X - p5.X, 2) + Math.Pow(p6.Y - p5.Y, 2))), 0.5f) + t5;

            //MessageBox.Show((float)t5 + "  " + (float)t6);
            g.FillEllipse(Brushes.Red, p0.X - 3, p0.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, p1.X - 3, p1.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, p2.X - 3, p2.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, p3.X - 3, p3.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, p4.X - 3, p4.Y - 3, 5, 5);
            g.FillEllipse(Brushes.Red, p5.X - 3, p5.Y - 3, 5, 5);
            g.DrawString("p0", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p0.X, p0.Y));
            g.DrawString("p1", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p1.X, p1.Y));
            g.DrawString("p2", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p2.X, p2.Y));
            g.DrawString("p3", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p3.X, p3.Y));
            g.DrawString("p4", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p4.X, p4.Y));
            g.DrawString("p5", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(p5.X, p5.Y));

            //g.FillEllipse(Brushes.Red, p6.X - 3, p6.Y - 3, 5, 5);


            /*  g.FillEllipse(Brushes.Blue, p0.X + (float)t0 - 3, background.Height / 2, 5, 5);
              g.FillEllipse(Brushes.Blue, p1.X + (float)t1 - 3, background.Height / 2, 5, 5);
              g.FillEllipse(Brushes.Blue, p2.X + (float)t2 - 3, background.Height / 2, 5, 5);
              g.FillEllipse(Brushes.Blue, p3.X + (float)t3 - 3, background.Height / 2, 5, 5);
              */
            
              l.Add(p1);             
           
              for (double t=t1;t<t2;t+= 0.0002f)
              {
                  double a1x = (t1 - t) / (t1 - t0) * p0.X + (t - t0) / (t1 - t0) * p1.X;
                  double a2x = (t2 - t) / (t2 - t1) * p1.X + (t - t1) / (t2 - t1) * p2.X;
                  double a3x = (t3 - t) / (t3 - t2) * p2.X + (t - t2) / (t3 - t2) * p3.X;

                  double a1y = (t1 - t) / (t1 - t0) * p0.Y + (t - t0) / (t1 - t0) * p1.Y;
                  double a2y = (t2 - t) / (t2 - t1) * p1.Y + (t - t1) / (t2 - t1) * p2.Y;
                  double a3y = (t3 - t) / (t3 - t2) * p2.Y + (t - t2) / (t3 - t2) * p3.Y;

                  double b1x = (t2 - t) / (t2 - t0) * a1x + (t - t0) / (t2 - t0) * a2x;
                  double b2x = (t3 - t) / (t3 - t1) * a2x + (t - t1) / (t3 - t1) * a3x;

                  double b1y = (t2 - t) / (t2 - t0) * a1y + (t - t0) / (t2 - t0) * a2y;
                  double b2y = (t3 - t) / (t3 - t1) * a2y + (t - t1) / (t3 - t1) * a3y;

                  double cx = (t2 - t) / (t2 - t1) * b1x + (t - t1) / (t2 - t1) * b2x;
                  double cy = (t2 - t) / (t2 - t1) * b1y + (t - t1) / (t2 - t1) * b2y;
                  l.Add(new PointF((float)cx, (float)cy));
                g.FillEllipse(Brushes.Red,(float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
              }
            //  l.Add(p2);


             for (double t=t2;t<t3;t+=0.0002f)
              {
                  double a1x = (t2 - t) / (t2 - t1) * p1.X + (t - t1) / (t2 - t1) * p2.X;
                  double a2x = (t3 - t) / (t3 - t2) * p2.X + (t - t2) / (t3 - t2) * p3.X;
                  double a3x = (t4 - t) / (t4 - t3) * p3.X + (t - t3) / (t4 - t3) * p4.X;

                  double a1y = (t2 - t) / (t2 - t1) * p1.Y + (t - t1) / (t2 - t1) * p2.Y;
                  double a2y = (t3 - t) / (t3 - t2) * p2.Y + (t - t2) / (t3 - t2) * p3.Y;
                  double a3y = (t4 - t) / (t4 - t3) * p3.Y + (t - t3) / (t4 - t3) * p4.Y;

                  double b1x = (t3 - t) / (t3 - t1) * a1x + (t - t1) / (t3 - t1) * a2x;
                  double b2x = (t4 - t) / (t4 - t2) * a2x + (t - t2) / (t4 - t2) * a3x;

                  double b1y = (t3 - t) / (t3 - t1) * a1y + (t - t1) / (t3 - t1) * a2y;
                  double b2y = (t4 - t) / (t4 - t2) * a2y + (t - t2) / (t4 - t2) * a3y;

                  double cx = (t3 - t) / (t3 - t2) * b1x + (t - t2) / (t3 - t2) * b2x;
                  double cy = (t3 - t) / (t3 - t2) * b1y + (t - t2) / (t3 - t2) * b2y;
                  l.Add(new PointF((float)cx, (float)cy));
                  g.FillEllipse(Brushes.Red,(float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
              }
            l.Add(p3);


            for (double t = t3; t < t4; t += 0.0002f)
            {
                double a1x = (t3 - t) / (t3 - t2) * p2.X + (t - t2) / (t3 - t2) * p3.X;
                double a2x = (t4 - t) / (t4 - t3) * p3.X + (t - t3) / (t4 - t3) * p4.X;
                double a3x = (t5 - t) / (t5 - t4) * p4.X + (t - t4) / (t5 - t4) * p5.X;

                double a1y = (t3 - t) / (t3 - t2) * p2.Y + (t - t2) / (t3 - t2) * p3.Y;
                double a2y = (t4 - t) / (t4 - t3) * p3.Y + (t - t3) / (t4 - t3) * p4.Y;
                double a3y = (t5 - t) / (t5 - t4) * p4.Y + (t - t4) / (t5 - t4) * p5.Y;

                double b1x = (t4 - t) / (t4 - t2) * a1x + (t - t2) / (t4 - t2) * a2x;
                double b2x = (t5 - t) / (t5 - t3) * a2x + (t - t3) / (t5 - t3) * a3x;

                double b1y = (t4 - t) / (t4 - t2) * a1y + (t - t2) / (t4 - t2) * a2y;
                double b2y = (t5 - t) / (t5 - t3) * a2y + (t - t3) / (t5 - t3) * a3y;

                double cx = (t4 - t) / (t4 - t3) * b1x + (t - t3) / (t4 - t3) * b2x;
                double cy = (t4 - t) / (t4 - t3) * b1y + (t - t3) / (t4 - t3) * b2y;
                l.Add(new PointF((float)cx, (float)cy));
                g.FillEllipse(Brushes.Red,(float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
            }

            //l.Add(p4);
          //  g.DrawCurve(Pens.Red, l.ToArray());

        } //functioneaza bine testat si verificat

        public static void Catmull_Rom_rest()
        {
           
            for (double ti = t[nr-2]; ti < t[nr-1]; ti +=0.0002f)
            {
                double a1y = ((t[nr - 2] - ti) / (t[nr - 2] - t[nr - 3])) * coords[nr - 3].Y + ((ti - t[nr - 3]) / (t[nr - 2] - t[nr - 3])) * coords[nr - 2].Y;
                double a2y = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * coords[nr - 2].Y + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * coords[nr - 1].Y;
                double a3y = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].Y + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].Y;
                double b1y = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 3])) * a1y + ((ti - t[nr - 3]) / (t[nr - 1] - t[nr - 3])) * a2y;
                double b2y = ((t0 - ti) / (t0 - t[nr - 2])) * a2y + ((ti - t[nr - 2]) / (t0 - t[nr - 2])) * a3y;
                double cy = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * b1y + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * b2y;

                double a1x = ((t[nr - 2] - ti) / (t[nr - 2] - t[nr - 3])) * coords[nr - 3].X + ((ti - t[nr - 3]) / (t[nr - 2] - t[nr - 3])) * coords[nr - 2].X;
                double a2x = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * coords[nr - 2].X + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * coords[nr - 1].X;
                double a3x = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].X + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].X;
                double b1x = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 3])) * a1x + ((ti - t[nr - 3]) / (t[nr - 1] - t[nr - 3])) * a2x;
                double b2x = ((t0 - ti) / (t0 - t[nr - 2])) * a2x + ((ti - t[nr - 2]) / (t0 - t[nr - 2])) * a3x;
                double cx = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * b1x + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * b2x;
                // l.Add(new PointF((float)cx, (float)cy));
                g.FillEllipse(Brushes.Red,(float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
            }
            //l.Add(coords[nr - 2]);
            for (double ti = t[nr - 1]; ti < t0; ti += 0.0002f)
            {
                double a1y = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * coords[nr - 2].Y + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * coords[nr - 1].Y;
                double a2y = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].Y + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].Y;
                double a3y = ((t1 - ti) / (t1 - t0)) * coords[0].Y + ((ti - t0) / (t1 - t0)) * coords[1].Y;
                double b1y = ((t0 - ti) / (t0 - t[nr - 2])) * a1y + ((ti - t[nr - 2]) / (t0 - t[nr - 2])) * a2y;
                double b2y = ((t1 - ti) / (t1 - t[nr - 1])) * a2y + ((ti - t[nr - 1]) / (t1 - t[nr - 1])) * a3y;
                double cy = ((t0 - ti) / (t0 - t[nr - 1])) * b1y + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * b2y;

                double a1x = ((t[nr - 1] - ti) / (t[nr - 1] - t[nr - 2])) * coords[nr - 2].X + ((ti - t[nr - 2]) / (t[nr - 1] - t[nr - 2])) * coords[nr - 1].X;
                double a2x = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].X + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].X; 
                double a3x = ((t1 - ti) / (t1 - t0)) * coords[0].X + ((ti - t0) / (t1 - t0)) * coords[1].X;
                double b1x = ((t0 - ti) / (t0 - t[nr - 2])) * a1x + ((ti - t[nr - 2]) / (t0 - t[nr - 2])) * a2x;
                double b2x = ((t1 - ti) / (t1 - t[nr - 2])) * a2x + ((ti - t[nr - 2]) / (t1 - t[nr - 2])) * a3x;
                double cx = ((t0 - ti) / (t0 - t[nr - 1])) * b1x + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * b2x;
                // l.Add(new PointF((float)cx, (float)cy));
                g.FillEllipse(Brushes.Red, (float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
            }

            for (double ti = t0; ti <t1; ti += 0.0002f)
            {
                double a1y = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].Y + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].Y;
                double a2y = ((t1 - ti) / (t1 - t0)) * coords[0].Y + ((ti - t0) / (t1 - t0)) * coords[1].Y;
                double a3y = ((t2 - ti) / (t2 - t1)) * coords[1].Y + ((ti - t1) / (t2 - t1)) * coords[2].Y;
                double b1y = ((t1 - ti) / (t1 - t[nr - 1])) * a1y + ((ti - t[nr - 1]) / (t1 - t[nr - 1])) * a2y;
                double b2y = ((t1 - ti) / (t1 - t[nr - 1])) * a2y + ((ti - t[nr - 1]) / (t1 - t[nr - 1])) * a3y;
                double cy = ((t1 - ti) / (t1 - t0)) * b1y + ((ti - t0) / (t1 - t0)) * b2y;

                double a1x = ((t0 - ti) / (t0 - t[nr - 1])) * coords[nr - 1].X + ((ti - t[nr - 1]) / (t0 - t[nr - 1])) * coords[0].X;
                double a2x = ((t1 - ti) / (t1 - t0)) * coords[0].X + ((ti - t0) / (t1 - t0)) * coords[1].X;
                double a3x = ((t2 - ti) / (t2 - t1)) * coords[1].X + ((ti - t1) / (t2 - t1)) * coords[2].X;
                double b1x = ((t1 - ti) / (t1 - t[nr - 1])) * a1x + ((ti - t[nr - 1]) / (t1 - t[nr - 1])) * a2x;
                double b2x = ((t1 - ti) / (t1 - t[nr - 1])) * a2x + ((ti - t[nr - 1]) / (t1 - t[nr - 1])) * a3x;
                double cx = ((t1 - ti) / (t1 - t0)) * b1x + ((ti - t0) / (t1 - t0)) * b2x;
                // l.Add(new PointF((float)cx, (float)cy));
                g.FillEllipse(Brushes.Red, (float)cx, (float)cy, 1, 1);
                //g.DrawCurve(Pens.Red,)
            }
            
        }

        public static double NextDouble(double min,double max)
        {
           // double min1 = min - (int)min;
           // double max1 = max - (int)max;
            return (rand.NextDouble() * (max - min) + min);
        }


        public static int get_n(string s)
        {
            int n;
            try
            {
                n = int.Parse(s);
            }
            catch(ArgumentException e)
            {
                MessageBox.Show("Wrong number!");
                return 0;
            }
            return n;
         
        }


        public static void BackgroundGrid(int scaler,Color linecolor,Color backcolor)
        {
           
        }

        public static void DrawPoints()
        {
            for (int i = 0; i < nr; i++)
                g.FillEllipse(Brushes.Red, coords[i].X-3, coords[i].Y-3, 5, 5);
        }

        public static void DrawLines_surface()
        {
            for (int i = 0; i < nr - 1; i++)
                g.DrawLines(Pens.Red, coords);
            g.DrawLine(Pens.Red, coords[nr - 1], coords[0]);
        }

        public static void Clear_Background(Bitmap map)
        {
            g.Clear(Color.White);
        }
        
        public static void Clear_list(List<PointF> li)
        {
            li.Clear();
        }
    }
}
/*    PointF[] f = new PointF[nr];
          for(int i=0;i<nr;i++)
          {
              float radian = angles[i] * ((float)Math.PI/180);
              int x = rand.Next(15,200);
              g.FillEllipse(Brushes.Red,background.Width/2+(float)Math.Sin(radian) * x, background.Height/2+(float)Math.Cos(radian) * x,2,2);
              f[i] = new PointF(background.Width / 2 + (float)Math.Sin(radian) * x, background.Height / 2 + (float)Math.Cos(radian) * x);
              g.DrawLine(Pens.Black, background.Width / 2, background.Height / 2, background.Width / 2 + (float)Math.Sin(radian) * x, background.Height / 2 + (float)Math.Cos(radian) * x);
          }

          for (int i = 0; i < nr-1; i++)
          {
              g.DrawLine(Pens.Red, f[i], f[i + 1]);
          }
          g.DrawLine(Pens.Red, f[nr - 1], f[0]);*/
