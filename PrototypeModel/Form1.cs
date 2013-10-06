using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrototypeModel
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.GhostWhite;
            UI.redraw += Redraw;
            double sleepTime;
            sleepTime = double.Parse(ReplaceDot(textBox1.Text));

            double force = double.Parse(ReplaceDot(textBox3.Text));
            double scale = double.Parse(ReplaceDot(textBox4.Text));
            int iterations = int.Parse(textBox2.Text);
            UI.Button1Clicker(pictureBox1,(int)(sleepTime*1000),iterations,force,scale);
        }

        private void Redraw(object sender, ImageArguments arguments)
        {
            pictureBox1.Image = arguments._bmp;
        }

        private string ReplaceDot(string str)
        {
            string outStr = "";
            foreach (char c in str)
            {
                if (c!='.')
                {
                    outStr += c;
                }
                else
                {
                    outStr += '.';
                }
            }
            return outStr;
        }
    }
}
