using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace _20250222_SerializeDependencyProperty
{
    //[DataContract] // これをつけるとシリアライズできない、なんで？
    [KnownType(typeof(SolidColorBrush))]
    [KnownType(typeof(MatrixTransform))]
    public class AAA : DependencyObject, INotifyPropertyChanged
    {
        public AAA()
        {
            MyAAAs = [];
        }

        //[IgnoreDataMember] // シリアル化対象外にしたい場合つける
        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(AAA), new PropertyMetadata(Brushes.Transparent));


        private List<string> _myTexts = [];
        public List<string> MyTexts { get => _myTexts; set => SetProperty(ref _myTexts, value); }


        public int MyInteger
        {
            get { return (int)GetValue(MyIntegerProperty); }
            set { SetValue(MyIntegerProperty, value); }
        }
        public static readonly DependencyProperty MyIntegerProperty =
            DependencyProperty.Register(nameof(MyInteger), typeof(int), typeof(AAA), new PropertyMetadata(0));


        public ObservableCollection<AAA> MyAAAs
        {
            get { return (ObservableCollection<AAA>)GetValue(MyAAAsProperty); }
            set { SetValue(MyAAAsProperty, value); }
        }
        public static readonly DependencyProperty MyAAAsProperty =
            DependencyProperty.Register(nameof(MyAAAs), typeof(ObservableCollection<AAA>), typeof(AAA), new PropertyMetadata(null));


        public void MySerialize(string filePath)
        {
            var setting = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };
            DataContractSerializer serializer = new(typeof(AAA));
            using (XmlWriter writer = XmlWriter.Create(filePath, setting))
            {
                serializer.WriteObject(writer, this);
            }
        }

        public static AAA? MyDeserialize(string filePath)
        {
            DataContractSerializer serializer = new(typeof(AAA));

            using (var reader = XmlReader.Create(filePath))
            {
                if (serializer.ReadObject(reader) is AAA result)
                {
                    return result;
                }
                else { return null; }
            }
        }

        #region 必要

        protected void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion 必要


    }
}
