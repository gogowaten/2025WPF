using System.Windows;

namespace _20250222_SerializeDependencyProperty;

public partial class MainWindow : Window
{
    private AAA MyAAA { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        MyAAA = new()
        {
            MyInteger = 11111,
            MyTexts = ["マヨネーズ和えを以て", "唐辛子と茄子", "🌶️🍆"]
        };
        MyAAA.MyAAAs.Add(new AAA()
        {
            MyInteger = 222222,
            MyTexts = ["🀈🀉🀊🀈🀉🀊🀈🀉🀊🀚🀚🀚🀋 🀋", "タンヤオ"]
        });
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MyAAA.MySerialize("E:\\20250222.xml");
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var result = AAA.MyDeserialize("E:\\20250222.xml");
    }
}