using CNTK;
using NeutronNetwork.CNN.GUI.Model;
using NeutronNetwork.CNN.GUI.View;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeutronNetwork.CNN.GUI.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DigitNN.Instance.Learning(@"trainingsample.csv");
        }

        public ICommand ClearCommand { get; } = new AnotherCommandImplementation(o => ClearCanvas((InkCanvas)o));

        public ICommand AnalyzeCommand { get; } = new AnotherCommandImplementation(o => AnalyzeImage((InkCanvas)o));

        private static void ClearCanvas(InkCanvas canvas)
        {
            canvas.Strokes.Clear();
        }

        private static void AnalyzeImage(InkCanvas canvas)
        {
            Bitmap bitmap  = BitmapService.ResizeImage(BitmapService.GetInputImage(
                BitmapService.GetImageByCanvas(canvas)), 28, 28);

            ResultView rv = new ResultView(bitmap);
            rv.ShowDialog();
        }
    }
}
