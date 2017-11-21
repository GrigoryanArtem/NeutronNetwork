using NeutronNetwork.CNN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeutronNetwork.Helper
{
    public class CTFGenerator
    {
        public static void Generate(string filename, Digit[] digits)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var digit in digits) {
                sb.Append("|features ");

                foreach (var pxl in digit.Image)
                    sb.Append($"{pxl} ");

                sb.AppendLine($"|label{DigitToString((int)digit.Label)}");
            }

            File.WriteAllText(filename, sb.ToString());
        }

        private static string DigitToString(int digit)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 10; i++) {
                var value = i == digit ? 1 : 0;
                sb.Append($" {value}");
            }

            return sb.ToString();
        }
    }
}
