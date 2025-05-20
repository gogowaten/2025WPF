using System.ComponentModel;
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

namespace _20250520
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        //public CommandWindowViewModel CommandWindowViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            DataContext=new CommandWindowViewModel();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("");
        }
    }


    public class CommandForWin2 : ICommand
    {
        public int counter { get; set; } = 0;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            { return true; }
        }

        public void Execute(object? parameter)
        {
            counter++;
        }
    }

    public class ViewModelForWin2 : INotifyPropertyChanged
    {

    }

    public class AAA : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }

    // 外部から与えるデータ（＝ビューモデル）
    public class CommandWindowViewModel
    {
        class OkCommandImpl : ICommand
        {
            public bool CanExecute(object parameter) { return true; }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                MessageBox.Show("コマンドが実行されました。");
            }
        }

        public ICommand OkCommand { get; private set; }

        public CommandWindowViewModel()
        {
            this.OkCommand = new OkCommandImpl();
        }
    }
}