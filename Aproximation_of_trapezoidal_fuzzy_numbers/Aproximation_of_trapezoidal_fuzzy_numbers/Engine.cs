using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Aproximation_of_trapezoidal_fuzzy_numbers
{
    public static class Engine
    {
        public static Graphics g;
        public static Bitmap drawspace;
        public static PictureBox background;
        public static TextBox a1t, a2t, a3t, a4t;
        public static Label t1t, t2t, t3t, t4t;
        public static double a1, a2, a3, a4;
        public static double r;
        public static double t1, t2, t3, t4;
        public static double condition1,condition2,condition3;
        public static List<PointF> pointstodraw_left,pointstodraw_right;
        public static Random rand;
        public static ComboBox cb;
        public static string[] cb_indexes;
        public static int cb_selected_index;
        public static int offset;
        public static int incre;
        public static Color cl;


        public static void Start()
        {
            Drawgraph1();
            cl = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            Draw_aprox1(cl);
        }

        public static void Initialize(ContainerControl parent)
        {
            //
            a1t = (parent as Form1).textBox1;
            a2t = (parent as Form1).textBox2;
            a3t = (parent as Form1).textBox3;
            a4t = (parent as Form1).textBox4;
            t1t = (parent as Form1).label6;
            t2t = (parent as Form1).label7;
            t3t = (parent as Form1).label8;
            t4t = (parent as Form1).label9;
            //

            //
            cb = (parent as Form1).comboBox1;
            cb_indexes = new string[3];
            cb_indexes[0] = "Trapezoidal Aproximation preserving expected interval";
            cb_indexes[1] = "Trapezoidal Aproximation preserving ambiguity and value";
            cb_indexes[2] = "Nearest Interval,Trapezoidal &Triangular Aproxiamtion preserving ambiguity";
          
            for(int i=0;i<cb_indexes.Length;i++)
            {
                cb.Items.Add(cb_indexes[i]);
            }

            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.SelectedIndex = 0;
            cb_selected_index = 0;

            //

            //
            pointstodraw_left = new List<PointF>();
            pointstodraw_right = new List<PointF>();
            //

            //
            background = (parent as Form1).pictureBox1;
            drawspace = new Bitmap(background.Width,background.Height);
            g = Graphics.FromImage(drawspace);
            //


            //

            offset = 35;
            //

            //

            incre = 0;
            //

            //

            rand = new Random();
            //
        }

        public static void get_input(string _a1,string _a2,string _a3,string _a4,string _r)
        {
            try
            {
                if (_a1 != null || _a1 != string.Empty || _a1 != "")
                    a1 = int.Parse(_a1);
                else
                    MessageBox.Show("a1 is empty!");
            }
            catch(ArgumentException e)
            {
                MessageBox.Show("a1 is not a valid number! :" + e.ToString());
            }
         

            try
            {
                if (_a2 != null || _a2 != string.Empty || _a2 != "")
                    a2 = int.Parse(_a2);
                else
                    MessageBox.Show("a2 is empty!");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("a2 is not a valid number! :" + e.ToString());
            }

            try
            {
                if (_a3 != null || _a3 != string.Empty || _a3 != "")
                    a3 = int.Parse(_a3);
                else
                    MessageBox.Show("a3 is empty!");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("a3 is not a valid number! :"+e.ToString());
            }

            try
            {
                if (_a4 != null || _a4 != string.Empty || _a4 != "")
                    a4 = int.Parse(_a4);
                else
                    MessageBox.Show("a4 is empty");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("a4 is not a valid number! :"+e.ToString());
            }

            try
            {
                if (_r != null || _r != string.Empty || _r != "")
                    r = int.Parse(_r);
                else
                    MessageBox.Show("r empty");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("r is not a valid number! :" + e.ToString());
            }
        }

        public static void check_condition1()
        {
            // condition1 = (float)((a4 / 3) - (a1 / 3) - ((a4 - a3) *r)/(3*(r+1)) - (((a2 - a1)) * r)/(3*(r+1)) - a4 + a1 + ((a4 - a3) *r)/(r * 2 + 1) + ((a2 - a1) * r) / (2 * r + 1));
            condition1 = (5 * r + 1) * a1 + (2 * r) * (r - 1) * a2 - 2 * r * (r + 2) * a3 + (r - 1) * a4;
            if(condition1>0)
            {
                t1 = (a1 + r*a2) / (1 + r);
                t2 = t3 = t1;
                t4 = (-a1 - r*a2 + 2*r*a3 + 2*a4)/ (1 + r);
                //MessageBox.Show("t1="+t1+" ;t2="+t2+" ;t3="+t3+" ;t4="+t4);
                
            }

            condition2 = (1 - r) * a1 + 2 * r * (r + 2) * a2 - 2 * r * (r - 1) * a3 - (5 * r + 1) * a4;
            if(condition2>0)
            {
                t1 = (2 * a1 + 2 * r * a2 - r * a3 - a4) / (1 + r);
                t2 = (a4 + r * a3) / (1 + r);
                t3 = t4 = t2;
                //MessageBox.Show("t1=" + t1 + " ;t2=" + t2 + " ;t3=" + t3 + " ;t4=" + t4);
               
            }

            condition3= (1 - r)*a1 + 2*r*(r + 2)*a2 - 2 * r * (r + 2) * a3 + (r - 1) * a4;

            if(condition1<=0&&condition2<=0&&condition3>0)
            {
                t1 = ((9 * r + 3) * a1 + 6 * Math.Pow(r, 2) * a2 - 2 * r * (r + 2) * a3 + (r - 1) * a4) / (2 * (1 + r) * (1 + 2 * r));
                t2 = ((1 - r) * a1 + 2 * r * (r + 2) * a2 + 2 * r * (r + 2) * a3 + (1 - r) * a4) / (2*(1 + r)*(1 + 2*r));
                t3 = t2;
                t4 = ((r - 1) * a1 - 2 * r * (r + 2) * a2 + 6 * Math.Pow(r, 2) * a3 + (9 * r + 3) * a4) / (2*(1 + r)*(1 + 2*r));
                //MessageBox.Show("t1=" + t1 + " ;t2=" + t2 + " ;t3=" + t3 + " ;t4=" + t4);
               

            }
            //MessageBox.Show(condition1+" ");
            if(condition3<=0)
            {
                t1 = ((5 * r + 1) * a1 + 2 * r * (r - 1) * a2) / ((1 + r)*(1 + 2*r));
                t2 = ((1 - r) * a1 + 2 * r * (r + 2) * a2) / ((1 + r)*(1 + 2*r));
                t3 = (2 * r * (r + 2) * a3 + (1 - r) * a4) / ((1 + r)*(1 + 2*r));
                t4 = (2 * r * (r - 1) * a3 + (5 * r + 1) * a4) / ((1 + r)*(1 + 2*r));
                //MessageBox.Show("t1=" + t1 + " ;t2=" + t2 + " ;t3=" + t3 + " ;t4=" + t4);
              
            }
            change_Tparams();
        }

        public static void check_condition2()
        {
            condition1 = (1 - 5) * (a1 - a4) + 2 * r * (r + 2) * (a2 - a3);
            if(condition1<=0)
            {
                t1 = ((5 * r + 1) * a1 + 2 * r * (r - 1) * a2) / ((1 + r) * (1 + 2 * r));
                t2 = ((1 - r) * a1 + 2 * r * (r + 2) * a2) / ((1 + r) * (1 + 2 * r));
                t3 = (2 * r * (r + 2) * a3 + (1 - r) * a4) / ((1 + r) * (1 + 2 * r));
                t4 = (2 * r * (r - 1) * a3 + (5 * r + 1) * a4) / ((1 + r) * (1 + 2 * r));
            }
            condition2 = (1 - r) * a1 + (2 * Math.Pow(r, 2) + 4 * r) * a2 - 2 * Math.Pow(r, 2) * a3 - (3 * r + 1) * a4;
            if(condition2>0)
            {
                t1 = (3 * a1 - 2 * a4 + 6 * r * a2 - 4 * r * a3) / (1 + 2 * r);
                t2 = (a4 + 2 * r * a3) / (1 + 2 * r);
                t3 = t4 = t2;
            }

            condition3 = (3 * r + 1) * a1 + 2 * Math.Pow(r, 2) * a2 - (2 * Math.Pow(r, 2) + 4 * r) * a3 + (r - 1) * a4;
            if(condition3>0)
            {
                t1 = (a1 + 2 * r * a2) / (1 + 2 * r);
                t2 = t3 = t1;
                t4 = (-2 * a1 + 3 * a4 - 4 * r * a2 + 6 * r * a3) / (1 + 2 * r);
            }
            if(condition1>0&&condition2<=0&&condition3<=0)
            {
                t1 = ((2 + 4 * r) * a1 + (2 * r + 4 * Math.Pow(r, 2)) * a2 - (2 * Math.Pow(r, 2) + 4 * r) * a3 + (r - 1) * a4) / ((1 + 2 * r) * (1 + r));
                t2 = ((1 - r) * a1 + (4 * r + 2 * Math.Pow(r, 2)) * a2 + (4 * r + 2 * Math.Pow(r, 2)) * a3 + (1 - r) * a4) / (2*((1 + 2 * r) * (1 + r)));
                t3 = t2;
                t4 = ((r - 1) * a1 - (4 * r + 2 * Math.Pow(r, 2)) * a2 + (4 * Math.Pow(r, 2) + 2 * r) * a3 + (2 + 4 * r) * a4) / ((1 + 2 * r) * (1 + r));
            }

            change_Tparams();
        }

        public static void check_condition3()
        {
            double condition1 = (r - 1) * a1 - 2 * r * (r + 2) * a2 + 2 * r * (r + 2) * a3 - (r + 1) * a4;
            if(condition1>=0)
            {
                t1 = ((5 * r + 1) * a1 + 2 * r * (r - 1) * a2) / ((1 + 2 * r) * (r + 1));
                t2 = ((1 - r) * a1 + 2 * r * (r + 2) * a2) / ((1 + 2 * r) * (r + 1));
                t3 = (2 * r * (r + 2) * a3 + (1 - r) * a4) / ((1 + 2 * r) * (r + 1));
                t4 = (2 * r * (r - 1) * a3 + (5 * r + 1) * a4) / ((1 + 2 * r) * (r + 1));
            }

            double condition2 = (1 - r) * a1 + 2 * r * (r + 2) * a2 - 2 * Math.Pow(r, 2) * a3 - (3 * r + 1) * a4;
            if(condition2>0)
            {
                t1 = ((13 * r + 11) * a1 + 2 * r * (11 * r + 10) * a2 - 2 * r * (7 * r + 8) * a3 - (5 * r + 7) * a4) / (4 * (1 + 2 * r) * (r + 1));
                t2 = ((r - 1) * a1 - 2 * r * (r + 2) * a2 + 2 * r * (5 * r + 4) * a3 + (7 * r + 5) * a4) / (4 * (1 + 2 * r) * (r + 1));
                t4 = t3 = t2;
            }
            double condition3 = -(3 * r + 1) * a1 - 2 * Math.Pow(r, 2) * a2 + 2 * r * (r + 2) * a3 - (r - 1) * a4;
            if(condition3<0)
            {
                t1 = ((7 * r + 5) * a1 + 2 * r * (5 * r + 4) * a2 - 2 * r * (r + 2) * a3 + (r - 1) * a4)/ (4 * (1 + 2 * r) * (r + 1));
                t3 = t2 = t1;
                t4 = (-(5 * r + 7) * a1 - 2 * r * (7 * r + 8) * a2 + 2 * r * (11 * r + 10) * a3 + (13 * r + 11) * a4)/ (4 * (1 + 2 * r) * (r + 1));
            }
            if(condition1<0&&condition2<=0&&condition3>=0)
            {
                t1 = (4 * (2 * r + 1) * a1 + 4 * r * (2*r +1) * a2 - 4 * r * (r + 2) * a3 + 2 * (r - 1) * a4)/ (2 * (1 + 2 * r) * (r + 1));
                t3 = t2 = ((1 - r) * a1 + 2 * r * (r + 2) * a2 + 2 * r * (r + 2) * a3 + (1 - r) * a4)/ (2 * (1 + 2 * r) * (r + 1));
                t4 = (2 * (r - 1) * a1 - 4 * r * (r + 2) * a2 + 4 * r * (2 * r + 1) * a3 + 4 * (2 * r + 1) * a4)/ (2 * (1 + 2 * r) * (r + 1));
            }
            change_Tparams();
        }

        public static void change_Tparams()
        {
            t1t.Visible = true;
            t2t.Visible = true;
            t3t.Visible = true;
            t4t.Visible = true;
            t1t.Text = "t1=" + t1;
            t2t.Text = "t2=" + t2;
            t3t.Text = "t3=" + t3;
            t4t.Text = "t4=" + t4;
        }


        public static void Drawgraph1()
        {
            pointstodraw_left.Clear();
            pointstodraw_right.Clear();

            g.FillRectangle(Brushes.Black, 0, 0, background.Width, background.Height);
            for (int i = 0; i < background.Width / 80; i++)
            {
                g.DrawLine(Pens.White, 35 + i * 80, background.Height - 15, 35 + i * 80, background.Height);
                g.DrawString("" + (incre + (i - 1)), new Font("Arial", 10, FontStyle.Bold), Brushes.White, 40 + i * 80, background.Height - 15);
                g.DrawLine(Pens.White, 0, background.Height - 15, background.Width, background.Height - 15);
            }


            double k=0;
          
            if((a2-a1)>0)
                k = 1 / (80 * (a2 - a1));
            int j = 1;

            int c = 0;

            pointstodraw_left.Add(new PointF(offset+(float)(a1+1)*80, background.Height - 15));

            for(double alpha=k;alpha<1;alpha+=k)
            {
                
                double Al = (Math.Pow(alpha, 1f / r) * 100);

                //g.DrawEllipse(Pens.DarkGray, offset + (float)((a1+1)* 80) + j, background.Height - (float)Al-15, 1, 1);

                //if (c % 3 == 0)
                    pointstodraw_left.Add(new PointF(offset + (float)((a1 + 1) * 80) + j, background.Height - (float)Al - 15));
                j++;
            }

            g.DrawLine(Pens.DarkGray, offset + (float)((a2+1) * 80), background.Height - 115, offset + (float)((a3+1) * 80), background.Height - 115);

            j = 1;

            if ((a4 - a3) > 0)
                k = 1 / (80 * (a4 - a3));

            c = 0;
            


            for (double alpha=1-k; alpha>k;alpha-=k)
            {
                double Au = (Math.Pow(alpha, 1f / r)) * 100;

                ///g.DrawEllipse(Pens.DarkGray, offset + (float)((a4 + 1) * 80 - j), background.Height - (100 - (float)Au) - 15, 1, 1);

               // if (c % 3 == 0)
                    pointstodraw_right.Add(new PointF(offset + (float)((a4 + 1) * 80 - j), background.Height - (100 - (float)Au) - 15));
                j++;
            }
            pointstodraw_right.Add(new PointF(offset + (float)(a3 + 1) * 80, background.Height - 115));

            g.DrawCurve(Pens.DarkGray, pointstodraw_left.ToArray());
            g.DrawCurve(Pens.DarkGray, pointstodraw_right.ToArray());
            Update_background();
        }

        public static void Draw_aprox1(Color cl)
        {
          
            g.DrawLine(new Pen(cl), offset + (float)(t1 + 1) * 80f, background.Height - 15, offset + (float)(t2 + 1) * 80f, background.Height - 115);
            g.DrawLine(new Pen(cl), offset + (float)(t2 + 1) * 80f, background.Height - 115, offset + (float)(t3 + 1) * 80f, background.Height - 115);
            g.DrawLine(new Pen(cl), offset + (float)(t3 + 1) * 80f, background.Height - 115, offset + (float)(t4 + 1) * 80f, background.Height - 15);

            Update_background();
        }
     
        public static void Update_background()
        {
            background.BackgroundImage = null;
            background.BackgroundImage = drawspace;
        }
        
    }

    public class Fraction
    {
        public int a;
        public int b;

        public Fraction(int a,int b)
        {
            this.a = a;
            this.b = b;
        }
    }

/*
    public static void drawgraph() //MAI TREBE LUCRAT!
    {
        pointstodraw_left.Clear();
        pointstodraw_right.Clear();
        g.Clear(Color.White);
        for (int i = 0; i < ((background.Width - background.Width / 4f) / 80f); i++)
        {
            g.DrawLine(Pens.Blue, background.Width / 4f + i * 80f, background.Height - 10, background.Width / 4f + i * 80f, background.Height);
        }


        double Al;
        double Au;
        int j = 0;
        for (double alpha = 0; alpha <= 1; alpha += 1 / (((a2 - a1) * 80f) * 6))
        {
            Al = a1 + (a2 - a1) * Math.Pow(alpha, 1 / r);
            if ((a2 - a1) == 1)
            {
                //g.DrawEllipse(Pens.Black, background.Width / 4f + (j/6), background.Height - ((float)Al - (int)Al) * 100, 1, 1);
                pointstodraw_left.Add(new PointF(offset + background.Width / 4f + (j / 6), background.Height - ((float)Al - (int)Al) * 100));
            }

            else
            {
                //g.DrawEllipse(Pens.Black, background.Width / 4f + (j/6), background.Height - (100 * (float)Al) / (float)a2, 1, 1);
                pointstodraw_left.Add(new PointF(offset + background.Width / 4f + (j / 6), background.Height - (100 * (float)Al) / (float)a2));
            }

            //  MessageBox.Show((Al)+"    "+(background.Height-(float)Al)+"    "+background.Height);
            j++;
        }

        g.DrawLine(Pens.Black, offset + background.Width / 4f + (float)((a2 - 1) * 80), background.Height - 100, offset + background.Width / 4f + (float)((a3 - 1) * 80), background.Height - 100);
        j = 0;

        for (double alpha = 1 / (((a4 - a3) * 80f) * 6); alpha <= 1; alpha += 1 / (((a4 - a3) * 80f) * 6))
        {
            Au = a4 - (a4 - a3) * Math.Pow(alpha, 1 / r);
            if ((a4 - a3) == 1)
            {
                //g.DrawEllipse(Pens.Black, background.Width / 4f + (float)((a3 - 1) * 80f) + (j/6), background.Height - ((float)Au - (int)Au) * 100, 1, 1);
                pointstodraw_right.Add(new PointF(offset + background.Width / 4f + (float)((a3 - 1) * 80f) + (j / 6), background.Height - ((float)Au - (int)Au) * 100));
            }
            else
            {
                // g.DrawEllipse(Pens.Black, background.Width / 4f + (float)((a3 - 1) * 80f) + (j/6), background.Height - (100 * (float)Au) / (float)a4, 1, 1);
                pointstodraw_right.Add(new PointF(offset + background.Width / 4f + (float)((a3 - 1) * 80f) + (j / 6), background.Height - (100 * (float)Au) / (float)a4));
            }

            //MessageBox.Show("" + Au);
            j++;
        }

        g.DrawCurve(Pens.Black, pointstodraw_left.ToArray());
        g.DrawLine(Pens.Black, offset + background.Width / 4f + (float)((a3 - 1) * 80), background.Height - 100, pointstodraw_right[0].X, pointstodraw_right[0].Y);
        // g.DrawLine(Pens.Black, pointstodraw_right[pointstodraw_right.Count-1].X, pointstodraw_right[pointstodraw_right.Count-1].Y, background.Width / 4f + (float)((a3) * 80f), background.Height-1);
        g.DrawCurve(Pens.Black, pointstodraw_right.ToArray());

        background.BackgroundImage = null;
        background.BackgroundImage = drawspace;
    }// AM FACUT ALTA FUNCTIE ASTA NU MAI ARE UTILITATE

    public static void draw_aprox() //MAI TREBE LUCRAT!
    {
        double Tu = t1 + (t2 - t1);
        double Tl = t4 - (t4 - t3);
        g.DrawLine(Pens.Orange, offset + background.Width / 4f + ((float)(t1 - 1) * 80f), background.Height, offset + background.Width / 4f + ((float)(t2 - 1) * 80f), background.Height - 100);
        g.DrawLine(Pens.Orange, offset + background.Width / 4f + ((float)(t2 - 1) * 80f), background.Height - 100, offset + background.Width / 4f + ((float)(t3 - 1) * 80f), background.Height - 100);
        g.DrawLine(Pens.Orange, offset + background.Width / 4f + ((float)(t3 - 1) * 80f), background.Height - 100, offset + background.Width / 4f + ((float)(t4 - 1) * 80f), background.Height);

        background.BackgroundImage = null;
        background.BackgroundImage = drawspace;
    }//AM FACUT ALTA FUNCTIE ASTA NU MAI ARE UTILITATE
    */
}
