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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250303_LineThumbManualRelayout
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
            Relayout();
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
            Width = Math.Max(1, Width + e.HorizontalChange);
            Height = Math.Max(1, Height + e.VerticalChange);
            e.Handled = true;
        }



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



        public void Relayout()
        {
            var r4 = MyEzLine.MyBounds4;
            this.Width = r4.Width;
            this.Height = r4.Height;
            double imaLeft = Canvas.GetLeft(MyEzLine);
            double imaTop = Canvas.GetTop(MyEzLine);
            double tasLeft = imaLeft + r4.Left;
            double tasTop = imaTop + r4.Top;


            Canvas.SetLeft(MyEzLine, -r4.Left);
            Canvas.SetTop(MyEzLine, -r4.Top);
            Canvas.SetLeft(this, Canvas.GetLeft(this) + tasLeft);
            Canvas.SetTop(this, Canvas.GetTop(this) + tasTop);
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



}
