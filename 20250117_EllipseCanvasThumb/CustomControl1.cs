using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace _20250117_EllipseCanvasThumb
{


    public class CanvasThumb : Thumb
    {
        private readonly Thumb MyThumb;
        private const double MinimumSize = 1;
        private const double MinimumLocate = 0;
        private const double ThumbSize = 20;
        static CanvasThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CanvasThumb), new FrameworkPropertyMetadata(typeof(CanvasThumb)));
        }
        public CanvasThumb()
        {
            MyThumb = new()
            {
                Width = ThumbSize,
                Height = ThumbSize,
                Cursor = Cursors.SizeNWSE
            };
            MyThumb.DragDelta += Thumb_DragDelta;
            DragDelta += Thumb_DragDelta;
            SetInitialPosition();
        }
        private void SetInitialPosition()
        {
            Canvas.SetLeft(MyThumb, MinimumLocate);
            Canvas.SetTop(MyThumb, MinimumLocate);
            Canvas.SetLeft(this, MinimumLocate);
            Canvas.SetTop(this, MinimumLocate);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Thumb t)
            {
                if (t == MyThumb)
                {
                    //最小サイズ未満にならないようにThumbの移動
                    Canvas.SetLeft(t, Math.Max(MinimumSize, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumSize, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
                else if (t == this)
                {
                    //最小座標未満にならないように自身の移動
                    Canvas.SetLeft(t, Math.Max(MinimumLocate, Canvas.GetLeft(t) + e.HorizontalChange));
                    Canvas.SetTop(t, Math.Max(MinimumLocate, Canvas.GetTop(t) + e.VerticalChange));
                    e.Handled = true;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            //Templateの中のCanvasを取得してMyThumbを追加とBinding処理
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                panel.Children.Add(MyThumb);

                //バインド
                //自身のサイズをソースにMyThumbの座標をバインド
                MyThumb.DataContext = this;
                _ = MyThumb.SetBinding(Canvas.LeftProperty,
                    new Binding(nameof(Width)) { Mode = BindingMode.TwoWay });
                _ = MyThumb.SetBinding(Canvas.TopProperty,
                    new Binding(nameof(Height)) { Mode = BindingMode.TwoWay });
            }
        }
    }


    public class EllipseThumb : CanvasThumb
    {
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(null));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(EllipseThumb), new PropertyMetadata(null));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(EllipseThumb), new PropertyMetadata(1.0));

        static EllipseThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseThumb), new FrameworkPropertyMetadata(typeof(EllipseThumb)));
        }
        public EllipseThumb()
        {

        }
    }

    
    public class EllipseTextThumb : EllipseThumb
    {

        public Brush TextBackground
        {
            get { return (Brush)GetValue(TextBackgroundProperty); }
            set { SetValue(TextBackgroundProperty, value); }
        }
        public static readonly DependencyProperty TextBackgroundProperty =
            DependencyProperty.Register(nameof(TextBackground), typeof(Brush), typeof(EllipseTextThumb), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EllipseTextThumb), new PropertyMetadata(string.Empty));


        static EllipseTextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseTextThumb), new FrameworkPropertyMetadata(typeof(EllipseTextThumb)));
        }
        public EllipseTextThumb()
        {

        }
    }


}
