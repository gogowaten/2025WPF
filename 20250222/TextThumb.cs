using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Unicode;
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
//using System.Xaml;
using System.Xml;

namespace _20250222
{
    [KnownType(typeof(SolidColorBrush))]
    [KnownType(typeof(MatrixTransform))]
    [KnownType(typeof(Thumb))]
    [KnownType(typeof(TextThumb))]
    [DataContract(IsReference =false)]
    public class TextThumb : Thumb
    {
        static TextThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextThumb), new FrameworkPropertyMetadata(typeof(TextThumb)));
        }
    
        public TextThumb()
        {
            DataContext = this;
            Initialized += TextThumb_Initialized;
            DragDelta += TextThumb_DragDelta;
        }

        private void TextThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            MyLeft += e.HorizontalChange;
            MyTop += e.VerticalChange;
            e.Handled = true;
        }

        private void TextThumb_Initialized(object? sender, EventArgs e)
        {
            if (Foreground is SolidColorBrush solid)
            {
                var iro = solid.Color;
                MyForegroundA = iro.A;
                MyForegroundR = iro.R;
                MyForegroundG = iro.G;
                MyForegroundB = iro.B;
            }
            var mb = new MultiBinding() { Mode = BindingMode.TwoWay, Converter = new MyConverterARGBSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyForegroundA)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyForegroundR)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyForegroundG)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyForegroundB)) { Mode = BindingMode.TwoWay });
            SetBinding(ForegroundProperty, mb);

            if (Background is SolidColorBrush bg)
            {
                var iro = bg.Color;
                MyBackgroundA = iro.A;
                MyBackgroundR = iro.R;
                MyBackgroundG = iro.G;
                MyBackgroundB = iro.B;
            }
            mb = new MultiBinding() { Converter = new MyConverterARGBSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyBackgroundA)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundR)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundG)) { Mode = BindingMode.TwoWay });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundB)) { Mode = BindingMode.TwoWay });
            SetBinding(BackgroundProperty, mb);

        }

        public void MyXamlWriter(string filePath)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            };
            StringBuilder sb = new();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            XamlDesignerSerializationManager manager = new(writer) {XamlWriterMode = XamlWriterMode.Expression};
            XamlWriter.Save(this, manager);
            var neko = sb.ToString();    
        }

        public void MySerialize(string filePath)
        {
            DataContractSerializer serializer = new(typeof(TextThumb));
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            };
            using XmlWriter writer = XmlWriter.Create(filePath, settings);
            serializer.WriteObject(writer, this);
        }

        public TextThumb? MyDeserialize(string filePath)
        {
            DataContractSerializer serializer = new(typeof(TextThumb));
            using XmlReader reader = XmlReader.Create(filePath);
            if (serializer.ReadObject(reader) is TextThumb thumb)
            {
                return thumb;
            }
            else { return null; }
        }

        public double MyLeft
        {
            get { return (double)GetValue(MyLeftProperty); }
            set { SetValue(MyLeftProperty, value); }
        }
        public static readonly DependencyProperty MyLeftProperty =
            DependencyProperty.Register(nameof(MyLeft), typeof(double), typeof(TextThumb), new PropertyMetadata(0.0));

        public double MyTop
        {
            get { return (double)GetValue(MyTopProperty); }
            set { SetValue(MyTopProperty, value); }
        }
        public static readonly DependencyProperty MyTopProperty =
            DependencyProperty.Register(nameof(MyTop), typeof(double), typeof(TextThumb), new PropertyMetadata(0.0));

        public string MyText
        {
            get { return (string)GetValue(MyTextProperty); }
            set { SetValue(MyTextProperty, value); }
        }
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register(nameof(MyText), typeof(string), typeof(TextThumb), new PropertyMetadata(string.Empty));


        public byte MyForegroundA
        {
            get { return (byte)GetValue(MyForegroundAProperty); }
            set { SetValue(MyForegroundAProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundAProperty =
            DependencyProperty.Register(nameof(MyForegroundA), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)255));



        public byte MyForegroundR
        {
            get { return (byte)GetValue(MyForegroundRProperty); }
            set { SetValue(MyForegroundRProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundRProperty =
            DependencyProperty.Register(nameof(MyForegroundR), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));


        public byte MyForegroundG
        {
            get { return (byte)GetValue(MyForegroundGProperty); }
            set { SetValue(MyForegroundGProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundGProperty =
            DependencyProperty.Register(nameof(MyForegroundG), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

        public byte MyForegroundB
        {
            get { return (byte)GetValue(MyForegroundBProperty); }
            set { SetValue(MyForegroundBProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundBProperty =
            DependencyProperty.Register(nameof(MyForegroundB), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

        public byte MyBackgroundA
        {
            get { return (byte)GetValue(MyBackgroundAProperty); }
            set { SetValue(MyBackgroundAProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundAProperty =
            DependencyProperty.Register(nameof(MyBackgroundA), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

        public byte MyBackgroundR
        {
            get { return (byte)GetValue(MyBackgroundRProperty); }
            set { SetValue(MyBackgroundRProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundRProperty =
            DependencyProperty.Register(nameof(MyBackgroundR), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

        public byte MyBackgroundG
        {
            get { return (byte)GetValue(MyBackgroundGProperty); }
            set { SetValue(MyBackgroundGProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundGProperty =
            DependencyProperty.Register(nameof(MyBackgroundG), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

        public byte MyBackgroundB
        {
            get { return (byte)GetValue(MyBackgroundBProperty); }
            set { SetValue(MyBackgroundBProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundBProperty =
            DependencyProperty.Register(nameof(MyBackgroundB), typeof(byte), typeof(TextThumb), new PropertyMetadata((byte)0));

    }


    public class MyConverterARGBSolidBrush : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var a = (byte)values[0];
            var r = (byte)values[1];
            var g = (byte)values[2];
            var b = (byte)values[3];
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var bru = (Color)((SolidColorBrush)value).Color;
            return [bru.A, bru.R, bru.G, bru.B];
        }
    }
}
