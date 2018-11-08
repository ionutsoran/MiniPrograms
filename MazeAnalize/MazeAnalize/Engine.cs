using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Timers;
using System.Threading.Tasks;

namespace MazeAnalize
{
    public enum Direction { Up,Left,Right,Down};
    public static class Engine
    {
        public static Graphics g;
        public static Bitmap map,defaultmap;
        public static Direction dire = Direction.Left;
        public static Point lastpoint;
        public static Timer timer=new Timer();
        public static Random rand = new Random();
        public static Form1 form1;
       
      

        public static void GetSize(int x,int y,Form1 form)//initializarea spatiului de lucru pe bitmap
        {
            map = new Bitmap(x,y);
            defaultmap = new Bitmap(x, y);
            g = Graphics.FromImage(map);
            lastpoint = new Point(x/2,y/2);
            form1 = form;
        }

        //am facut un alt timer visual cred din toolbox si cu ala o mers trebe sa intrebi dc!
        public static void StartTimer()//inceperea desenarii folosind timer + declaratii la timer
        {
            timer.Interval = 50;
            timer.Elapsed += new ElapsedEventHandler(timing);
            DrawLine(lastpoint, dire);
          //  lista.Add(dire.ToString());
            form1.listBox1.Items.Add(dire.ToString());
            form1.pictureBox1.BackgroundImage = map;
            form1.timer1.Interval = 50;
            form1.timer1.Start();
           // timer.Start();
        }

        public static void timing(object sender,EventArgs e)//la fiecare 0.8 secunde deseneaza o linie
        {
            

            // direction = (Direction)(rand.Next(Enum.GetValues(typeof(Direction)).Length));
            Direction direc = getRightDirection(dire); //salvez valoarea corecta returnata de functia  getrightdirection
            DrawLine(lastpoint, direc); //apeles functia de desenare
           // form1.listBox1.Items.Add(direc.ToString());
            form1.pictureBox1.BackgroundImage = defaultmap;//container temporar pentru schimbarea referintei 
            form1.pictureBox1.BackgroundImage = map;//updatez referinta cu cea originala
            
        }

        public static void DrawLine(Point last, Direction dir)//functia de desenat linia in functie de directia primita
        {
            switch (dir)
            {
                case Direction.Up:
                    g.DrawLine(Pens.Orange, last, new Point(last.X, last.Y - 5));
                    lastpoint = new Point(last.X, last.Y - 5);
                    dire = Direction.Up; break;

                case Direction.Down:
                    g.DrawLine(Pens.Green, last, new Point(last.X, last.Y + 5));
                    lastpoint = new Point(last.X, last.Y + 5);
                    dire = Direction.Down; break;

                case Direction.Left:
                    g.DrawLine(Pens.Blue, last, new Point(last.X - 5, last.Y));
                    lastpoint = new Point(last.X - 5, last.Y);
                    dire = Direction.Left; break;

                case Direction.Right:
                    g.DrawLine(Pens.Red, last, new Point(last.X + 5, last.Y));
                    lastpoint = new Point(last.X + 5, last.Y);
                    dire = Direction.Right; break;

                default: break;
            }
        }

        public static Direction getRightDirection(Direction lastDirection)
         {
            bool ok;
            Direction direction;
            do
            {
                ok = true;
                direction = (Direction)(rand.Next(Enum.GetValues(typeof(Direction)).Length));

                if (direction == Direction.Left && lastDirection == Direction.Right)
                    ok = false;
                else
                    if (direction == Direction.Right && lastDirection == Direction.Left)
                    ok = false;
                else
                         if (direction == Direction.Up && lastDirection == Direction.Down)
                    ok = false;
                else
                             if (direction == Direction.Down && lastDirection == Direction.Up)
                    ok = false;

            } while (!ok); //verific daca nu ma intorc napoi din drum*/

            return direction;
        }//functie care intoarce o directie corecta

       
    }
}
