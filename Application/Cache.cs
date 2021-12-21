using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Application
{
    public static class Cache
    {
        private static Dictionary<string, Bitmap> Images { get; set; } = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmapFromCache(string path)
        {
            if(!Images.ContainsKey(path))
            {
                //path = AppDomain.CurrentDomain.BaseDirectory + path;
                Images.Add(path, new Bitmap(path));
            }
            return Images[path];
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static Bitmap CreateEmptyBitmap(int width, int height)
        {
            string emptyKey = "empty";

            if (Images.ContainsKey(emptyKey)) return (Bitmap)Images[emptyKey].Clone();

            Images.Add(emptyKey, new Bitmap(width, height));
            Graphics g = Graphics.FromImage(Images[emptyKey]);
            g.FillRectangle(new SolidBrush(System.Drawing.Color.LightGreen), 0, 0, width, height);
            return (Bitmap)Images[emptyKey].Clone();
        }

        public static void EmptyCache()
        {
            Images.Clear();
        }
    }
}
