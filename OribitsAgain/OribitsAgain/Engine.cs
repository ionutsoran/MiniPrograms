using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OribitsAgain
{
    public struct Planet //strucuta pentru a ingloba campii unei planete
    {
        public PointF loc;
        public float raza;
        public Planet(PointF loc,float raza)
        {
            this.loc = loc;
            this.raza = raza;
        }
    }


    public static class Engine
    {
        public static Graphics g;
        public static Bitmap map;
        public static PointF center;
        public static Random rand;
        public static PictureBox box;
        public static Timer timer;
        public static float t = 0;
        public static List<Planet> plantes;//lista cu datele planetelor
        public static ListBox listb;
        public static int nr1, nr2, nr3;//astea sunt doar pentru culorii ca sa nu fie random tot timpu

        public static void Init(PictureBox box1,Timer timer1,ListBox listb1)
        {
            listb = listb1;
            timer = timer1;
            box = box1;
            map = new Bitmap(box1.Width, box1.Height);
            g = Graphics.FromImage(map);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            rand = new Random();
            timer.Interval = 50;
            timer.Tick += new EventHandler(timer_tick);
            plantes = new List<Planet>(); 
            nr1 = rand.Next(256);
            nr2 = rand.Next(256);
            nr3 = rand.Next(256);
        }

        public static void Start()
        {
            center = new PointF(map.Width / 2, map.Height / 2);
            timer.Start();
            box.Image = map;
        }

        public static void GenerateElipses(PointF poz,float raza,float t1,int nr)//apelul recu
        {
            if(raza>1)
            {
                PointF f = new PointF(poz.X + (float)Math.Cos(t1) * (6 * raza), poz.Y + (float)Math.Sin(t1) * (3.5f * raza));
                AddtoList(f, raza);
                t1 = t1 *(float)Math.E;
                DrawElipse(f, raza,nr);
                GenerateElipses(f, raza/2,t1,nr+1);
            }
             

        }

        public static void DrawElipse(PointF poz,float raza,int i)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb((nr1*i)%255, (nr2 * i) % 255,(nr3 * i) % 255)), poz.X - raza / 2, poz.Y - raza / 2, raza, raza);//desenez elipsa cu centrul in punctul poz
        }

        public static void timer_tick(object sender,EventArgs e)
        {
            // listb.Items.Clear();
            if (listb.Items.Count >= 30)
                listb.Items.Clear();
            plantes.Clear(); //curat lista de planete
            if (t >= 360)
                t = 0;
            g.Clear(Color.White);
            DrawElipse(center, 100,3);//desene prima planeta
            plantes.Add(new Planet(center, 100));//adaug prima planeta in lista
            GenerateElipses(center,50,t,2);//calculez si desenez locatia planetelor
            Collisions();//detectez coliziunile la momenutul respecitv
            box.Image = map;
            t += 0.005f;//cresc variabila (0,2pi)
            
        }

        public static void AddtoList(PointF poz,float raza)
        {
            plantes.Add(new Planet(poz,raza));
        }

        public static void Collisions()
        {
            for (int i = 0; i < plantes.Count; i++)
                for (int j = i + 1; j < plantes.Count; j++)
                {
                    g.DrawLine(Pens.Red, plantes[i].loc, plantes[j].loc);
                    if (Dist2P(plantes[i].loc, plantes[j].loc) <= (plantes[i].raza/2 + plantes[j].raza/2))//verific daca dista este mai mica decat suma razelor si da razele ca si parametru sunt defapt diametre
                        listb.Items.Add("Planet:" + i + " collided with Planet:" + j);
                }        
        }

        public static PointF CalcCenter(PointF poz,float raza)
        {
            return new PointF(poz.X, poz.Y);
        }//functia asta e degeaba oricum eu cand desenez puncul de start e fix centrul deci nu mai trebe calculat

        public static float Dist2P(PointF p1,PointF p2)//dista dintre 2 puncte in plan
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }

   
}
