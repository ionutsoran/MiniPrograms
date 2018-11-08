using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _255_shaders_of_gray
{
    public partial class Form1 : Form
    {
        Image img;//=Image.FromFile(@"C:\Users\Inner\Desktop\shaorma.jpg");
        Random rand = new Random();
        Bitmap bm;
        Graphics g;
        public Form1()
        {
            InitializeComponent();
            Engine.Initialize(pictureBox1,listBox1,textBox4,textBox5,textBox6,textBox7,comboBox1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            // Gradiend_Gray();
            // Radial_gradient();
            // Radial_gradient1();
            // Mod_img();
            //Input_Color();
            Engine.Start();
        }

        public void Gradiend_Gray()
        {
            bm = new Bitmap(700, 700);
            g = Graphics.FromImage(bm);
            int x = 0;
            for (int i = 0; i <= 255; i++)
            {
                g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(i, i, i))), x, 0, x, 500);
                g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(i, i, i))), x + 1, 0, x+1, 500);
                x += 2;
            }
            pictureBox1.BackgroundImage = bm;
        }

        public void Radial_gradient()
        {
            bm = new Bitmap(700, 700);
            g = Graphics.FromImage(bm);
            int x = 0;
            for (int i = 0; i <= 255; i++)
            {

                g.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(i, i, i))), pictureBox1.Width / 2 - x / 2, pictureBox1.Height / 2 - x / 2, x, x);
                g.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(i, i, i))), pictureBox1.Width / 2 - x / 2, pictureBox1.Height / 2 - x / 2, x+1, x+1);
                x +=2;
            }
            pictureBox1.BackgroundImage = bm;
        }

        public void Radial_gradient1()
        {
            List<Point> list = new List<Point>();
            bm = new Bitmap(700, 700);
            g = Graphics.FromImage(bm);
            int x = 0;
            double rad = Math.PI / 180;
            for(int j=0;j<256;j++)
            {
                float k = 0.2f;
                for (float i = 0; i < 360; i+=k)
                {
                    int pozx = (int)(x * Math.Sin(i * rad));
                    int pozy = (int)(x * Math.Cos(i * rad));
                    int pozx1 = (int)((x+1) * Math.Sin(i * rad));
                    int pozy1 = (int)((x+1) * Math.Cos(i * rad));
                    // list.Add(new Point(pictureBox1.Width / 2 + pozx, pictureBox1.Height / 2 + pozy));
                    g.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(j, j, j))), pictureBox1.Width / 2 + pozx, pictureBox1.Height / 2 + pozy, 1, 1);
                    //g.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(j, j, j))), pictureBox1.Width / 2 + pozx1, pictureBox1.Height / 2 + pozy1, 1, 1);
                }
                x++;

            }
                
            pictureBox1.BackgroundImage = bm;

        }

        public void Mod_img()
        {
            Bitmap bm1 = new Bitmap(1024, 768);
            bm = (Bitmap)img;
            for (int i = 0; i < 768; i++)
                for (int j = 0; j < 1024; j++)
                    bm1.SetPixel(j, i, bm.GetPixel(j, i));

                    for (int i=0;i<768;i++)
                for(int j=0;j<1024;j++)
                {
                    Color cl=bm1.GetPixel(j, i);
                    /*  if (cl.B > 0)
                          if(cl.R+60<256&&cl.G-60>=0)
                              cl = Color.FromArgb(cl.A, cl.R+60, cl.G-60, 0);
                          else
                              cl = Color.FromArgb(cl.A, cl.R, cl.G, 0);*/
                    //cl = Color.FromArgb(cl.A, 255 - cl.R, 255 - cl.G, 255 - cl.B);
                    //cl = Color.FromArgb(cl.A, 255 - cl.G, 255 - cl.B, 255 - cl.R);
                    //cl = Color.FromArgb(cl.A, cl.G, cl.B, cl.R);
                    // cl = Color.FromArgb(cl.A, cl.G, cl.R ,cl.B);
                    // cl = Color.FromArgb(cl.A, cl.G, cl.G, cl.G);
                    // cl = Color.FromArgb(cl.A, cl.R, cl.R, cl.R);
                    // cl = Color.FromArgb(cl.A, cl.B, cl.B, cl.B);
                    float m =((cl.R + cl.G + cl.B )/ 3);
                    float difR = Math.Abs(cl.R - m);
                    float difG = Math.Abs(cl.G - m);
                    float difB = Math.Abs(cl.B - m);
                    float mr;
                    float mg;
                    float mb;
                    if (m > cl.R)
                        mr = cl.R + ((difR / 100) * hScrollBar1.Value);
                    else
                        mr = cl.R - ((difR / 100) * hScrollBar1.Value);

                    if (m > cl.G)
                        mg = cl.G + ((difG / 100) * hScrollBar1.Value);
                    else
                        mg = cl.G - ((difG / 100) * hScrollBar1.Value);

                    if (m > cl.B)
                        mb = cl.B + ((difB / 100) * hScrollBar1.Value);
                    else
                        mb = cl.B - ((difB / 100) * hScrollBar1.Value);

                    cl = Color.FromArgb(cl.A, (int)mr, (int)mg, (int )mb);
                    bm1.SetPixel(j, i, cl);

                }
            pictureBox1.BackgroundImage = bm1;
        }

        public void Input_Color()
        {
            bm = new Bitmap(700, 700);
            g = Graphics.FromImage(bm);
            int red = int.Parse(textBox1.Text);
            int greed = int.Parse(textBox2.Text);
            int blue = int.Parse(textBox3.Text);
            g.FillRectangle(new SolidBrush(Color.FromArgb(red, greed, blue)), 0, 0, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.BackgroundImage = bm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Engine.Clearbackground();
        }
    }
}
