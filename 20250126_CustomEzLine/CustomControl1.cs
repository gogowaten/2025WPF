using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
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

namespace _20250126_CustomEzLine
{

    //public class CustomControl1 : Control
    //{
    //    static CustomControl1()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
    //    }
    //}

    public class EzLineShape : Shape
    {
        #region 依存関係プロパティ
        // FrameworkPropertyMetadataOptions.AffectsRender // デザイン画面上での更新で必要
        // FrameworkPropertyMetadataOptions.AffectsMeasure // 必要ないかも？



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLineShape),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsFilled
        {
            get { return (bool)GetValue(MyIsFilledProperty); }
            set { SetValue(MyIsFilledProperty, value); }
        }
        public static readonly DependencyProperty MyIsFilledProperty =
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLineShape), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));


        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzLineShape),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLineShape),
                new FrameworkPropertyMetadata(FillRule.Nonzero,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        //読み取り専用のPen、なぜかバインドできない、エラーになる
        //private static readonly DependencyPropertyKey MyPenPropertyKey =
        //    DependencyProperty.RegisterReadOnly(nameof(MyPen), typeof(Pen), typeof(EzLine), new PropertyMetadata(null));
        //public static readonly DependencyProperty MyPenProperty = MyPenPropertyKey.DependencyProperty;
        //public Pen MyPen
        //{
        //    get { return (Pen)GetValue(MyPenPropertyKey.DependencyProperty); }
        //    internal set { SetValue(MyPenPropertyKey, value); }
        //}

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLineShape), new FrameworkPropertyMetadata(new Pen(),
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure));



        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            set { SetValue(MySegmentPointsProperty, value); }
        }
        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLineShape), new PropertyMetadata(new PointCollection()));



        #endregion 依存関係プロパティ

        #region 読み取り専用依存関係プロパティ

        /// <summary>
        /// Pen(Stroke)の太さを考慮した位置とサイズ
        /// </summary>
        private static readonly DependencyPropertyKey MyBoundsWithPenPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBoundsWithPen), typeof(Rect), typeof(EzLineShape), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyBoundsWithPenProperty = MyBoundsWithPenPropertyKey.DependencyProperty;
        public Rect MyBoundsWithPen
        {
            get { return (Rect)GetValue(MyBoundsWithPenPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsWithPenPropertyKey, value); }
        }

        //GeometryのRect、不必要
        private static readonly DependencyPropertyKey MyBoundsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBounds), typeof(Rect), typeof(EzLineShape), new PropertyMetadata(new Rect()));
        public static readonly DependencyProperty MyBoundsProperty = MyBoundsPropertyKey.DependencyProperty;
        public Rect MyBounds
        {
            get { return (Rect)GetValue(MyBoundsPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBoundsPropertyKey, value); }
        }


        #endregion 読み取り専用依存関係プロパティ



        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints is null || MyPoints.Count == 0)
                {
                    return Geometry.Empty;
                }

                //書き換えのたびにここでnewするのが気になる
                StreamGeometry geo = new() { FillRule = MyFillRule };
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], MyIsFilled, MyIsClosed);
                    context.PolyLineTo(MySegmentPoints, MyIsStroked, MyIsSmoothJoin);
                }
                geo.Freeze();
                return geo;
            }
        }

        static EzLineShape()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineShape), new FrameworkPropertyMetadata(typeof(EzLineShape)));
        }
        public EzLineShape()
        {
            DataContext = this;
            
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            SetBounds();            
        }


        private void SetBounds()
        {
            //GetWidenedPathGeometry(Pen).Boundsと同じ値、線の描画を考慮したものが取得できる
            Rect bounds = DefiningGeometry.GetRenderBounds(MyPen);
            MyBoundsWithPen = bounds;

            //var rSize = RenderSize;//広がりは追随、縮小は無反応
            //MyBounds = new Rect(0, 0, rSize.Width, rSize.Height);

            Rect georect = DefiningGeometry.Bounds;
            MyBounds = georect;
        }

    }





    #region コンバーター



    ///// <summary>
    ///// PointCollectionの最初の要素を返す、PathFigureのStartPoint用
    ///// </summary>
    //public class MyConverterStratPoint : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is PointCollection pc && pc.Count > 0)
    //        {
    //            return pc[0];
    //        }
    //        return new Point();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    /// <summary>
    /// PointCollectionをクローンして、その最初の要素を除いて返す
    /// </summary>
    public class MyConverterSegment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc)
            {
                if (pc.Count >= 2)
                {
                    PointCollection cpc = pc.Clone();
                    cpc.RemoveAt(0);
                    return cpc;
                }
                else
                {
                    return pc;
                }
            }
            else
            {
                return new PointCollection();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 太さから透明色のPenを作って返す
    /// </summary>
    public class MyConverterPen : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double bold = (double)value;
            return new Pen(Brushes.Transparent, bold);
        }

        //こっちは未使用になるはず        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // /// <summary>
    ///// ブラシと太さからPenを作って返す
    ///// </summary>
    //public class MyCovnerterPen : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        Brush b = (Brush)values[0];
    //        double thickness = (double)values[1];
    //        return new Pen(b, thickness);
    //    }

    //    //こっちは未使用になるはず
    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //        //Pen pen = (Pen)value;
    //        //return [pen.Brush, pen.Thickness];
    //    }
    //}

    #endregion コンバーター
}
