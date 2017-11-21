using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace NeutronNetwork.CNN
{
    public static class BitmapConvertor
    {
        public static Digit ToDigit(this Bitmap bitmap, float gamma = 1, bool IsInvert = false)
        {
            float[] image = new float[bitmap.Width * bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                    image[(y * bitmap.Width) + x] = (1f * (IsInvert == true ? 1f : 0f)) - 
                        (ColorToGrayscale(bitmap.GetPixel(x, y), gamma) / 256f);

            var result = new Digit(null, image);

            Bitmap bmp = new Bitmap(28, 28);

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int color = (int)(result.Image[y * bmp.Width + x] * 255f);
                    bmp.SetPixel(x, y, Color.FromArgb(color, color, color));
                }
            }

            bmp.Save($"temps.bmp");

            return result;
        }

        public static BitmapImage BitmapToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private static float ColorToGrayscale(Color color, float gamma)
        {
            return (float)(0.2126 * Math.Pow(color.R, gamma) +
                0.7152 * Math.Pow(color.G, gamma) +
                0.0722 * Math.Pow(color.B, gamma));
        }
    }
}
