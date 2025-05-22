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

//C#のForm,WPFで Delegate, Action, Func, メソッドの定義 の違いをサンプルコードでざっくり理解する #Windows - Qiita
// https://qiita.com/tatsubey/items/ec2fd859cf0bb40128eb

namespace _20250520
{
    public partial class MainWindow : Window
    {
        public AAA MyAAA { get; set; }
        public AAA MyAAA2 { get; set; } = new();
        public BBB MyBBB { get; set; }
        delegate void MyDel();
        public MainWindow()
        {
            InitializeComponent();

            MyAAA = new AAA();
            DataContext = this;
            
            var del = new MyDel(MyMessageShow);
            Action ac = new(del);
            MyBBB = new(new Action(del));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyMessageShow();
        }
        private void MyMessageShow()
        {
            MessageBox.Show("ゆっくりしていってね！！！");
        }
    }

    public class AAA : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            //起動時ここが処理される
            //ボタンクリック時にも処理され、続けてExecuteが処理される
            return true;
        }

        public void Execute(object? parameter)
        {
            //ボタンクリックしたときに処理される
            MessageBox.Show("CommandOK");
        }
    }

    public class BBB : ICommand
    {
        private readonly Action? _action;

        public BBB(Action? action)
        {
            _action = action;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _action?.Invoke();
        }
    }
}