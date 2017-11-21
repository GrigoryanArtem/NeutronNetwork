namespace NeutronNetwork.CNN
{
    public class DigitNNEventArgs
    {
        public string Message { get; private set; }

        public DigitNNEventArgs(string message) => Message = message;
    }
}