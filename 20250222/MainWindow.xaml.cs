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
using System.Windows.Markup;


namespace _20250222;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public static readonly DependencyProperty MyPropertyProperty =
        DependencyProperty.Register("MyProperty", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));
    private AAA MyAAA { get; set; } = null!;
    public MainWindow()
    {
        InitializeComponent();
        IniAAA();
    }

    private void IniAAA()
    {
        MyAAA = new() { MyZIndex = 0 };
        MyAAA.MyDatas.Add(new AAA() { MyBrush = Brushes.Red });
        MyAAA.MyDatas.Add(new AAA() { MyBrush = Brushes.Blue, MyZIndex = 2 });
        MyAAA.MyDatas.Add(new AAA() { MyBrush = Brushes.Green, MyZIndex = 3 });
        var data = new AAA() { MyBrush = Brushes.Magenta, MyZIndex = 4 };
        MyAAA.MyDatas.Add(data);
    }
    public string MyProperty
    {
        get { return (string)GetValue(MyPropertyProperty); }
        set { SetValue(MyPropertyProperty, value); }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MyAAA.Serialize();
        //MyProperty = "Test";
        //Serialize();
    }

    private void Serialize()
    {
        var myPropertyValue = MyProperty;
        var serializedValue = System.Text.Json.JsonSerializer.Serialize(myPropertyValue);
        // シリアル化された値を使用する処理を追加
        MessageBox.Show(serializedValue);
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        //MyTextThumb.MyXamlWriter("E:\\20250223.xml");
        MyTextThumb.MySerialize("E:\\20250223.xml");
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        TextThumb? thumb = MyTextThumb.MyDeserialize("E:\\20250223.xml");
    }
}