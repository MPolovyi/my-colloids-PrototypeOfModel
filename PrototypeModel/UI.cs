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

        public static void Button1Clicker(PictureBox pb)
        {
            World world = new World();
            Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 200; i++)
                    {
                        Bitmap map = world.Live(i);
                        Thread.Sleep(1000);
                        redraw(null, new ImageArguments(map,i.ToString()));
                    }
                    MessageBox.Show("Fin");
                });
        }


    }
}
