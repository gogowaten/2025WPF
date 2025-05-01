using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250501_01
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250501_01"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250501_01;assembly=_20250501_01"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }


    public class TexxtBox : TextBox
    {
        //private FrameworkElement MyTextB = null!;
        static TexxtBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TexxtBox), new FrameworkPropertyMetadata(typeof(TexxtBox)));
        }
        public TexxtBox()
        {
            Loaded += TexxtBox_Loaded;
        }

        private void TexxtBox_Loaded(object sender, RoutedEventArgs e)
        {


            //MyTextB.SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyHigeProperty) });

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("textB") is FrameworkElement text)
            {
                //MyTextB = text;
                var hige = new Binding() { Source = this, Path = new PropertyPath(MyHigeProperty) };
                var higeThickness = new Binding() { Source = this, Path = new PropertyPath(MyHigeThicknessProperty) };
                var textHeight = new Binding() { Source = text, Path = new PropertyPath(ActualHeightProperty) };
                var textWidth = new Binding() { Source = text, Path = new PropertyPath(ActualWidthProperty) };
                MultiBinding mb;

                var mbHeight = new MultiBinding() { Converter = new MyConvHige2() };
                mbHeight.Bindings.Add(hige);
                mbHeight.Bindings.Add(textHeight);

                if (GetTemplateChild("left") is Rectangle left)
                {
                    left.SetBinding(HeightProperty, mbHeight);
                    mb = new() { Converter = new MyConvTopLeft(), ConverterParameter = "left" };
                    mb.Bindings.Add(hige);
                    mb.Bindings.Add(higeThickness);
                    left.SetBinding(Canvas.LeftProperty, mb);
                }
                if (GetTemplateChild("right") is Rectangle right)
                {
                    right.SetBinding(HeightProperty, mbHeight);
                    mb = new() { Converter = new MyConvHige() };
                    mb.Bindings.Add(hige);
                    mb.Bindings.Add(textWidth);
                    right.SetBinding(Canvas.LeftProperty, mb);
                }

                var mbWidth = new MultiBinding() { Converter = new MyConvHige2() };
                mbWidth.Bindings.Add(hige);
                mbWidth.Bindings.Add(textWidth);

                if (GetTemplateChild("top") is Rectangle top)
                {
                    top.SetBinding(WidthProperty, mbWidth);
                    mb = new() { Converter = new MyConvTopLeft(), ConverterParameter = "top" };
                    mb.Bindings.Add(hige);
                    mb.Bindings.Add(higeThickness);
                    top.SetBinding(Canvas.TopProperty, mb);
                }
                if (GetTemplateChild("bottom") is Rectangle bottom)
                {
                    bottom.SetBinding(WidthProperty, mbWidth);
                    mb = new() { Converter = new MyConvHige() };
                    mb.Bindings.Add(hige);
                    mb.Bindings.Add(textHeight);
                    bottom.SetBinding(Canvas.TopProperty, mb);
                }
            }

        }


        public Brush MyHigeBrush
        {
            get { return (Brush)GetValue(MyHigeBrushProperty); }
            set { SetValue(MyHigeBrushProperty, value); }
        }
        public static readonly DependencyProperty MyHigeBrushProperty =
            DependencyProperty.Register(nameof(MyHigeBrush), typeof(Brush), typeof(TexxtBox), new PropertyMetadata(Brushes.MediumAquamarine));


        public Thickness MyHigeThickness
        {
            get { return (Thickness)GetValue(MyHigeThicknessProperty); }
            set { SetValue(MyHigeThicknessProperty, value); }
        }
        public static readonly DependencyProperty MyHigeThicknessProperty =
            DependencyProperty.Register(nameof(MyHigeThickness), typeof(Thickness), typeof(TexxtBox), new PropertyMetadata(new Thickness(1.0)));


        public double MyHige
        {
            get { return (double)GetValue(MyHigeProperty); }
            set { SetValue(MyHigeProperty, value); }
        }
        public static readonly DependencyProperty MyHigeProperty =
            DependencyProperty.Register(nameof(MyHige), typeof(double), typeof(TexxtBox), new PropertyMetadata(10.0));

    }



    public class MyConvTopLeft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hige = (double)values[0];
            var thickness = (Thickness)values[1];
            var locate = (string)parameter;
            if (locate == "left")
            {
                return hige - thickness.Left;
            }
            else if (locate == "top")
            {
                return hige - thickness.Top;
            }
            else
            {
                return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConvHige : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hige = (double)values[0];
            var hen = (double)values[1];
            return hen + hige;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvHige2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hige = (double)values[0];
            var hen = (double)values[1];
            return hen + hige + hige;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




}
