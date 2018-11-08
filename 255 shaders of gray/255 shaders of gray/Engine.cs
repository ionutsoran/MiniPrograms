using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace _255_shaders_of_gray
{
    public static class Engine
    {
#region variables declaration
        public static Graphics g;
        public static Bitmap map;
        public static Bitmap tmap;
        public static PictureBox drawspace;
        public static List<Point> points;
        public static float rad;
        public static string[] functionsnames;
        public static ListBox lb1;
        public static TextBox tb1, tb2, tb3, tb4;
        public static ComboBox cb1;
        public static Random rand;
        public static Starclass[] pp;
        public static Timer time;
        public static Point middle;

        //for warpspace section
        public static float[] contor;
        public static float[] acceleration;
        public static float z;
        public static float[] tempx, tempy;
        public static float[] index;


        //for trajectory section
        public static float d;
        public static float gconst;
        public static float grav;
        public static float speed;
        public static float force;
        public static float angle;
        public static int ballsize;

        //for vertical trajectory
        public static List<ColorPoint> ps;
        public static bool toinit;

        //for fireworks
        public static Firework[] fpoints;

        //testone
        public static int kcolor;

        //actions list
        public static List<MyAction> ToCall;
        public static List<MyAction> Update_func;
        #endregion

        #region states of the Engine
        public static void Initialize(PictureBox pb, ListBox lb, TextBox tba, TextBox tbb, TextBox tbc, TextBox tbd, ComboBox cb)
        {
            drawspace = pb;
            lb1 = lb;
            tb1 = tba;
            tb2 = tbb;
            tb3 = tbc;
            tb4 = tbd;
            cb1 = cb;
            map = new Bitmap(drawspace.Width, drawspace.Height);//draw space
            tmap = new Bitmap(drawspace.Width, drawspace.Height);
            // g = Graphics.FromImage(map); //graphics API
            rand = new Random();  //random class
            cb.SelectedIndexChanged += Combobox_indexchanged; //combobox for function selection
            cb.Focus();
            middle = new Point(drawspace.Height / 2, drawspace.Height / 2); //get the middle point of the canvas
            points = new List<Point>(); //a list that contains points to be rendered
            ps = new List<ColorPoint>();
            Update_func = new List<MyAction>(); //list of updatable functions
            ToCall = new List<MyAction>(); //list of the functions to be called
            rad = (float)Math.PI / 180;  // 1 radian value
            toinit = true;
            kcolor =100;


            time = new Timer();  //update
            time.Interval = 1000;
            time.Tick += Update;
            time.Start();
            // contor = 1.5f;
            // gconst = 9.80665f;
            gconst = 0.29419f;
            force = 50;
            d = 0;
            ballsize = 8;


            ToCall.Add(new MyAction(() => DrawStars(int.Parse(tb1.Text)), false, "drawspace"));
            ToCall.Add(new MyAction(() => DrawStars_White(int.Parse(tb1.Text)), false, "drawspace_white"));
            ToCall.Add(new MyAction(() => Trajectory(30), false, "trajectory"));
            ToCall.Add(new MyAction(() => Vertical_Trajectory(20), false, "vtrajectory"));
            ToCall.Add(new MyAction(() => Ameba_fractal(500), false, "ameba_fractal"));
            ToCall.Add(new MyAction(() => Testone1(), false,"testone"));
            ToCall.Add(new MyAction(() => Testone2(), false, "testone2"));
            ToCall.Add(new MyAction(() => Testone3(), false, "testone3"));
            ToCall.Add(new MyAction(() => Testone4(), false, "testone4"));
            ToCall.Add(new MyAction(() => Testone5(), false, "testone5"));
            ToCall.Add(new MyAction(() => Testone6(), false, "testone6"));
            ToCall.Add(new MyAction(() => PathGradient(), false, "pathgradient"));
            ToCall.Add(new MyAction(() => Distgradient(), false, "distgradient"));
            ToCall.Add(new MyAction(() => Xsquaredvs2X(), false, "xsquared_vs_2x"));
            ToCall.Add(new MyAction(() => Terrain2D(), false, "Quadric_Spline"));
            ToCall.Add(new MyAction(() => Nvertices_polygon(), false, "Nvertices_polygon"));
            ToCall.Add(new MyAction(() => ShowIfPrime(int.Parse(tb1.Text)), false, "ShowIfPrime"));
            Update_func.Add(new MyAction(() => Drawstars_Update(), false, "drawspace"));
            Update_func.Add(new MyAction(() => Drawstars_White_Update(), false, "drawspace_white"));
            Update_func.Add(new MyAction(() => Trajectory_Update(), false, "trajectory"));
            Update_func.Add(new MyAction(() => Vertical_Trajectory_Update(), false, "vtrajectory"));


            functionsnames = new string[] {"Warp Space","Warp Space white","Projected Trajectory","Vertical Trajectory","Ameba Fractal","Heightmap with color",
                                           "Heightmap with grayscale","Heightmap with spline grayscale","!!Close to Perlin","Special Effects 1",
                                           "Special Effects 2","Path Gradient","Dist Gradient","xsquared_vs_2x","Quadric_Spline","Nvertices_polygon","ShowIfPrime"};
            cb.Items.Clear();
            cb.Items.AddRange(functionsnames);
            cb.SelectedIndex = 0;
        }

        public static void Start()
        {
            //Point one = new Point(map.Width / 2 - 100, map.Height / 2 - 100);
            //Point two = new Point(one.X + 200, one.Y);
            // g.DrawLine(Pens.BlueViolet, one, two);
            /* Koch_snoflake(one, two,false);
             for (int i = 0; i < points.Count; i++)
                 g.DrawEllipse(Pens.Cyan, points[i].X, points[i].Y, 1, 1);
             */
            //  MessageBox.Show((-1.769230*(180 / Math.PI)).ToString());
            // Drawlines(int.Parse(tb1.Text));
            // DrawStars(int.Parse(tb1.Text),200);
            // Trajectory(30);
            // Testone4();
            // g = Graphics.FromImage(map);
            foreach (MyAction i in ToCall)
                if (i.torun)
                    i.method.Invoke();      
            /* drawspace.BackgroundImage = null;
             drawspace.BackgroundImage = map;*/

        }

        private static void Update(object sender, EventArgs e)
        {
            foreach (MyAction i in Update_func)
            {
                if (i.torun)
                    i.method.Invoke();
            }
            //Updatebackground(drawspace, map);
        }
        #endregion

        #region some geometry and classic fractals
        public static void Rand_polygon()
        {

        }//not implemented yet

        public static void Koch_snoflake(Point one, Point two, bool flip)
        {
            float dist = (float)Math.Sqrt((one.X - two.X) * (one.X - two.X) + (one.Y - two.Y) * (one.Y - two.Y));

         
        } //indev

        public static void Drawlines(int cadran) //drawing lines based on their specific quarter
        {
            g = Graphics.FromImage(map);

        } //indev
        #endregion

        #region different gradient patterns

        public static void Testone1() //trying to reproduce perlin noise with color
        {
            g = Graphics.FromImage(map);
            Point displace = new Point(middle.X, middle.Y);
            for (int i = 0; i < 100; i++)
            {
                displace = new Point(displace.X + rand.Next(-80, 80), displace.Y + rand.Next(-80, 80));
                int x = rand.Next(15, 20);
                Point[] pts = new Point[x];
                for (int j = 0; j < x; j++)
                {
                    float angle = rand.Next(360);
                    int l = rand.Next(1, 30);
                    pts[j] = new Point(displace.X + (int)(Math.Sin(angle * rad) * l), displace.Y + (int)(Math.Cos(angle * rad) * l));
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256), rand.Next(256))), pts);
            }
            Updatebackground(drawspace, map);
        }

        public static void Testone2() //trying to reproduce perlin noise with grayscale
        {
            g = Graphics.FromImage(map);
            Point displace = new Point(middle.X, middle.Y);
            for (int i = 0; i < 100; i++)
            {
                displace = new Point(displace.X + rand.Next(-80, 80), displace.Y + rand.Next(-80, 80));
                int x = rand.Next(15, 20);
                Point[] pts = new Point[x];
                for (int j = 0; j < x; j++)
                {
                    float angle = rand.Next(360);
                    int l = rand.Next(1, 30);
                    pts[j] = new Point(displace.X + (int)(Math.Sin(angle * rad) * l), displace.Y + (int)(Math.Cos(angle * rad) * l));
                }
                int c = rand.Next(256);
                kcolor = (kcolor + c) / 2;
                g.FillPolygon(new SolidBrush(Color.FromArgb(c, kcolor, kcolor, kcolor)), pts);
            }
            Updatebackground(drawspace, map);
        }

        public static void Testone3() //trying to reproduce perlin noise with spline grayscale
        {
            g = Graphics.FromImage(map);
            Point displace = new Point(middle.X,middle.Y);
            for(int i=0;i<100;i++)
            {
                displace = new Point(displace.X+rand.Next(-80,80),displace.Y+rand.Next(-80,80));
                int x = rand.Next(15, 20);
                Point[] pts = new Point[x];
                for (int j = 0; j < x; j++)
                {
                    float angle = rand.Next(360);
                    int l = rand.Next(1, 30);
                    pts[j] = new Point(displace.X + (int)(Math.Sin(angle * rad) * l), displace.Y + (int)(Math.Cos(angle * rad) * l));
                }
                int c = rand.Next(256);
                kcolor = (kcolor + c) / 2;
                g.FillClosedCurve(new SolidBrush(Color.FromArgb(c, kcolor, kcolor, kcolor)),pts);
            }
            Updatebackground(drawspace, map);
        }

        public static void Testone4()
        {
            g = Graphics.FromImage(map);
            Point stpoint = new Point(13, 13);
            float Xn = drawspace.Width / 25;
            float Yn = drawspace.Height / 25;
            for (int i = 0; i < Xn; i++)
            {
                for (int j = 0; j < Yn; j++)
                {
                    stpoint = new Point(13 + j * 25, 13 + i * 25);
                    Point[] pts = new Point[rand.Next(20, 30)];
                    for (int k = 0; k < pts.Length; k++)
                    {
                        int angle = rand.Next(360);
                        float l = rand.Next(25, 125) + (float)rand.NextDouble();
                        float xi = (float)Math.Sin(angle * rad) * l;
                        float yi = (float)Math.Cos(angle * rad) * l;
                        pts[k] = new Point(stpoint.X + (int)xi, stpoint.Y + (int)yi);
                        if (rand.Next(1, 4) % 3 == 0)
                            Recursiv_Test4(pts[k], 63);
                    }
                    int c = rand.Next(256);
                    kcolor = (kcolor + c) / 2;
                    g.FillPolygon(new SolidBrush(Color.FromArgb(kcolor, kcolor, kcolor, kcolor)), pts);

                }

            }
            Updatebackground(drawspace, map);
        } //important result close to perlin 

        public static void Recursiv_Test4(Point pt, int lentgh)
        {
            if (lentgh > 1)
            {
                Point[] pts = new Point[rand.Next(20, 30)];
                for (int k = 0; k < pts.Length; k++)
                {
                    int angle = rand.Next(360);
                    float l = rand.Next(1, lentgh) + (float)rand.NextDouble();
                    float xi = (float)Math.Sin(angle * rad) * l;
                    float yi = (float)Math.Cos(angle * rad) * l;
                    pts[k] = new Point(pt.X + (int)xi, pt.Y + (int)yi);
                    if (rand.Next(1, 27) % 7 == 0)
                        Recursiv_Test4(pts[k], lentgh / 2);
                }
                int c = rand.Next(256);
                kcolor = (kcolor + c) / 2;
                g.FillPolygon(new SolidBrush(Color.FromArgb(kcolor, kcolor, kcolor, kcolor)), pts);
            }
        } //update for test4 function

        public static void Testone5()
        {
            g = Graphics.FromImage(map);
            Point displace = new Point(middle.X, middle.Y);
            for (int i = 0; i < 100; i++)
            {
                displace = new Point(displace.X + rand.Next(-10, 10), displace.Y + rand.Next(-10, 10));
                int x = rand.Next(7, 30);
                Point[] pts = new Point[x];
                for (int j = 0; j < x; j++)
                {
                    float angle = rand.Next(360);
                    int l = rand.Next(1, 30);
                    pts[j] = new Point(displace.X + (int)(Math.Sin(angle * rad) * l), displace.Y + (int)(Math.Cos(angle * rad) * l));
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256), rand.Next(256))), pts);
            }
            Updatebackground(drawspace, map);
        } //this is more for visual effects

        public static void Testone6()
        {
            g = Graphics.FromImage(map);
            Point displace = new Point(middle.X, middle.Y);
            for (int i = 0; i < 100; i++)
            {
                displace = new Point(displace.X + rand.Next(-10, 10), displace.Y + rand.Next(-10, 10));
                int x = rand.Next(7, 15);
                Point[] pts = new Point[x];
                for (int j = 0; j < x; j++)
                {
                    float angle = rand.Next(360);
                    int l = rand.Next(30, 80);
                    pts[j] = new Point(displace.X + (int)(Math.Sin(angle * rad) * l), displace.Y + (int)(Math.Cos(angle * rad) * l));
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), pts);
            }
            Updatebackground(drawspace, map);
        } //this is more for visual effects

        public static void PathGradient()
        {
            g = Graphics.FromImage(map);
            int nr = 72;
            for(int i=0;i<nr;i++)
            {
                int xi = rand.Next(10, drawspace.Width - 10);
                int yi = rand.Next(10, drawspace.Height - 10);
                int c = rand.Next(256);
                map.SetPixel(xi,yi,Color.FromArgb(c,c,c,c));

                for(int j=1;j<10;j++)
                    for(int k=1;k<10;k++)
                    {
                        int ck = (c+(k-1)*j)/k;
                        Color cl = Color.FromArgb(ck,ck,ck,ck);
                        map.SetPixel(xi + k, yi + j, cl);
                        map.SetPixel(xi - k, yi + j, cl);
                        map.SetPixel(xi + k, yi - j, cl);
                        map.SetPixel(xi - k, yi - j, cl);
                    }
            }
            Updatebackground(drawspace, map);
            
        } //this is for visual effect

        public static void Distgradient()
        {
            g = Graphics.FromImage(map);
            Point pt1 = new Point(rand.Next(10, drawspace.Width - 10), rand.Next(10, drawspace.Height - 10));
          
            int r = rand.Next(20, 51);
            for (int i = 0; i < 700; i++)
                for (int j = 0; j < 700; j++)
                {

                }
            Updatebackground(drawspace, map);
        } //this is for visual effect BUGGED!
        #endregion

        #region some pathfinding fractal

        public static void Ameba_fractal(float radius)
        {
            g = Graphics.FromImage(map);
            float angle = rand.Next(360);
            g.DrawEllipse(new Pen(Color.Chocolate, 3), middle.X - (drawspace.Width - 40) / 2, middle.Y - (drawspace.Height - 40) / 2, drawspace.Width - 40, drawspace.Height - 40);
            for (int i = 0; i < rand.Next(10, 51); i++)
            {
                angle = rand.Next(360);
                int distance = rand.Next(drawspace.Width - 39);
                int size = rand.Next(distance / 12, distance / 4);
                int ad = rand.Next(distance / 5, (int)(distance / 2.4f));
                float catx = middle.X + (float)Math.Sin(angle * rad) * ad;
                float caty = middle.Y + (float)Math.Cos(angle * rad) * ad;
                g.DrawEllipse(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))), catx - size / 2, caty - size / 2, size, size);
            }
            tempx = new float[rand.Next(6, 9)];
            for (int i = 0; i < tempx.Length; i++)
            {
                angle = rand.Next(360);
                tempx[i] = angle;
                points.Add(new Point(middle.X + (int)(Math.Sin(angle * rad) * (drawspace.Width - 40) / 2), middle.Y + (int)(Math.Cos(angle * rad) * (drawspace.Height - 40) / 2)));
                g.FillEllipse(Brushes.Blue, points[i].X - 3, points[i].Y - 3, 6, 6);
            }
            g.FillEllipse(Brushes.Black, middle.X - 4, middle.Y - 4, 8, 8);
            float dist = (float)Math.Sqrt((points[0].X - middle.X) * (points[0].X - middle.X) + (points[0].Y - middle.Y) * (points[0].Y - middle.Y));
            Point[] pts;

            for (int j = 0; j < tempx.Length; j++)
            {
                pts = new Point[(int)dist];
                for (int i = 0; i < (int)dist; i++)
                {
                    int x = rand.Next(-12, 12);
                    pts[i] = (new Point(middle.X + (int)(Math.Sin((tempx[j] + x) * rad) * i), middle.Y + (int)(Math.Cos((tempx[j] + x) * rad) * i)));

                }
                g.DrawCurve(Pens.Blue, pts);
            }

            Updatebackground(drawspace, map);
        } //indev fractal

        public static void Ameba_fractal_Update()
        {

        } //indev fractal

        #endregion

        #region Trying 1d Terrain simulation using Sin function       
        public static void Xsquaredvs2X()
        {
            g = Graphics.FromImage(map);
            g.DrawLine(Pens.Black, 0, middle.Y, drawspace.Width, middle.Y);
            g.DrawLine(Pens.Black, middle.X, 0, middle.X, drawspace.Height);
            PointF[] po1 = new PointF[200];
            PointF[] po2 = new PointF[200];
            PointF[] po3 = new PointF[200];
            PointF[] po4 = new PointF[400];
            PointF[] po5 = new PointF[400];
            PointF[] po6 = new PointF[400];
            PointF[] po7 = new PointF[400];
            PointF[] po8 = new PointF[400];
            float x = 0;
            int seed = sum_of_digits(rand.Next());
            int some = rand.Next(1, 51);
            int c = 0;
            int c1 = rand.Next(6);
            for (int i = 0; i < 400; i++)
            {
                float y = middle.Y - (float)Math.Pow(Math.E, x);
                #region f1
                /*  if (float.IsInfinity(y) || y == float.NaN || i>50)
                      po1[i] = new PointF(middle.X + i, -1);
                  else
                      po1[i] = new PointF(middle.X + i, (int)(middle.Y - (float)Math.Pow(Math.E, x)));
                  po2[i] = new PointF(middle.X + i, middle.Y - 2 * x);
                  po3[i] = new PointF(middle.X + i, middle.Y - (float)Math.Pow(x, 2));
                  po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 20);
                  po5[i] = new PointF(2 * i+5, middle.Y - (float)Math.Sin(x) * 10-10);
                  po6[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 15 - 15);
                  po7[i] = new PointF(2 * i - 10, middle.Y - (float)Math.Sin(x) * 30 - 20);
                  po8[i] = new PointF(i*5, middle.Y - (float)Math.Sin(x) * 50 - 50);*/
                #endregion
                if (c1 == some)
                {
                    c1 = 0;
                    some = rand.Next(1, 51);
                    c = clamper(seed);
                }
                switch (c)
                {
                    case 0: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 20 - 20); break;
                    case 1: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 10 - 10); break;
                    case 2: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 15 - 15); break;
                    case 3: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 30 - 20); break;
                    case 4: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 50 - 50); break;
                    case 5: po4[i] = new PointF(2 * i, middle.Y - (float)Math.Sin(x) * 80 - 80); break;
        }
                c1++;
                x += 0.15f;
            }
            g.DrawLines(Pens.DarkGoldenrod, po4);
            /*  g.DrawLines(Pens.Red, po1);
              g.DrawLines(Pens.Green, po2);
              g.DrawLines(Pens.Blue, po3);
              g.DrawLines(Pens.DarkGoldenrod, po4);
              g.DrawLines(Pens.DarkGoldenrod, po5);
              g.DrawLines(Pens.DarkGoldenrod, po6);
              g.DrawLines(Pens.DarkGoldenrod, po7);
              g.DrawLines(Pens.DarkGoldenrod, po8);*/
            Updatebackground(drawspace, map);

        } //different functions comparison

        public static int sum_of_digits(int nr)
        {
            int n = 0;
            while (nr != 0)
            {
                n = nr % 10;
                nr /= 10;
            }
            return n;
        }

        public static int clamper(int seed)
        {
            if (seed % 2 == 0)
            {
                int x = rand.Next(101);
                if (x % 5 == 0)
                    return 0;
                else
                    return 1;
            }
            else
                return rand.Next(2, 6);
        }
        #endregion

        #region Going Through Space

        public static void DrawStars(int nr)
        {
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            drawspace.BackColor = Color.Black;
            pp = new Starclass[nr];
            tempx = new float[nr];
            tempy = new float[nr];
            acceleration = new float[nr];
            contor = new float[nr];
            index = new float[nr];
            z = 3;
            for (int i = 0; i < nr; i++)
            {
                float angle = (rand.Next(360) * (float)Math.PI) / 180;
                int length = rand.Next(1, 300);
                pp[i] = new Starclass(angle, length);
                g.FillEllipse(Brushes.White, middle.X + (float)Math.Sin(angle) * length, middle.Y + (float)Math.Cos(angle) * length, z, z);
            }
            time.Interval = 15;
            Update_func[Search_tag("drawspace")].torun = true;


        } //call WarpSpace function

        private static void Drawstars_Update()
        {
            g.Clear(Color.Black);
            for (int i = 0; i < pp.Length; i++)
            {
                int px = (int)(drawspace.Width / 2 + (float)Math.Sin(pp[i].angle) * (pp[i].length + acceleration[i]));
                int py = (int)(drawspace.Height / 2 + (float)Math.Cos(pp[i].angle) * (pp[i].length + acceleration[i]));
                int z1 = (int)(z + 0.012f * Math.Abs(py - drawspace.Height / 2));
                // g.FillEllipse(Brushes.White, px - z1 / 2, py - z1 / 2, z1, z1);
                if (index[i] > 0)
                    g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(((int)(index[i] * 100) % 255), rand.Next(256), rand.Next(256), rand.Next(256))), 2), (int)(drawspace.Width / 2 + (float)Math.Sin(pp[i].angle) * (pp[i].length + 0.7f * acceleration[i])), (int)(drawspace.Height / 2 + (float)Math.Cos(pp[i].angle) * (pp[i].length + 0.7f * acceleration[i])), px, py);

                // tempx[i] = px;
                // tempy[i] = py;
                acceleration[i] = (float)Math.Pow(Math.E, Math.Sqrt(contor[i]));
                contor[i] += 0.9f;



                if (px > (drawspace.Width + 120) || py > (drawspace.Height + 120) || (px > (drawspace.Width + 120) && py > (drawspace.Height + 120)) || px < -120 || py < -120 || (px < -120 && py < -120))
                {
                    acceleration[i] = 0;
                    contor[i] = 2.5f;
                    index[i] = -1;
                    float angle = (rand.Next(360) * (float)Math.PI) / 180;
                    int length = rand.Next(1, 300);
                    pp[i] = new Starclass(angle, length);
                }
                index[i]++;
            }
            Updatebackground(drawspace, map);




        } //update for warpspace

        public static void DrawStars_White(int nr)
        {
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            drawspace.BackColor = Color.Black;
            pp = new Starclass[nr];
            tempx = new float[nr];
            tempy = new float[nr];
            acceleration = new float[nr];
            contor = new float[nr];
            index = new float[nr];
            z = 3;
            for (int i = 0; i < nr; i++)
            {
                float angle = (rand.Next(360) * (float)Math.PI) / 180;
                int length = rand.Next(1, 300);
                pp[i] = new Starclass(angle, length);
                g.FillEllipse(Brushes.White, middle.X + (float)Math.Sin(angle) * length, middle.Y + (float)Math.Cos(angle) * length, z, z);
            }
            time.Interval = 15;
            Update_func[Search_tag("drawspace_white")].torun = true;


        } //call WarpSpace function

        private static void Drawstars_White_Update()
        {
            g.Clear(Color.Black);
            for (int i = 0; i < pp.Length; i++)
            {
                int px = (int)(drawspace.Width / 2 + (float)Math.Sin(pp[i].angle) * (pp[i].length + acceleration[i]));
                int py = (int)(drawspace.Height / 2 + (float)Math.Cos(pp[i].angle) * (pp[i].length + acceleration[i]));
                int z1 = (int)(z + 0.012f * Math.Abs(py - drawspace.Height / 2));
                // g.FillEllipse(Brushes.White, px - z1 / 2, py - z1 / 2, z1, z1);
                if (index[i] > 0)
                    g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(((int)(index[i] * 100) % 255), 255, 255, 255)), 2), (int)(drawspace.Width / 2 + (float)Math.Sin(pp[i].angle) * (pp[i].length + 0.7f * acceleration[i])), (int)(drawspace.Height / 2 + (float)Math.Cos(pp[i].angle) * (pp[i].length + 0.7f * acceleration[i])), px, py);

                // tempx[i] = px;
                // tempy[i] = py;
                acceleration[i] = (float)Math.Pow(Math.E, Math.Sqrt(contor[i]));
                contor[i] += 0.9f;



                if (px > (drawspace.Width + 120) || py > (drawspace.Height + 120) || (px > (drawspace.Width + 120) && py > (drawspace.Height + 120)) || px < -120 || py < -120 || (px < -120 && py < -120))
                {
                    acceleration[i] = 0;
                    contor[i] = 2.5f;
                    index[i] = -1;
                    float angle = (rand.Next(360) * (float)Math.PI) / 180;
                    int length = rand.Next(1, 300);
                    pp[i] = new Starclass(angle, length);
                }
                index[i]++;
            }
            Updatebackground(drawspace, map);




        } //update for warpspace
        #endregion

        #region trajectories 

        public static void Trajectory(int interval)
        {
            // time.Stop();
            points.Clear();
            g = Graphics.FromImage(tmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            d = 0;
            // gconst = 0.29419f;
            grav = float.Parse(tb4.Text);
            gconst = (grav / 100) * 3;
            grav = gconst;
            speed = float.Parse(tb2.Text);
            force = float.Parse(tb1.Text);
            angle = float.Parse(tb3.Text);
            time.Interval = interval;
            Update_func[Search_tag("trajectory")].torun = true;


        }//call Trajectory function

        public static void Trajectory_Update()
        {
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            CopyBitmapGdi(tmap, map);
            float py = (float)(d * Math.Tan(angle * rad) - (gconst * Math.Pow(d, 2)) / (2 * Math.Pow(force * Math.Cos(angle * rad), 2)));
            d += speed;
            // gconst += 0.29419f;
            gconst += grav;

            if ((drawspace.Height - 200 - py) <= drawspace.Height - 200)
            {
                points.Add(new Point(50 + (int)d, (int)(drawspace.Height - 200 - py)));
                Point[] pfs = new Point[points.Count];
                for (int i = 0; i < points.Count; i++)
                    pfs[i] = new Point(points[i].X, points[i].Y);
                g.DrawLine(Pens.Black, 0, drawspace.Height - 200, drawspace.Width, drawspace.Height - 200);
                if (pfs.Length > 1)
                    g.DrawCurve(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)), ballsize / 2), pfs);
                g.FillEllipse(Brushes.Chartreuse, 50 + (int)d - ballsize / 2, (int)(drawspace.Height - 200 - py) - ballsize / 2, ballsize, ballsize);
                Updatebackground(drawspace, map);
            }
            else
            {
                g = Graphics.FromImage(tmap);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // points.Add(new Point(50+(int)d , (int)(drawspace.Height - 200 - py)));  
                PointF p1 = points[points.Count - 1];
                PointF p2 = new Point(50 + (int)d, (int)(drawspace.Height - 200 - py));
                PointF p3 = new Point(1, drawspace.Height - 200);
                PointF p4 = new Point(drawspace.Width + 200, drawspace.Height - 200);
                // float ptx = ((p1.X*p2.Y-p1.Y*p2.X)*(p3.X-p4.X)-(p1.X-p2.X)*(p3.X*p4.Y-p3.Y*p4.X))/((p1.X-p2.X)*(p3.Y-p4.Y)-(p1.Y-p2.Y)*(p3.X-p4.X));
                // float pty = ((p1.X*p2.Y-p1.Y*p2.X)*(p3.Y-p4.Y)-(p1.Y-p2.Y)*(p3.X*p4.Y-p3.Y*p4.X))/((p1.X-p2.X)*(p3.Y-p4.Y)-(p1.Y-p2.Y)*(p3.X-p4.X));
                PointF itr = Intersectionof2lines(p1, p2, p3, p4);
                points.Add(new Point((int)itr.X, (int)itr.Y - ballsize / 2));
                Point[] pfs = new Point[points.Count];
                for (int i = 0; i < points.Count; i++)
                    pfs[i] = new Point(points[i].X, points[i].Y);
                g.DrawCurve(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)), ballsize / 2), pfs);
                g.FillEllipse(Brushes.Chartreuse, itr.X - ballsize / 2, itr.Y - ballsize, ballsize, ballsize);
                g.DrawLine(Pens.Black, 0, drawspace.Height - 200, drawspace.Width, drawspace.Height - 200);
                //g.FillRectangle(Brushes.White, 50 + d - acc, drawspace.Height - 199, 200, 200);
                g.DrawString("" + gconst, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 50 + d - speed, drawspace.Height - 180);
                lb1.Items.Add(d);
                Update_func[Search_tag("trajectory")].torun = false;
                Updatebackground(drawspace, tmap);
            }



        } //trajectory update

        public static void Vertical_Trajectory(int nr)
        {
            g = Graphics.FromImage(map);
            //speed = (float.Parse(tb2.Text) / 100) * 1.5f;
            if (toinit)
            {
                ps.Clear();
                speed = float.Parse(tb2.Text);
                grav = (float.Parse(tb4.Text) / 100) * 1.5f;
                tempx = new float[nr];
                tempy = new float[nr];
                acceleration = new float[nr];
                for (int i = 0; i < nr; i++)
                {
                    ps.Add(new ColorPoint(rand.Next(10, drawspace.Width - 9), drawspace.Height - 200, Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))));
                    tempy[i] = (rand.Next((int)speed / 2, (int)speed + 1) / 100) * 1.5f;
                    acceleration[i] = grav;
                    toinit = false;
                }
            }
            else
                for (int i = 0; i < nr; i++)
                {
                    ps[i] = new ColorPoint(ps[i].x, drawspace.Height - 200, ps[i].color);
                    tempy[i] = (rand.Next((int)speed / 3, (int)speed + 1) / 100) * 1.5f;
                    acceleration[i] = grav;
                }
            time.Interval = 30;
            Update_func[Search_tag("vtrajectory")].torun = true;
        } //throwing ball in the air and falling simulation

        public static void Vertical_Trajectory_Update()
        {
            g.Clear(Color.White);
            bool ok = true;
            for (int i = 0; i < ps.Count; i++)
            {
                float py = ps[i].y + (-tempy[i] + acceleration[i]);
                if (py < drawspace.Height - 200)
                {
                    g.DrawLine(Pens.Black, 0, drawspace.Height - 200, drawspace.Width, drawspace.Height - 200);
                    acceleration[i] += grav;
                    g.FillEllipse(new SolidBrush(ps[i].color), ps[i].x - 4, py - 4, 8, 8);
                    ps[i] = new ColorPoint(ps[i].x, (int)py, ps[i].color);
                    ok = false;
                }
                else
                {
                    g.DrawLine(Pens.Black, 0, drawspace.Height - 200, drawspace.Width, drawspace.Height - 200);
                    PointF p1 = new PointF(ps[i].x - 4, py - (-tempy[i] + acceleration[i]) - 4);
                    PointF p2 = new PointF(ps[i].x - 4, py - 4);
                    PointF p3 = new PointF(0, drawspace.Height - 200);
                    PointF p4 = new PointF(drawspace.Width, drawspace.Height - 200);
                    PointF itr = Intersectionof2lines(p1, p2, p3, p4);
                    g.FillEllipse(new SolidBrush(ps[i].color), itr.X, itr.Y - 8, 8, 8);

                }
                //tempx[i] = tempx[i] - acceleration[i];
                //tempy[i] = tempy[i] + tempx[i];
            }
            /*  bool ok = true;
              for (int i = 0; i < ps.Count; i++)
                  if (!isok[i])
                      ok = false;*/
            if (ok)
                Update_func[Search_tag("vtrajectory")].torun = false;
            Updatebackground(drawspace, map);
        } //update for vertical trajectory

        public static void Fireworks(int nr)
        {
            g = Graphics.FromImage(map);
            fpoints = new Firework[nr];
            speed = float.Parse(tb2.Text);
            grav = (float.Parse(tb4.Text) / 100) * 1.5f;
            tempx = new float[nr];
            tempy = new float[nr];
            for (int i = 0; i < nr; i++)
            {
                fpoints[i] = new Firework(new PointF(rand.Next(10, drawspace.Width - 9), drawspace.Height - 200), Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)), rand.Next(8, 15));
                tempy[i] = (rand.Next((int)speed / 2, (int)speed + 1) / 100) * 1.5f;
                acceleration[i] = grav;
            }
            Update_func[Search_tag("fireworks")].torun = true;
        } //indev

        public static void Fireworks_Update(int nr)
        {
            bool ok = true;
            for (int i = 0; i < fpoints.Length; i++)
            {
                float py = fpoints[i].projectile.Y + (-tempy[i] + acceleration[i]);
                if ((tempy[i] - acceleration[i]) >= 0)
                {
                    g.DrawLine(Pens.Black, 0, drawspace.Height - 200, drawspace.Width, drawspace.Height - 200);
                    acceleration[i] += grav;
                    g.FillEllipse(new SolidBrush(fpoints[i].color), fpoints[i].projectile.X - 4, fpoints[i].projectile.Y - 4, 8, 8);
                    fpoints[i].projectile = new PointF(fpoints[i].projectile.X, fpoints[i].projectile.Y);
                    ok = false;
                }
                else
                {
                    if (!fpoints[i].deployed)
                    {
                        for (int j = 0; j < fpoints[i].firepoints.Length; j++)
                        {
                            gconst = (float.Parse(tb4.Text) / 100) * 1.5f;
                            speed = (float)(rand.Next((int)float.Parse(tb2.Text) / 2, (int)float.Parse(tb2.Text) + 1)) / 100;
                            force = float.Parse(tb1.Text);
                            angle = rand.Next(1, 90);
                        }

                        fpoints[i].deployed = true;
                    }
                    else
                    {
             //           for (int j = 0; j < fpoints[i].firepoints.Length; j++)
                    }
                        
                }
            }
        } //indev

        #endregion

        public static void Terrain2D()
        {
            g = Graphics.FromImage(map);
            //List<PointF> lista = new List<PointF>();
            /* PointF[] pt = new PointF[200];
             pt[0] = (new PointF(0, middle.Y - (float)Math.Sin(rand.Next(90)) * rand.Next(1,81)));
             for (int i = 1; i < 200; i++)
                 pt[i] = (new PointF(pt[i - 1].X + rand.Next(10, 50), middle.Y - (float)Math.Sin(rand.Next(90)) * rand.Next(1, 81)));

             g.DrawLines(Pens.Green, pt);*/

            PointF[] pt = new PointF[200];
            pt[0] = (new PointF(0, middle.Y - (float)Math.Sin(rand.Next(90)) * rand.Next(1, 20)));
            for (int i = 1; i < 200; i++)
                pt[i] = (new PointF(pt[i - 1].X + rand.Next(10, 25), pt[i-1].Y - (float)Math.Sin(rand.Next(90)) * rand.Next(1, 20)));
            g.DrawLines(Pens.Green, pt);
            Updatebackground(drawspace, map);         
        }

        public static void Nvertices_polygon()
        {
            g = Graphics.FromImage(map);
            Point[] pointts = new Point[rand.Next(12, 31)];
            pointts[0]=new Point(rand.Next(0, drawspace.Width / 3), rand.Next(drawspace.Height / 3, drawspace.Height / 2));
            for (int i = 1; i < pointts.Length; i++)
                pointts[i] = new Point(pointts[i - 1].X + rand.Next(5, 21), rand.Next(drawspace.Height / 3, drawspace.Height / 2+ drawspace.Height / 3));
            for (int i = 0; i < pointts.Length; i++)
                g.DrawEllipse(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))),pointts[i].X-2,pointts[i].Y+2,5,5);
            for (int i = 0; i < pointts.Length - 1; i++)
                g.DrawLine(Pens.CadetBlue, pointts[i], pointts[i + 1]);
            Updatebackground(drawspace, map);
        } 

        public static bool IsPrime(int nr)
        {
            if (nr <= 1 || (nr % 2 == 0 && nr != 2))
                return false;
            if (nr == 2)
                return true;
            int x = 3;
            while(x<=nr/2)
            {
                if (nr % x == 0)
                    return false;
                x += 2;
            }
            return true;
        }

        public static void ShowIfPrime(int nr)
        {
            if (IsPrime(nr) == true)
                MessageBox.Show("It's a prime");
            else
                MessageBox.Show("It's not a prime");
        }

        public static void Combobox_indexchanged(object sender, EventArgs e)
        {
            StopAll_Action();
            ToCall[(sender as ComboBox).SelectedIndex].torun = true;
        } //enabling the function specificed by the combobox index 

        public static int Search_tag(string tag)
        {
            for (int i = 0; i < Update_func.Count; i++)
                if (Update_func[i].tag == tag)
                    return i;
            return 0;
        }// searching for a specific tag in the myaction class;

        public static void StopAll_Action()
        {
            foreach (MyAction i in ToCall)
                i.torun = false;
            foreach (MyAction i in Update_func)
                i.torun = false;
        }//disabling anyfunctions that are running or are able to be called out

        public static void CopyBitmapGdi(Bitmap source, Bitmap dest)
        {
            Rectangle bounds = new Rectangle(0, 0, source.Width, source.Height);
            BitmapData sdata = source.LockBits(bounds, ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData ddata = dest.LockBits(bounds, ImageLockMode.WriteOnly, dest.PixelFormat);

            IntPtr ptrs = sdata.Scan0;
            IntPtr ptrd = ddata.Scan0;

            int bytes1 = Math.Abs(sdata.Stride) * source.Height;
            byte[] rgbValues1 = new byte[bytes1];

            Marshal.Copy(ptrs, rgbValues1, 0, bytes1);
            Marshal.Copy(rgbValues1, 0, ptrd, bytes1);
            source.UnlockBits(sdata);
            dest.UnlockBits(ddata);

        }

        public static PointF Intersectionof2lines(Point p1, Point p2, Point p3, Point p4)
        {
            float ptx = ((p1.X * p2.Y - p1.Y * p2.X) * (p3.X - p4.X) - (p1.X - p2.X) * (p3.X * p4.Y - p3.Y * p4.X)) / ((p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X));
            float pty = ((p1.X * p2.Y - p1.Y * p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X * p4.Y - p3.Y * p4.X)) / ((p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X));
            return new PointF(ptx, pty);
        }

        public static PointF Intersectionof2lines(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float ptx = ((p1.X * p2.Y - p1.Y * p2.X) * (p3.X - p4.X) - (p1.X - p2.X) * (p3.X * p4.Y - p3.Y * p4.X)) / ((p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X));
            float pty = ((p1.X * p2.Y - p1.Y * p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X * p4.Y - p3.Y * p4.X)) / ((p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X));
            return new PointF(ptx, pty);
        }

        public static void Updatebackground(PictureBox pb, Bitmap map)
        {
            pb.BackgroundImage = null;
            pb.BackgroundImage = map;
        }

        public static void Clearbackground()
        {
            StopAll_Action();
            ToCall[cb1.SelectedIndex].torun = true;
            g = Graphics.FromImage(tmap);
            g.Clear(Color.White);
            g = Graphics.FromImage(map);
            g.Clear(Color.White);
            toinit = true;
            points.Clear();
            gconst = 0.29419f;
            d = 0;
            lb1.Items.Clear();
            if (acceleration != null)
                for (int i = 0; i < acceleration.Length; i++)
                    acceleration[i] = 0;

            if (contor != null)
                for (int i = 0; i < contor.Length; i++)
                    contor[i] = 0;

            if (index != null)
                for (int i = 0; i < index.Length; i++)
                    index[i] = 0;

            drawspace.BackgroundImage = null;
            drawspace.BackgroundImage = map;


        }

    }

    public class Starclass
    {
        public float angle;
        public float length;

        public Starclass()
        {
            angle = 0;
            length = 0;
        }

        public Starclass(float angle, float length)
        {
            this.angle = angle;
            this.length = length;
        }

    }

    public class PointM
    {
        float angle;
        float x;
        float y;

        public PointM()
        {
            angle = 0;
            x = 0;
            y = 0;

        }

        public PointM(float angle, float x, float y)
        {
            this.angle = angle;
            this.x = x;
            this.y = y;
        }

        public Point setpoint()
        {
            return new Point((int)x, (int)y);
        }
    }

    public class ColorPoint
    {
        public float x;
        public float y;
        public Color color;

        public ColorPoint(float x, float y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }

    public class MyAction
    {
        public Action method;
        public bool torun;
        public string tag;

        public MyAction(Action method)
        {
            this.method = method;
            torun = true;
            tag = "";
        }

        public MyAction(Action method, bool torun)
        {
            this.method = method;
            this.torun = torun;
            tag = "";
        }

        public MyAction(Action method, bool torun, string tag)
        {
            this.method = method;
            this.torun = torun;
            this.tag = tag;
        }

    }

    public class Firework
    {
        public PointF projectile;
        public Color color;
        public Spoint[] firepoints;
        public bool deployed;

        public Firework(PointF projectile, Color color, int nr)
        {
            this.projectile = projectile;
            this.color = color;
            deployed = false;
            firepoints = new Spoint[nr];
        }
    }

    public class Spoint
    {
        public float x;
        public float y;
        public float angle;
        public float gravity;
        public float speed;
        public float force;

        public Spoint()
        {
            x = 0;
            y = 0;
            angle = 0;   
            speed = 5;
            force = 100;
            gravity = 9.8f;
        }

        public Spoint(float x,float y)
        {
            this.x = x;
            this.y = y;
            angle = 0;
            speed = 5;
            force = 100;
            gravity = 9.8f;
        }
        
        public Spoint(float x,float y,float angle)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            speed = 5;
            force = 100;
            gravity = 9.8f;
        }

        public Spoint(float x, float y, float angle,float speed)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.speed = speed;
            force = 100;
            gravity = 9.8f;
        }

        public Spoint(float x, float y, float angle, float speed,float force)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.speed = speed;
            this.force = force;
            gravity = 9.8f;
        }

        public Spoint(float x, float y, float angle, float speed, float force,float gravity)
        {
            this.x = x;
            this.y = y;
            this.angle = angle;
            this.speed = speed;
            this.force = force;
            this.gravity = gravity;
        }
    }

    public struct Vector2D
    {
        float x;
        float y;

        public void Create_Vector2D(float x,float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Polynom2
    {
        float x;
        float a;
        float b;


        public void SetPolynom(float a,float x,float b)
        {
            this.x = x;
            this.a = a;
            this.b = b;
        }

        public float[] results()
        {
            float delta =(b * b - 4 * a);
            if (delta > 0)
                return new float[] { (-b - (float)Math.Sqrt(delta)) / (2 * a), (-b + (float)Math.Sqrt(delta)) / (2 * a) };
           // if (delta == 0)
                return new float[] { -b / (2 * a) };
           // return new float[] { (-b - (float)Math.Sqrt(-delta)) / (2 * a), (-b + (float)Math.Sqrt(-delta)) / (2 * a) };
        }
    }

}
