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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _20250201
{

    [ContentProperty(nameof(MyData))]
    public  class KisoThumb : Thumb
    {
        static KisoThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KisoThumb),new FrameworkPropertyMetadata(typeof(KisoThumb)));
        }
        public KisoThumb() {
            //MyData = new();
            DataContext = MyData;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Canvas") is Canvas panel)
            {
                MyBaseCanvas = panel;
            }
        }

        public ItemData MyData
        {
            get { return (ItemData)GetValue(MyDataProperty); }
            set { SetValue(MyDataProperty, value); }
        }
        public static readonly DependencyProperty MyDataProperty =
            DependencyProperty.Register(nameof(MyData), typeof(ItemData), typeof(KisoThumb),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure ));


        #region 依存関係プロパティ

        #region 読み取り専用

        private static readonly DependencyPropertyKey MyBaseCanvasPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MyBaseCanvas), typeof(Canvas), typeof(KisoThumb), new PropertyMetadata(null));
        public static readonly DependencyProperty MyBaseCanvasProperty = MyBaseCanvasPropertyKey.DependencyProperty;
        public Canvas MyBaseCanvas
        {
            get { return (Canvas)GetValue(MyBaseCanvasPropertyKey.DependencyProperty); }
            internal set { SetValue(MyBaseCanvasPropertyKey, value); }
        }


        #endregion 読み取り専用

        #region 通常



        #endregion 通常


        #endregion 依存関係プロパティ
    }

    public class EzLineThumb : KisoThumb
    {
        static EzLineThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EzLineThumb), new FrameworkPropertyMetadata(typeof(EzLineThumb)));
        }
        public EzLineThumb()
        {

        }

    }
}
