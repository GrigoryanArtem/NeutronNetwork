using System.Windows;

namespace NeutronNetwork.CNN.GUI.Model
{
    public class Rectangle
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }

        public int Width => Right - Left;
        public int Height => Bottom - Top;
    }
}
