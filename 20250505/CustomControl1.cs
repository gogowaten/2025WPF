using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250505
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250505"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:_20250505;assembly=_20250505"
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

    public class Togoole : ToggleButton
    {
        static Togoole()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Togoole), new FrameworkPropertyMetadata(typeof(Togoole)));
        }
        public Togoole()
        {

        }
    }
    
    public class Togoole2 : ToggleButton
    {
        static Togoole2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Togoole2), new FrameworkPropertyMetadata(typeof(Togoole2)));
        }
        public Togoole2()
        {

        }
    }
    
    public class Togoole3 : ToggleButton
    {
        static Togoole3()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Togoole3), new FrameworkPropertyMetadata(typeof(Togoole3)));
        }
        public Togoole3()
        {

        }


        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(Togoole3), new PropertyMetadata(Brushes.Tomato));

    }

    public class ToggleControl : ToggleButton
    {
        static ToggleControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleControl), new FrameworkPropertyMetadata(typeof(ToggleControl)));
        }
        public ToggleControl() { }
    }

}
