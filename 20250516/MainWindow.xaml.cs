using System.Windows;

//ダイアログ ボックスの概要 - WPF | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/windows/dialog-boxes-overview?view=netdesktop-9.0

// ウィンドウを閉じる方法 - WPF | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/windows/how-to-close-window-dialog-box

namespace _20250516
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new(MyTextBlock.Text);
            if (inputBox.ShowDialog() == true)
            {
                MyTextBlock.Text = inputBox.MyTextBox.Text;
            }
        }
    }
}