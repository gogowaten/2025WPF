using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _20250404
{
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
        public ItemData() { }

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

    }
}
