using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Windows.Input;
using System.Runtime.Serialization.Json;
using System.Globalization;

namespace _20250222
{

    public class AAA : DependencyObject
    {

        public AAA()
        {

            MyDatas = [];
        }

        public int MyZIndex
        {
            get { return (int)GetValue(MyZIndexProperty); }
            set { SetValue(MyZIndexProperty, value); }
        }
        public static readonly DependencyProperty MyZIndexProperty =
            DependencyProperty.Register(nameof(MyZIndex), typeof(int), typeof(AAA),
                new FrameworkPropertyMetadata(0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [XmlIgnore]
        [IgnoreDataMember]
        public Brush MyBrush
        {
            get { return (Brush)GetValue(MyBrushProperty); }
            set { SetValue(MyBrushProperty, value); }
        }
        public static readonly DependencyProperty MyBrushProperty =
            DependencyProperty.Register(nameof(MyBrush), typeof(Brush), typeof(AAA), new PropertyMetadata(Brushes.Transparent));


        [DataMember]
        public ObservableCollection<AAA> MyDatas
        {
            get { return (ObservableCollection<AAA>)GetValue(MyDatasProperty); }
            set { SetValue(MyDatasProperty, value); }
        }
        public static readonly DependencyProperty MyDatasProperty =
            DependencyProperty.Register(nameof(MyDatas), typeof(ObservableCollection<AAA>), typeof(AAA), new PropertyMetadata(null));

        public void Serialize()
        {
           

            //using (var stream = new FileStream("E:\\20250222.xml", FileMode.OpenOrCreate, FileAccess.Write))
            //{
            //    var serializer = new XmlSerializer(typeof(AAA));
            //    serializer.Serialize(stream, this);
            //}

            using (var stream = new FileStream("E:\\20250222.xml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                var serializer = new DataContractSerializer(typeof(AAA));
                serializer.WriteObject(stream, this);
            }


            //string xaml = XamlWriter.Save(this);
            //File.WriteAllText("E:\\20250222.xaml", xaml);
        }




    }
    public class BrushConverter : JsonConverter<Brush>
    {
        public override Brush Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var color = reader.GetString();
            return (Brush)new BrushConverter().ConvertFromInvariantString(color);
        }

        public override void Write(Utf8JsonWriter writer, Brush value, JsonSerializerOptions options)
        {
            var color = value.ToString();
            writer.WriteStringValue(color);
        }

        private Brush ConvertFromInvariantString(string color)
        {
            return (Brush)new BrushConverter().ConvertFromInvariantString(color);
        }
    }

    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            
            return DateTimeOffset.ParseExact(reader.GetString()!, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToString(
                    "MM/dd/yyyy", CultureInfo.InvariantCulture));
    }

}
