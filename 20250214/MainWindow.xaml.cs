using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace _20250214;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private EzLineThumb MyTT;
    public MainWindow()
    {
        InitializeComponent();

        var pc = new PointCollection
        {
            new Point(),
            new Point(100, 200)
        };
        ItemData data = new()
        {
            MyLeft = 200,
            MyPoints = pc,
            MyStrokeThickness=20,
        };

        MyTT = new(data);
        MyTT.Name = "eeeeeeeeee";
        MyCanvas.Children.Add(MyTT);
        var neko = MyTT.MyItemData;
    }

    private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (MyEz.MyPoints != null && MyEz.MyPoints.Count != 0)
        {
            MyEz.MyPoints[MyEz.MyPoints.Count - 1] = e.GetPosition(MyCanvas);
        }
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MyEz.AnchorOn();

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        MyEz.AnchorOff();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        Random r = new();
        MyEz.AddPoint(new Point(r.Next(200), r.Next(200)), MyEz.MyPoints.Count);
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        if (MyEz.MyPoints.Count > 0)
        {
            MyEz.RemovePoint(MyEz.MyPoints.Count - 1);
        }
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
        var left = MyTT.MyLeft;
        var data = MyTT.MyItemData;
        //MyEz.ZeroFix();
    }


    private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (MyEz.MyPoints.Count == 0)
        {
            MyEz.MyPoints.Add(e.GetPosition(MyCanvas));
        }
        MyEz.MyPoints.Add(e.GetPosition(MyCanvas));
    }

    private void MyCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        MyCanvas.PreviewMouseLeftButtonDown -= MyCanvas_PreviewMouseLeftButtonDown;
        //MyCanvas.MouseLeftButtonDown -= MyCanvas_MouseLeftButtonDown;
        MyCanvas.MouseMove -= MyCanvas_MouseMove;
        MyCanvas.MouseRightButtonDown -= MyCanvas_MouseRightButtonDown;
    }

    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        var data = MyEz.MyItemData;



        
        string filePath = "E:\\20250214.xml";

        //SaveLine(MyEz.MyItemData, filePath);
        CCC c = new();
        //SaveLine3(c, filePath);
        //SaveLine2(MyEz, filePath);
        string kanac = KanaSerialize(MyEz);
        var kanad = KanaDeserialize(kanac);
        if(kanad is EzLineThumb kana)
        {
            MyCanvas.Children.Add(kana);
        }
    }


    private void SaveLine(ItemData line, string filePath)
    {
        DataContractSerializer serializer = new(typeof(ItemData));
        using FileStream stream = new(filePath, FileMode.Create);
        serializer.WriteObject(stream, line);
    }
    private void SaveLine2<T>(T line, string filePath)
    {
        DataContractSerializer serializer = new(typeof(T));
        using FileStream stream = new(filePath, FileMode.Create);
        serializer.WriteObject(stream, line);
    }
    private void SaveLine3<T>(T line, string filePath)
    {
        DataContractSerializer serializer = new(typeof(T));
        using (XmlWriter writer = XmlWriter.Create(filePath))
        {
            try
            {
                serializer.WriteObject(writer, line);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        
    }

    // 指定の WPF オブジェクトをシリアル化する
    // obj : シリアル化するオブジェクト
    // 戻り値は、シリアル化後の XML ドキュメント形式の文字列
    private string KanaSerialize(object obj)
    {
        var settings = new XmlWriterSettings();

        // 出力時の条件
        settings.Indent = true;
        settings.NewLineOnAttributes = false;

        // XML バージョン情報の出力を抑制する
        // バージョン情報が必要な場合は ConformanceLevel.Document を指定する
        settings.ConformanceLevel = ConformanceLevel.Fragment;

        var sb = new StringBuilder();
        XmlWriter writer = null;
        XamlDesignerSerializationManager manager = null;

        try
        {
            writer = XmlWriter.Create(sb, settings);
            manager = new XamlDesignerSerializationManager(writer);
            manager.XamlWriterMode = XamlWriterMode.Expression;
            System.Windows.Markup.XamlWriter.Save(obj, manager);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }

        return sb.ToString();
    }

    //---------------------------------------------------------------------------------------------
    // 指定の XML 文を読み込んで逆シリアル化し、WPF オブジェクトを返す
    // xmlText : XML 文
    // 戻り値  : WPF オブジェクト
    private object KanaDeserialize(string xmlText)
    {
        var doc = new XmlDocument();

        try
        {
            doc.LoadXml(xmlText);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        object obj = null;

        try
        {
            obj = System.Windows.Markup.XamlReader.Load(new XmlNodeReader(doc));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        return obj;
    }
}


[DataContract]
[KnownType(typeof(SolidColorBrush))]
public class CCC : DependencyObject
{
    [DataMember]
    public Brush MyBrush
    {
        get { return (Brush)GetValue(MyBrushProperty); }
        set { SetValue(MyBrushProperty, value); }
    }
    public static readonly DependencyProperty MyBrushProperty =
        DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(CCC), new PropertyMetadata(Brushes.Transparent));

    public CCC()
    {
        MyBrush = Brushes.Red;
    }
}
