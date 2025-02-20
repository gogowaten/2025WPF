using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace _20250216_Serialize
{
    public class KisoThumb : Thumb
    {
        protected ItemType MyItemType { get; set; }
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb), new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb()
        {
            MyItemData = new() { MyItemType = ItemType.None };
            DataContext = MyItemData;
            DragDelta += KisoThumb_DragDelta;
            Initialized += KisoThumb_Initialized;
        }

        private void KisoThumb_Initialized(object? sender, EventArgs e)
        {
            //デザイン画面で作成された要素の場合、ItemDataは無いので新規作成後に
            //デザイン画面の設定をItemDataに反映してからバインド設定
            if (MyItemData.MyItemType == ItemType.None)
            {
                MyItemData.MyItemType = MyItemType;
                CopyValueToItemData();
                MyItemDataBind();
            }
            //ファイルやItemDataから作成された要素の場合、そのままItemDataとバインド
            else
            {
                MyItemDataBind();
            }

            DataContext = MyItemData; //ここで適用

        }

        public KisoThumb(ItemData data)
        {
            MyItemData = data;
        }


        //ItemDataなしのコンストラクタで使う
        //バインドの前に、XAMLからの設定をItemDataにいれる
        private void CopyValueToItemData()
        {
            //XAMLでの設定をItemDataに入れる
            MyItemData.MyText = MyText;
            MyItemData.MyFontSize = FontSize;

            Color bc = ((SolidColorBrush)MyForeground).Color;
            MyItemData.MyForegroundA = bc.A;
            MyItemData.MyForegroundR = bc.R;
            MyItemData.MyForegroundG = bc.G;
            MyItemData.MyForegroundB = bc.B;

            bc = ((SolidColorBrush)MyBackground).Color;
            MyItemData.MyBackgroundA = bc.A;
            MyItemData.MyBackgroundR = bc.R;
            MyItemData.MyBackgroundG = bc.G;
            MyItemData.MyBackgroundB = bc.B;

        }

        //バインド、ItemDataをソース
        private void MyItemDataBind()
        {
            //バインド、ItemDataをソース
            SetBinding(MyTextProperty, nameof(MyItemData.MyText));
            SetBinding(MyLeftProperty, nameof(MyItemData.MyLeft));
            SetBinding(MyTopProperty, nameof(MyItemData.MyTop));
            SetBinding(FontSizeProperty, nameof(MyItemData.MyFontSize));

            var mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundA)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundR)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundG)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyForegroundB)));
            SetBinding(MyForegroundProperty, mb);


            mb = new() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundA)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundR)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundG)));
            mb.Bindings.Add(new Binding(nameof(MyItemData.MyBackgroundB)));
            SetBinding(MyBackgroundProperty, mb);

        }

        private void KisoThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is KisoThumb kiso)
            {
                kiso.MyLeft += e.HorizontalChange;
                kiso.MyTop += e.VerticalChange;
                e.Handled = true;
            }
        }


        #region 依存関係プロパティ


        public ItemData MyItemData
        {
            get { return (ItemData)GetValue(MyItemDataProperty); }
            set { SetValue(MyItemDataProperty, value); }
        }
        public static readonly DependencyProperty MyItemDataProperty =
            DependencyProperty.Register(nameof(MyItemData), typeof(ItemData), typeof(KisoThumb), new PropertyMetadata(null));


        #region 共通

        public Brush MyForeground
        {
            get { return (Brush)GetValue(MyForegroundProperty); }
            set { SetValue(MyForegroundProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundProperty =
            DependencyProperty.Register(nameof(MyForeground), typeof(Brush), typeof(KisoThumb),
                new FrameworkPropertyMetadata(Brushes.Black,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public Brush MyBackground
        {
            get { return (Brush)GetValue(MyBackgroundProperty); }
            set { SetValue(MyBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundProperty =
            DependencyProperty.Register(nameof(MyBackground), typeof(Brush), typeof(KisoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //テキスト系要素のText要素
        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(KisoThumb),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(KisoThumb),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            internal set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            internal set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(KisoThumb), new PropertyMetadata(0.0));


        #endregion 共通


        #endregion 依存関係プロパティ

    }


    public class TextThumb : KisoThumb
    {
        static TextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextThumb), new FrameworkPropertyMetadata(typeof(TextThumb)));
        }

        public TextThumb()
        {
            MyItemType = ItemType.Text;
        }

        public TextThumb(ItemData data)
        {
            MyItemData = data;
        }
    }


    public class MyConverterARGBtoSolidBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (byte)values[0];
            var r = (byte)values[1];
            var g = (byte)values[2];
            var b = (byte)values[3];
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var br = (SolidColorBrush)value;
            return [br.Color.A, br.Color.R, br.Color.G, br.Color.B];

        }
    }




}
