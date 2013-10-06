using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PrototypeModel
{
    public static class UI
    {
        public delegate void Drawer(Object sender, ImageArguments bmp);

        public static event Drawer redraw;

        public static void Button1Clicker(PictureBox pb,int sleepTime,int iterationCount,double force, double scale)
        {
            World world = new World(pb.Height, pb.Width, force,scale);
            Task.Factory.StartNew(() =>
                {
                    Bitmap map = world.InitialCondition();
                    for (int i = 0; i < iterationCount; i++)
                    {
                        redraw(null, new ImageArguments(map, i.ToString()));
                        map = world.Live(i);
                        //MessageBox.Show(i.ToString());
                        Thread.Sleep(sleepTime);
                    }
                    MessageBox.Show("Fin");
                });
        }


    }
}
