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

namespace _20250516
{
    /// <summary>
    /// InputBox.xaml の相互作用ロジック
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox(string text)
        {
            InitializeComponent();
            MyTextBox.Text = text;
            MyTextBox.SelectAll();
        }

        private void MyOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void MyCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
