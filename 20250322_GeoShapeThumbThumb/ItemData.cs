using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _20250322_GeoShapeThumbThumb
{
    public class ItemData : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ItemData()
        {
            MyPoints = [new Point(), new Point(200, 20)];
        }


        private double _myStrokeThickness = 30.0;
        public double MyStrokeThickness { get => _myStrokeThickness; set => SetProperty(ref _myStrokeThickness, value); }


        private ShapeType _myShapeType;
        public ShapeType MyShapeType { get => _myShapeType; set => SetProperty(ref _myShapeType, value); }


        private double _myAnagle;
        public double MyAngle { get => _myAnagle; set => SetProperty(ref _myAnagle, value); }


        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ItemData), new PropertyMetadata(null));


    }
}