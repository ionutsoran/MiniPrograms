using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aproximation_of_trapezoidal_fuzzy_numbers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Engine.Initialize(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Engine.get_input(textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text,textBox5.Text);
            switch(Engine.cb_selected_index)
            {
                case 0:Engine.check_condition1();break;
                case 1:Engine.check_condition2();break;
                case 2:Engine.check_condition3();break;
                default:break;
            }
            Engine.Start();
           // MessageBox.Show(Engine.cb_selected_index + "");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Engine.cb_selected_index = comboBox1.SelectedIndex;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Engine.offset+= 80;
            Engine.incre--;
            Engine.Drawgraph1();
            Engine.Draw_aprox1(Engine.cl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Engine.offset-= 80;
            Engine.incre++;
            Engine.Drawgraph1();
            Engine.Draw_aprox1(Engine.cl);
        }
    }
}
