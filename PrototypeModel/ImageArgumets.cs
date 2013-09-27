using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    public class ImageArguments : EventArgs
    {
        public Bitmap _bmp;

        public ImageArguments(Bitmap bmp, string iter)
        {
            _bmp = bmp;
        }
    }
}
