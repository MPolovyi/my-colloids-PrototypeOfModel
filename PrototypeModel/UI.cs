using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrototypeModel
{
    public static class UI
    {
        public static void Button1SimpleClicker(PictureBox pb)
        {
            Bitmap bmp = new Bitmap(pb.Width,pb.Height);
            Graphics canvas = Graphics.FromImage(bmp);

            Font timesFont = new Font("Times New Roman", 10.0f);
            Brush BlackBrush = new SolidBrush(Color.Black);
            canvas.DrawString("SomeString", timesFont, BlackBrush, 10, 30);

            pb.BackColor = Color.Azure;
            pb.Image = bmp;
        }

        public static void Button1Clicker()
        {
            World world = new World();
            Lattice[,] lattices = world.Live(100);
        }
    }
}
