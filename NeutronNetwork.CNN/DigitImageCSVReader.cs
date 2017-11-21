using System.IO;
using System.Linq;

namespace NeutronNetwork.CNN
{
    public static class DigitImageCSVReader
    {
        public static Digit[] Read(string filename)
        {
            var lines = File.ReadAllLines(filename);

            return lines.Skip(1)
                .Select(line => line.Split(',')
                    .Select(s => float.Parse(s)))
                .Select(n => new Digit((int)n.First(), n.Skip(1).Select(f => f / 256f).ToArray()))
                .ToArray();
        }
    }
}
