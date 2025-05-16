using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250516
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new(MyTextBlock.Text);
            bool? result = inputBox.ShowDialog();
            if (result == true)
            {
                MyTextBlock.Text = inputBox.MyTextBox.Text;
            }
        }
    }
}