using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace _20250308
{
    //[028722]ベジエ曲線の各部の名称
    //https://support.justsystems.com/faq/1032/app/servlet/qadoc?QID=028722

    //ベジェ曲線の方向線とアンカーポイント、制御点を表示してみた - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/15547295
    //WPF、ベジェ曲線で直線表示、アンカー点の追加と削除 - 午後わてんのブログ
    //https://gogowaten.hatenablog.com/entry/2022/06/14/132217

    /// <summary>
    /// ベジェ曲線の方向線表示用、2色破線
    /// OnRenderで直線描画、その上にDefiningGeometryで破線描画
    /// </summary>

    class TwoColorDashLine : Shape
    {
        public TwoColorDashLine()
        {
            Stroke = Brushes.White;
            StrokeThickness = 2.0;
            StrokeDashArray = [5.0];
            SetMyBind();
            Loaded += TwoColorDashLine_Loaded;
        }

        private void TwoColorDashLine_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void SetMyBind()
        {
            var mb = new MultiBinding() {Converter = new MyConvLinePen() };
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(MyStrokeBaseProperty) });
            mb.Bindings.Add(new Binding() { Source = this, Path = new PropertyPath(StrokeThicknessProperty) });
            SetBinding(MyPenProperty, mb);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            for (int i = 0; i < MyPoints.Count-1; i++)
            {
                if ((i - 1) % 3 != 0)
                {
                    drawingContext.DrawLine(MyPen, MyPoints[i], MyPoints[i + 1]);
                }
            }
            base.OnRender(drawingContext);
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null) { return Geometry.Empty; }
                StreamGeometry geo = new();
                using var context = geo.Open();
                for (int i = 0; i < MyPoints.Count-1; i++)
                {
                    if ((i - 1) % 3 != 0)
                    {
                        context.BeginFigure(MyPoints[i], isFilled: false, isClosed: false);
                        context.LineTo(MyPoints[i + 1], isStroked: true, isSmoothJoin: false);
                    }
                }
                geo.Freeze();
                return geo;
            }
        }


        #region 依存関係プロパティ


        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(TwoColorDashLine), new PropertyMetadata(null));


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(TwoColorDashLine),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public SolidColorBrush MyStrokeBase
        {
            get { return (SolidColorBrush)GetValue(MyStrokeBaseProperty); }
            set { SetValue(MyStrokeBaseProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeBaseProperty =
            DependencyProperty.Register(nameof(MyStrokeBase), typeof(SolidColorBrush), typeof(TwoColorDashLine), new PropertyMetadata(Brushes.Black));
        #endregion 依存関係プロパティ

    }


    #region コンバーター
    public class MyConvLinePen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var stroke = (SolidColorBrush)values[0];
            var thickness = (double)values[1];
            return new Pen(stroke, thickness);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion コンバーター
}
