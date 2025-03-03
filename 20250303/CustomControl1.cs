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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250303
{

    public class LineThumb : Thumb
    {

        static LineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineThumb), new FrameworkPropertyMetadata(typeof(LineThumb)));
        }
        public LineThumb()
        {

            DragDelta += LineThumb_DragDelta;
            Loaded += LineThumb_Loaded;
        }

        private void LineThumb_Loaded(object sender, RoutedEventArgs e)
        {
            //Relayout();
            //MyBind();
            //MyBind2();
            //MyLeftは指定用
            //MyOffsetLeftは実際の表示位置なのでCanvas．Leftとバインドする、計算はEzLineの位置とMyLeftからする
            //SetBinding(WidthProperty, new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Converter = new MyConvRectWidth() });


        }

        private void MyBind()
        {

            //var mb = new MultiBinding() { Converter = new MyConverterLeftRectLeft() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyLeftProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
            //SetBinding(MyOffsetLeftProperty, mb);

            //mb = new MultiBinding() { Converter = new MyConverterTopRectTop() };
            //mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyTopProperty), Mode = BindingMode.OneWay });
            //mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property), Mode = BindingMode.OneWay });
            //SetBinding(MyOffsetTopProperty, mb);

            //SetBinding(Canvas.LeftProperty, new Binding() { Source = this, Path = new PropertyPath(MyOffsetLeftProperty) });
            //SetBinding(Canvas.TopProperty, new Binding() { Source = this, Path = new PropertyPath(MyOffsetTopProperty) });

        }

        private void MyBind2()
        {
            var mb = new MultiBinding() { Converter = new MyConverterLeftRectLeft() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(Canvas.LeftProperty) });
            mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property) });
            SetBinding(MyOffsetLeftProperty, mb);

            mb = new MultiBinding() { Converter = new MyConverterTopRectTop() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(Canvas.TopProperty) });
            mb.Bindings.Add(new Binding() { Source = MyEzLine, Path = new PropertyPath(EzLine.MyBounds4Property) });
            SetBinding(MyOffsetTopProperty, mb);

        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("handle") is Thumb handle)
            {
                handle.DragDelta += Handle_DragDelta;
            }
            if (GetTemplateChild("line") is EzLine line)
            {
                MyEzLine = line;
                if (MyPoints is null)
                {
                    MyPoints = MyEzLine.MyPoints;
                }
                else
                {
                    MyEzLine.MyPoints = MyPoints;
                }
            }
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyCanvas = panel;
            }

        }

        private void LineThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            e.Handled = true;
        }

        //ハンドルの移動でCanvasのサイズを変更
        private void Handle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyCanvas.Width = Math.Max(1, MyCanvas.Width + e.HorizontalChange);
            MyCanvas.Height = Math.Max(1, MyCanvas.Height + e.VerticalChange);
            e.Handled = true;
        }

        #region 依存関係プロパティ

        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));


        public double MyOffsetLeft
        {
            get { return (double)GetValue(MyOffsetLeftProperty); }
            set { SetValue(MyOffsetLeftProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetLeftProperty =
            DependencyProperty.Register(nameof(MyOffsetLeft), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));

        public double MyOffsetTop
        {
            get { return (double)GetValue(MyOffsetTopProperty); }
            set { SetValue(MyOffsetTopProperty, value); }
        }
        public static readonly DependencyProperty MyOffsetTopProperty =
            DependencyProperty.Register(nameof(MyOffsetTop), typeof(double), typeof(LineThumb), new PropertyMetadata(0.0));


        public Canvas MyCanvas
        {
            get { return (Canvas)GetValue(MyCanvasProperty); }
            set { SetValue(MyCanvasProperty, value); }
        }
        public static readonly DependencyProperty MyCanvasProperty =
            DependencyProperty.Register(nameof(MyCanvas), typeof(Canvas), typeof(LineThumb), new PropertyMetadata(null));

        public EzLine MyEzLine
        {
            get { return (EzLine)GetValue(MyEzLineProperty); }
            set { SetValue(MyEzLineProperty, value); }
        }
        public static readonly DependencyProperty MyEzLineProperty =
            DependencyProperty.Register(nameof(MyEzLine), typeof(EzLine), typeof(LineThumb), new PropertyMetadata(null));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(LineThumb), new PropertyMetadata(null));
        #endregion 依存関係プロパティ

        /// <summary>
        /// 再描画
        /// </summary>
        public void Relayout()
        {
            var r4 = MyEzLine.MyBounds4;
            Width = r4.Width;
            Height = r4.Height;

            //変更する前の位置を使って計算しておく、タイミング重要
            double tasLeft = Canvas.GetLeft(MyEzLine) + Canvas.GetLeft(this) + r4.Left;
            double tasTop = Canvas.GetTop(MyEzLine) + Canvas.GetTop(this) + r4.Top;

            //図形の位置を変更、オフセット
            Canvas.SetLeft(MyEzLine, -r4.Left);
            Canvas.SetTop(MyEzLine, -r4.Top);

            //自身の位置を変更、図形の位置に合わせる
            Canvas.SetLeft(this, tasLeft);
            Canvas.SetTop(this, tasTop);
            //Canvas.SetLeft(this, Canvas.GetLeft(this) + tasLeft);
            //Canvas.SetTop(this, Canvas.GetTop(this) + tasTop);

        }

        /// <summary>
        /// アンカーハンドルの表示切替
        /// </summary>
        public void AdornerSwitch()
        {
            if (AdornerLayer.GetAdornerLayer(MyEzLine) is AdornerLayer layer)
            {
                Adorner[] ados = layer.GetAdorners(MyEzLine);
                //無ければ追加(表示)
                if (ados is null)
                {
                    layer.Add(new EzLineAdorner(MyEzLine));
                }
                //在れば削除
                else
                {
                    foreach (var item in ados.OfType<EzLineAdorner>())
                    {
                        layer.Remove(item);
                    }
                }
            }
        }
    }

    #region コンバーター
    
    public class MyConvRectWidth : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConvRectHeight : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return r.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterLeftRectLeft : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = (double)values[0];
            var r = (Rect)values[1];
            return left + r.Left;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterTopRectTop : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var top = (double)values[0];
            var r = (Rect)values[1];
            return top + r.Top;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConverterNegativeRectLeft : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.X;
            //return -r.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyConverterNegativeRectTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return -r.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion コンバーター


}