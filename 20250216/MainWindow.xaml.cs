using System.Text;
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

namespace _20250216;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    #region 依存関係プロパティ

    /// <summary>
    /// Thumb追加時のスライド量
    /// </summary>
    public int MySlideHorizontal
    {
        get { return (int)GetValue(MySlideHorizontalProperty); }
        set { SetValue(MySlideHorizontalProperty, value); }
    }
    public static readonly DependencyProperty MySlideHorizontalProperty =
        DependencyProperty.Register(nameof(MySlideHorizontal), typeof(int), typeof(KisoThumb), new PropertyMetadata(32));

    public int MySlideVertical
    {
        get { return (int)GetValue(MySlideVerticalProperty); }
        set { SetValue(MySlideVerticalProperty, value); }
    }
    public static readonly DependencyProperty MySlideVerticalProperty =
        DependencyProperty.Register(nameof(MySlideVertical), typeof(int), typeof(KisoThumb), new PropertyMetadata(32));

    #endregion 依存関係プロパティ

    //private void AddEllipseText(string text, Brush fill, double width, double height)
    //{
    //    EllipseTextThumb thumb = new() { MyText = text, MyFill = fill, MyWidth = width, MyHeight = height, MyLeft = MySlideHorizontal, MyTop = MySlideVertical };
    //    MyRootGroup.AddThumbToActiveGroup(thumb);
    //}

    //private void MyButtonAdd_Click(object sender, RoutedEventArgs e)
    //{
    //    AddEllipseText($"{DateTime.Now:ss秒FFF}", Brushes.YellowGreen, 50, 50);
    //}

    //private void MyButtonTest_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.ActiveGroupToOutside();
    //}

    //private void MyButtonTest2_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.ActiveGroupToInside();
    //}

    //private void MyButtonTest4_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.ActiveGroupFromClickedThumbsParent();
    //}

    //private void MyButtonRemove_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.RemoveSelectedThumbsFromActiveGroup();
    //}

    //private void MyButtonRemoveAll_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.RemoveAll();
    //}

    //private void MyButtonMakeGroup_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.AddGroupFromSelected();
    //}

    //private void MyButonUngroup_Click(object sender, RoutedEventArgs e)
    //{
    //    MyRootGroup.UngroupFocusThumb();
    //}

    //private void Button_Click(object sender, RoutedEventArgs e)
    //{
    //    var data = KanaSerialize(MyGroup2);
    //}



    //// 指定の WPF オブジェクトをシリアル化する
    //// obj : シリアル化するオブジェクト
    //// 戻り値は、シリアル化後の XML ドキュメント形式の文字列
    //private string KanaSerialize(object obj)
    //{
    //    var settings = new XmlWriterSettings();

    //    // 出力時の条件
    //    settings.Indent = true;
    //    settings.NewLineOnAttributes = false;

    //    // XML バージョン情報の出力を抑制する
    //    // バージョン情報が必要な場合は ConformanceLevel.Document を指定する
    //    settings.ConformanceLevel = ConformanceLevel.Fragment;

    //    var sb = new StringBuilder();
    //    XmlWriter writer = null;
    //    XamlDesignerSerializationManager manager = null;

    //    try
    //    {
    //        writer = XmlWriter.Create(sb, settings);
    //        manager = new XamlDesignerSerializationManager(writer);
    //        manager.XamlWriterMode = XamlWriterMode.Expression;
    //        System.Windows.Markup.XamlWriter.Save(obj, manager);
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.Message);
    //    }
    //    finally
    //    {
    //        if (writer != null)
    //            writer.Close();
    //    }

    //    return sb.ToString();
    //}

    ////---------------------------------------------------------------------------------------------
    //// 指定の XML 文を読み込んで逆シリアル化し、WPF オブジェクトを返す
    //// xmlText : XML 文
    //// 戻り値  : WPF オブジェクト
    //private object KanaDeserialize(string xmlText)
    //{
    //    var doc = new XmlDocument();

    //    try
    //    {
    //        doc.LoadXml(xmlText);
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.Message);
    //    }

    //    object obj = null;

    //    try
    //    {
    //        obj = System.Windows.Markup.XamlReader.Load(new XmlNodeReader(doc));
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.Message);
    //    }

    //    return obj;
    //}





}





