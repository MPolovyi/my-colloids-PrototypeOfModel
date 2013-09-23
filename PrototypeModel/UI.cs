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
        public static void Button1Clicker(PictureBox pb)
        {
            World world = new World();

            for (int i = 0; i < 100; i++)
            {
                Task<Bitmap> live = new Task<Bitmap>(()=>
                    {
                        Bitmap map = world.Live();
                        MessageBox.Show("MessageFromThread");
                        return map;
                    });
                live.Start();
                live.Wait();
                pb.Image = live.Result;

                Thread.Sleep(1000);
            }
        }
    }
}
