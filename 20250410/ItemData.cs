using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _20250410
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
        public ItemData(ItemType type) { Type = type; }


        private ItemType _Type;
        public ItemType Type { get => _Type; set => SetProperty(ref _Type, value); }



        /// <summary>
        /// 横位置の指定
        /// </summary>
        private double _Left;
        public double Left { get => _Left; set => SetProperty(ref _Left, value); }

        private double _Top;
        public double Top { get => _Top; set => SetProperty(ref _Top, value); }

        private string _Text = string.Empty;
        public string Text { get => _Text; set => SetProperty(ref _Text, value); }

        /// <summary>
        /// 中の要素の回転角度、RotateTransformのAngleにバインドする用
        /// </summary>
        private double _Angle;
        public double Angle { get => _Angle; set => SetProperty(ref _Angle, value); }

        /// <summary>
        /// 中の要素の拡大率、ScaleTransformのScaleXにバインドする用
        /// </summary>
        private double _ScaleX = 1.0;
        public double ScaleX { get => _ScaleX; set => SetProperty(ref _ScaleX, value); }

        private double _ScaleY = 1.0;
        public double ScaleY { get => _ScaleY; set => SetProperty(ref _ScaleY, value); }


        private double _CenterX;
        public double CenterX { get => _CenterX; set => SetProperty(ref _CenterX, value); }

        private double _CenterY;
        public double CenterY { get => _CenterY; set => SetProperty(ref _CenterY, value); }


        //アンカーポイント群
        //通知プロパティだとリアルタイムで動作確認できないので依存関係プロパティにしている
        [DataMember]
        public PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(ItemData), new PropertyMetadata(null));






        public override string ToString()
        {
            return Type.ToString();
        }
    }
}