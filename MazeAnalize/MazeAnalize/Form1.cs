using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeAnalize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Engine.GetSize(pictureBox1.Width, pictureBox1.Height,this);
            Engine.StartTimer();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // direction = (Direction)(rand.Next(Enum.GetValues(typeof(Direction)).Length));
            Direction direc =Engine.getRightDirection(Engine.dire); //salvez valoarea corecta returnata de functia  getrightdirection
            Engine.DrawLine(Engine.lastpoint, direc); //apeles functia de desenare
            listBox1.Items.Add(direc.ToString());
            pictureBox1.BackgroundImage =Engine.defaultmap;//container temporar pentru schimbarea referintei 
            pictureBox1.BackgroundImage =Engine.map;//updatez referinta cu cea originala
        }
    }
}
