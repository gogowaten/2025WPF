﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace _20250212
{
    public class EzLine : Shape
    {

        protected override Geometry DefiningGeometry
        {
            get
            {
                if (MyPoints == null || MyPoints.Count == 0) { return Geometry.Empty; }
                StreamGeometry geo = new() { FillRule = MyFillRule };
                using (var context = geo.Open())
                {
                    context.BeginFigure(MyPoints[0], MyIsFilled, MyIsClosed);
                    context.PolyLineTo(MySegmentPoints, MyIsStroked, MyIsSmoothJoin);
                }
                geo.Freeze();

                //Boundsの更新はここで行う必要がある。OnRenderではなんか違う
                MyBounds1 = geo.Bounds;
                MyBounds2 = geo.GetRenderBounds(MyPen);
                //回転後のBounds
                var clone = geo.Clone();
                clone.Transform = RenderTransform;
                MyBounds3 = clone.Bounds;
                MyBounds4 = clone.GetRenderBounds(MyPen);

                return geo;
            }
        }

        public EzLine()
        {
            DataContext = this;
            SetBinding(MySegmentPointsProperty, new Binding() { Source = this, Path = new PropertyPath(MyPointsProperty), Mode = BindingMode.OneWay, Converter = new MyConverterSegmentPoints() });

            MyPenBind();
            MyRenderTransformBind();
            MyOffsetBind();
        }

        //オフセット移動値のバインド
        private void MyOffsetBind()
        {
            MultiBinding mb = new() { Converter = new MyConverterIsOffsetX() };
            mb.Bindings.Add(MakeOneWayBind(MyIsOffsetProperty));
            mb.Bindings.Add(MakeOneWayBind(MyBounds4Property));
            SetBinding(Canvas.LeftProperty, mb);

            mb = new() { Converter = new MyConverterIsOffsetY() };
            mb.Bindings.Add(MakeOneWayBind(MyIsOffsetProperty));
            mb.Bindings.Add(MakeOneWayBind(MyBounds4Property));
            SetBinding(Canvas.TopProperty, mb);
        }

        //Penのバインド、Penは図形のBoundsを計測するために必要
        private void MyPenBind()
        {
            MultiBinding mb = new() { Converter = new MyConverterPen() };
            mb.Bindings.Add(MakeOneWayBind(StrokeThicknessProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeMiterLimitProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeEndLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeStartLineCapProperty));
            mb.Bindings.Add(MakeOneWayBind(StrokeLineJoinProperty));
            SetBinding(MyPenProperty, mb);
        }

        //RenderTransformとのバインド、これでMyAngle変更時にもBoundsが更新される
        private void MyRenderTransformBind()
        {
            //中心軸座標、Penでの描画Boundsの中心にしているけど、それ以外にも試したい
            //三角形なら重心、四角形ならそのまま、それ以上なら平均座標とか
            SetBinding(MyCenterXProperty, new Binding() { Source = this, Path = new PropertyPath(MyBounds2Property), Converter = new MyConverterCenterX(), Mode = BindingMode.OneWay });
            SetBinding(MyCenterYProperty, new Binding() { Source = this, Path = new PropertyPath(MyBounds2Property), Converter = new MyConverterCenterY(), Mode = BindingMode.OneWay });

            //RenderTransformとのバインド、RenderTransformはRotateTransformに決め打ちしている
            MultiBinding mb = new() { Converter = new MyConverterRenderTransform() };
            mb.Bindings.Add(MakeOneWayBind(MyAngleProperty));
            mb.Bindings.Add(MakeOneWayBind(MyCenterXProperty));
            mb.Bindings.Add(MakeOneWayBind(MyCenterYProperty));
            SetBinding(RenderTransformProperty, mb);
        }

        private Binding MakeOneWayBind(DependencyProperty property)
        {
            return new Binding() { Source = this, Path = new PropertyPath(property), Mode = BindingMode.OneWay };
        }

        #region 依存関係プロパティ

        //オフセット移動フラグ
        //回転時にMyBounds4分を移動するかどうか
        //trueで移動、falseで処理無し
        public bool MyIsOffset
        {
            get { return (bool)GetValue(MyIsOffsetProperty); }
            set { SetValue(MyIsOffsetProperty, value); }
        }
        public static readonly DependencyProperty MyIsOffsetProperty =
            DependencyProperty.Register(nameof(MyIsOffset), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        #region 通常

        #region 回転

        public double MyAngle
        {
            get { return (double)GetValue(MyAngleProperty); }
            set { SetValue(MyAngleProperty, value); }
        }
        public static readonly DependencyProperty MyAngleProperty =
            DependencyProperty.Register(nameof(MyAngle), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyCenterX
        {
            get { return (double)GetValue(MyCenterXProperty); }
            set { SetValue(MyCenterXProperty, value); }
        }
        public static readonly DependencyProperty MyCenterXProperty =
            DependencyProperty.Register(nameof(MyCenterX), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MyCenterY
        {
            get { return (double)GetValue(MyCenterYProperty); }
            set { SetValue(MyCenterYProperty, value); }
        }
        public static readonly DependencyProperty MyCenterYProperty =
            DependencyProperty.Register(nameof(MyCenterY), typeof(double), typeof(EzLine),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 回転

        public FillRule MyFillRule
        {
            get { return (FillRule)GetValue(MyFillRuleProperty); }
            set { SetValue(MyFillRuleProperty, value); }
        }
        public static readonly DependencyProperty MyFillRuleProperty =
            DependencyProperty.Register(nameof(MyFillRule), typeof(FillRule), typeof(EzLine),
                new FrameworkPropertyMetadata(FillRule.EvenOdd,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsSmoothJoin
        {
            get { return (bool)GetValue(MyIsSmoothJoinProperty); }
            set { SetValue(MyIsSmoothJoinProperty, value); }
        }
        public static readonly DependencyProperty MyIsSmoothJoinProperty =
            DependencyProperty.Register(nameof(MyIsSmoothJoin), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool MyIsStroked
        {
            get { return (bool)GetValue(MyIsStrokedProperty); }
            set { SetValue(MyIsStrokedProperty, value); }
        }
        public static readonly DependencyProperty MyIsStrokedProperty =
            DependencyProperty.Register(nameof(MyIsStroked), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(EzLine),
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
            DependencyProperty.Register(nameof(MyIsFilled), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool MyIsClosed
        {
            get { return (bool)GetValue(MyIsClosedProperty); }
            set { SetValue(MyIsClosedProperty, value); }
        }
        public static readonly DependencyProperty MyIsClosedProperty =
            DependencyProperty.Register(nameof(MyIsClosed), typeof(bool), typeof(EzLine),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion 通常

        #region Private set

        //変形後の線描画のBounds
        //TFGeo.GetRenderBounds(MyPen);
        public Rect MyBounds4
        {
            get { return (Rect)GetValue(MyBounds4Property); }
            private set { SetValue(MyBounds4Property, value); }
        }
        public static readonly DependencyProperty MyBounds4Property =
            DependencyProperty.Register(nameof(MyBounds4), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        //変形後のGeometryBounds
        //TFGeo.Bounds;
        public Rect MyBounds3
        {
            get { return (Rect)GetValue(MyBounds3Property); }
            private set { SetValue(MyBounds3Property, value); }
        }
        public static readonly DependencyProperty MyBounds3Property =
            DependencyProperty.Register(nameof(MyBounds3), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));


        //線描画のBounds
        //GetRenderBounds(MyPen);
        public Rect MyBounds2
        {
            get { return (Rect)GetValue(MyBounds2Property); }
            private set { SetValue(MyBounds2Property, value); }
        }
        public static readonly DependencyProperty MyBounds2Property =
            DependencyProperty.Register(nameof(MyBounds2), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        //GeometryBounds
        public Rect MyBounds1
        {
            get { return (Rect)GetValue(MyBounds1Property); }
            private set { SetValue(MyBounds1Property, value); }
        }
        public static readonly DependencyProperty MyBounds1Property =
            DependencyProperty.Register(nameof(MyBounds1), typeof(Rect), typeof(EzLine), new PropertyMetadata(new Rect()));

        public Pen MyPen
        {
            get { return (Pen)GetValue(MyPenProperty); }
            private set { SetValue(MyPenProperty, value); }
        }
        public static readonly DependencyProperty MyPenProperty =
            DependencyProperty.Register(nameof(MyPen), typeof(Pen), typeof(EzLine), new PropertyMetadata(new Pen()));

        public PointCollection MySegmentPoints
        {
            get { return (PointCollection)GetValue(MySegmentPointsProperty); }
            private set { SetValue(MySegmentPointsProperty, value); }
        }


        public static readonly DependencyProperty MySegmentPointsProperty =
            DependencyProperty.Register(nameof(MySegmentPoints), typeof(PointCollection), typeof(EzLine), new PropertyMetadata(null));

        #endregion Private set

        #endregion 依存関係プロパティ



    }




    #region コンバーター

    public class MyConverterIsOffsetY : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var offset = (bool)values[0];
            var bounds = (Rect)values[1];
            if (offset) { return -bounds.Y; }
            else { return 0.0; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MyConverterIsOffsetX : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var offset = (bool)values[0];
            var bounds = (Rect)values[1];
            if (offset) { return -bounds.X; }
            else { return 0.0; }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //回転軸のY座標、見た目通りの矩形(Bounds2)の中央にしている
    public class MyConverterCenterY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.Y * 2 + r.Height) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MyConverterCenterX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var r = (Rect)value;
            return (r.X * 2 + r.Width) / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //RenderTransformはRotateTransformに決め打ちしている
    public class MyConverterRenderTransform : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var angle = (double)values[0];
            var x = (double)values[1];
            var y = (double)values[2];
            return new RotateTransform(angle, x, y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Penの生成、各種プロパティも反映
    public class MyConverterPen : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var thick = (double)values[0];
            var miter = (double)values[1];
            var end = (PenLineCap)values[2];
            var sta = (PenLineCap)values[3];
            var join = (PenLineJoin)values[4];
            Pen result = new(Brushes.Transparent, thick)
            {
                EndLineCap = end,
                StartLineCap = sta,
                LineJoin = join,
                MiterLimit = miter
            };
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Segment用のPointCollection生成
    //ソースに影響を与えないためにクローン作成して、その先頭要素を削除して返す
    public class MyConverterSegmentPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection pc && pc.Count > 0)
            {
                var clone = pc.Clone();
                clone.RemoveAt(0);
                return clone;
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
    #endregion コンバーター
}
