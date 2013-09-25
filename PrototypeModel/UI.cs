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
        public static void Button1Clicker(PictureBox pb, Label label)
        {
            World world = new World();
            

            for (int i = 0; i < 200; i++)
            {
                Task Simulate = new Task(() =>
                                      TasksToWork(pb, world));
                Simulate.Start();
                Simulate.Wait();
                int i1 = i;
                label.Text = i1.ToString();
            }
            MessageBox.Show("Fin");
        }

        private static void TasksToWork(PictureBox pb,World world)
        {
            Func<Bitmap> live = () =>
                {
                    Bitmap map = world.Live();
                    //MessageBox.Show("MessageFromThread");
                    Thread.Sleep(1000);
                    return map;
                };
            Task<Bitmap> liveTask = Task<Bitmap>.Factory.StartNew(live);
            Bitmap image = liveTask.Result;
            MessageBox.Show("task");
            Task draw = Task.Factory.StartNew(() => Draw(pb, image));
            Task.WaitAll(liveTask, draw);
        }

        private static void Draw(PictureBox picture, Bitmap bmp)
        {
            picture.Image = bmp;
        }
    }
}
