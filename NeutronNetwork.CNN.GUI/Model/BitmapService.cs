using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NeutronNetwork.CNN.GUI.Model
{
    public static class BitmapService
    {
        private static Color NeutralColor = Color.White;

        private enum PointPosition
        {
            Top,
            Bottom,
            Left,
            Right
        }

        public static Bitmap GetImageByCanvas(InkCanvas canvas)
        {
            Rect rect = new Rect(canvas.RenderSize);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return (Bitmap)System.Drawing.Image.FromStream(ms);
            }
        }

        public static Bitmap LoadFromFile(string path)
        {
            return (Bitmap)System.Drawing.Image.FromFile(path);
        }

        public static Bitmap GetInputImage(Bitmap image)
        {
            var rect = GetExtremeRectangle(image);

            int squadSide = Math.Max(rect.Width, rect.Height);

            squadSide += (int)((double)squadSide * 0.2);

            int x = squadSide / 2 - rect.Width / 2;
            int y = squadSide / 2 - rect.Height / 2;

            Bitmap resultBmp = new Bitmap(squadSide, squadSide);
            using (Graphics gfx = Graphics.FromImage(resultBmp))
            {
                using (SolidBrush brush = new SolidBrush(NeutralColor))
                {
                    gfx.FillRectangle(brush, 0, 0, squadSide, squadSide);
                    gfx.DrawImage(
                        image, 
                        new RectangleF(x, y, rect.Width, rect.Height), 
                        new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height),
                        GraphicsUnit.Pixel);
                }
            }

            return resultBmp;
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        #region Private methods

        private static Rectangle GetExtremeRectangle(Bitmap image)
        {
            return new Rectangle
            {
                Top = GetExtremePoint(image, PointPosition.Top),
                Bottom = GetExtremePoint(image, PointPosition.Bottom),
                Left = GetExtremePoint(image, PointPosition.Left),
                Right = GetExtremePoint(image, PointPosition.Right)
            };
        }

        private static int GetExtremePoint(Bitmap image, PointPosition position)
        {
            int? result = null;

            switch (position)
            {
                case PointPosition.Top:
                    result = GetTopExtremePoint(image);
                    break;
                case PointPosition.Bottom:
                    result = GetBottomExtremePoint(image);
                    break;
                case PointPosition.Left:
                    result = GetLeftExtremePoint(image);
                    break;
                case PointPosition.Right:
                    result = GetRightExtremePoint(image);
                    break;
            }

            return result ?? throw new ArgumentException(nameof(position));
        }

        private static int GetTopExtremePoint(Bitmap image)
        {
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    if (image.GetPixel(x, y).ToArgb() != NeutralColor.ToArgb())
                        return y;

            return 0;
        }

        private static int GetBottomExtremePoint(Bitmap image)
        {
            for (int y = image.Height - 1; y >= 0; y--)
                for (int x = 0; x < image.Width; x++)
                    if (image.GetPixel(x, y).ToArgb() != NeutralColor.ToArgb())
                        return y;

            return image.Height - 1;
        }

        private static int GetLeftExtremePoint(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    if (image.GetPixel(x, y).ToArgb() != NeutralColor.ToArgb())
                        return x;

            return 0;
        }

        private static int GetRightExtremePoint(Bitmap image)
        {
            for (int x = image.Width - 1; x >= 0; x--)
                for (int y = 0; y < image.Height; y++)
                    if (image.GetPixel(x, y).ToArgb() != NeutralColor.ToArgb())
                        return x;

            return image.Width - 1;
        }
        #endregion
    }
}
