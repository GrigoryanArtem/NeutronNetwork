using NeutronNetwork.CNN;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeutronNetwork.Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            var digits = DigitImageCSVReader.Read(@"trainingsample.csv");

            for (int i = 0; i < 10; i++)
            {

                Bitmap bmp = new Bitmap(28, 28);

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        int color = (int)(digits[i].Image[y * bmp.Width + x] * 256f);
                        bmp.SetPixel(x, y, Color.FromArgb(color, color, color));
                    }
                }

                bmp.Save($"temp{i}.bmp");
            }
            //CTFGenerator.Generate(@"trainingsample", digits);
        }
    }
}
