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

namespace _20250502
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
    }

    //マークアップ拡張を作って、XAMLでグラデーションを簡単に書く方法 - SourceChord
    //https://sourcechord.hatenablog.com/entry/2014/02/22/232302
    [MarkupExtensionReturnType(typeof(LinearGradientBrush))]
    public class LinearGradientBrushExtension : MarkupExtension
    {
        public Color StartColor { get; set; } = Colors.White;
        public Color EndColor { get; set; } = Colors.Red;
        public double Angle { get; set; }

        public LinearGradientBrushExtension() { }
        public LinearGradientBrushExtension(Color start, Color end, double angle)
        {
            StartColor = start;
            EndColor = end;
            Angle = angle;
        }

        //必要
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new LinearGradientBrush(StartColor, EndColor, Angle);
        }
    }


    //[MarkupExtensionReturnType(typeof(LinearGradientBrush))]
    //public class LinearGradientBrushExtension : MarkupExtension
    //{
    //    public LinearGradientBrushExtension()
    //    {
    //        StartColor = Colors.White;
    //        EndColor = Colors.Black;
    //        Angle = 0;
    //    }

    //    public LinearGradientBrushExtension(Color startColor, Color endColor, double angle)
    //    {
    //        StartColor = startColor;
    //        EndColor = endColor;
    //        Angle = angle;
    //    }

    //    public Color StartColor { get; set; }
    //    public Color EndColor { get; set; }
    //    public double Angle { get; set; }

    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return new LinearGradientBrushExtension(StartColor, EndColor, Angle);
    //        LinearGradientBrush brush = new(StartColor, EndColor, Angle);
    //        return brush;
    //    }
    //}



}