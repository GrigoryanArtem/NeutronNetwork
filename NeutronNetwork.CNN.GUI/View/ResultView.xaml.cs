using NeutronNetwork.CNN.GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeutronNetwork.CNN.GUI.View
{
    /// <summary>
    /// Логика взаимодействия для ResultView.xaml
    /// </summary>
    public partial class ResultView : Window
    {
        public ResultView(Bitmap bitmap)
        {
            InitializeComponent();
            ctDigitPlot.Source = bitmap.BitmapToImageSource();
            var d = bitmap.ToDigit(IsInvert: true);
            ctResultLabel.Text = $"I think answer is {DigitNN.Instance.GetValue(d).ToString()}";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }  
    }
}
