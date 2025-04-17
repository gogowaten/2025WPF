using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _20250416
{

    public enum ItemType { None = 0, TextBlock, GeoShape }

    /// <summary>
    /// シリアライズするData
    /// </summary>
    public class ItemData : DependencyObject, IExtensibleDataObject, INotifyPropertyChanged
    {
        #region 必要
        public ExtensionDataObject? ExtensionData { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion 必要
        //public ItemData() { }
        public ItemData(ItemType type) { this.Type = type; }


        //図形

        private double _strokeThickness;
        public double StrokeThickness { get => _strokeThickness; set => SetProperty(ref _strokeThickness, value); }



        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ItemData), new PropertyMetadata(new PointCollection()));



        /// <summary>
        /// 横位置の指定
        /// </summary>
        private double _myLeft;
        public double MyLeft { get => _myLeft; set => SetProperty(ref _myLeft, value); }

        private double _myTop;
        public double MyTop { get => _myTop; set => SetProperty(ref _myTop, value); }

        private string _myText = string.Empty;
        public string MyText { get => _myText; set => SetProperty(ref _myText, value); }

        /// <summary>
        /// 中の要素の回転角度、RotateTransformのAngleにバインドする用
        /// </summary>
        private double _myAngle;
        public double MyAngle { get => _myAngle; set => SetProperty(ref _myAngle, value); }

        /// <summary>
        /// 中の要素の拡大率、ScaleTransformのScaleXにバインドする用
        /// </summary>
        private double _myScaleX = 1.0;
        public double MyScaleX { get => _myScaleX; set => SetProperty(ref _myScaleX, value); }

        private double _myScaleY = 1.0;
        public double MyScaleY { get => _myScaleY; set => SetProperty(ref _myScaleY, value); }


        private double _myCenterX;
        public double MyCenterX { get => _myCenterX; set => SetProperty(ref _myCenterX, value); }

        private double _myCenterY;
        public double MyCenterY { get => _myCenterY; set => SetProperty(ref _myCenterY, value); }


        private ItemType _type;
        public ItemType Type { get => _type; set => SetProperty(ref _type, value); }

    }
}