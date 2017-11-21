namespace NeutronNetwork.CNN
{
    public class Digit
    {
        public int? Label { get; set; }
        public float[] Image { get; set; }

        public Digit(int? label, float[] image)
        {
            Label = label;
            Image = image;
        }

        public Digit() { }
    }
}
