using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для LearningWindow.xaml
    /// </summary>
    public partial class LearningWindow : Window
    {
        public LearningWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DigitNN.Instance.DigitNNUpdated += OnDigitNNUpdated;
            DigitNN.Instance.Learning(@"trainingsample.csv");
            //Close();
        }

        private void OnDigitNNUpdated(DigitNNEventArgs e)
        {
            ctListBox.Items.Add(e.Message);
        }
    }
}
