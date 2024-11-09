using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOverlay
{
    class functions
    {
        public byte[] pngToBytes(System.Drawing.Bitmap bmp)
        {
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                bmp = ResizeBitmap(bmp, bmp.Width / 2, bmp.Height / 2);
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                arr = ms.ToArray();
            }
            return arr;
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }
    }
}
