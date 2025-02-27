using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using System.Xml;

namespace _20250227_SerializeRootGroup
{

    //[DataContract]
    [KnownType(typeof(ItemData))]
    [KnownType(typeof(SolidColorBrush))]
    [KnownType(typeof(MatrixTransform))]

    [DebuggerDisplay("{MyThumbType}")]
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


        public ItemData()
        {
            MyInitBind();
        }
        public ItemData(ThumbType type) : this()
        {
            MyThumbType = type;
        }

        public bool Serialize(string filePath)
        {
            DataContractSerializer serializer = new(typeof(ItemData));
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            };
            using XmlWriter writer = XmlWriter.Create(filePath, settings);

            try
            {
                serializer.WriteObject(writer, this);
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static ItemData? Deserialize(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            DataContractSerializer serializer = new(typeof(ItemData));
            using XmlReader reader = XmlReader.Create(filePath);
            if (serializer.ReadObject(reader) is ItemData data)
            {
                return data;
            }
            else { return null; }
        }

        private void MyInitBind()
        {
            var mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyForegroundA)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyForegroundR)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyForegroundG)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyForegroundB)) { Source = this });
            _ = BindingOperations.SetBinding(this, MyForegroundProperty, mb);

            mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyBackgroundA)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundR)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundG)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyBackgroundB)) { Source = this });
            _ = BindingOperations.SetBinding(this, MyBackgroundProperty, mb);

            mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyFillA)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyFillR)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyFillG)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyFillB)) { Source = this });
            _ = BindingOperations.SetBinding(this, MyFillProperty, mb);

        }


        private ObservableCollection<ItemData> _myThumbsItemData = [];
        public ObservableCollection<ItemData> MyThumbsItemData { get => _myThumbsItemData; set => SetProperty(ref _myThumbsItemData, value); }


        [DataMember] public string MyGuid { get; private set; } = Guid.NewGuid().ToString();

        private ThumbType _myThumbType;
        [DataMember] public ThumbType MyThumbType { get => _myThumbType; set => SetProperty(ref _myThumbType, value); }




        #region 共通


        private double _myLeft = 0.0;
        [DataMember] public double MyLeft { get => _myLeft; set => SetProperty(ref _myLeft, value); }

        private double _myTop = 0.0;
        [DataMember] public double MyTop { get => _myTop; set => SetProperty(ref _myTop, value); }

        private int _myZIndex = 0;
        [DataMember] public int MyZIndex { get => _myZIndex; set => SetProperty(ref _myZIndex, value); }


        private double _myWidth;
        [DataMember] public double MyWidth { get => _myWidth; set => SetProperty(ref _myWidth, value); }

        private double _myHeight;
        [DataMember] public double MyHeight { get => _myHeight; set => SetProperty(ref _myHeight, value); }

        #region ブラシ

        private byte _myFillA;
        [DataMember] public byte MyFillA { get => _myFillA; set => SetProperty(ref _myFillA, value); }
        private byte _myFillR;
        [DataMember] public byte MyFillR { get => _myFillR; set => SetProperty(ref _myFillR, value); }
        private byte _myFillG;
        [DataMember] public byte MyFillG { get => _myFillG; set => SetProperty(ref _myFillG, value); }
        private byte _myFillB;
        [DataMember] public byte MyFillB { get => _myFillB; set => SetProperty(ref _myFillB, value); }
        //オプションでBindsTwoWayByDefault必須、ここ以外ではTwoWayにできない
        [IgnoreDataMember]
        public Brush MyFill
        {
            get { return (Brush)GetValue(MyFillProperty); }
            set { SetValue(MyFillProperty, value); }
        }
        public static readonly DependencyProperty MyFillProperty =
            DependencyProperty.Register(nameof(MyFill), typeof(Brush), typeof(ItemData),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private byte _myForegroundA = 255;
        [DataMember] public byte MyForegroundA { get => _myForegroundA; set => SetProperty(ref _myForegroundA, value); }
        private byte _myForegroundR;
        [DataMember] public byte MyForegroundR { get => _myForegroundR; set => SetProperty(ref _myForegroundR, value); }
        private byte _myForegroundG;
        [DataMember] public byte MyForegroundG { get => _myForegroundG; set => SetProperty(ref _myForegroundG, value); }
        private byte _myForegroundB;
        [DataMember] public byte MyForegroundB { get => _myForegroundB; set => SetProperty(ref _myForegroundB, value); }

        [IgnoreDataMember]
        public Brush MyForeground
        {
            get { return (Brush)GetValue(MyForegroundProperty); }
            set { SetValue(MyForegroundProperty, value); }
        }
        public static readonly DependencyProperty MyForegroundProperty =
            DependencyProperty.Register(nameof(MyForeground), typeof(Brush), typeof(ItemData),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private byte _myBackgroundA = 0;
        [DataMember] public byte MyBackgroundA { get => _myBackgroundA; set => SetProperty(ref _myBackgroundA, value); }
        private byte _myBackgroundR = 0;
        [DataMember] public byte MyBackgroundR { get => _myBackgroundR; set => SetProperty(ref _myBackgroundR, value); }
        private byte _myBackgroundG = 0;
        [DataMember] public byte MyBackgroundG { get => _myBackgroundG; set => SetProperty(ref _myBackgroundG, value); }
        private byte _myBackgroundB = 0;
        [DataMember] public byte MyBackgroundB { get => _myBackgroundB; set => SetProperty(ref _myBackgroundB, value); }

        [IgnoreDataMember]
        public Brush MyBackground
        {
            get { return (Brush)GetValue(MyBackgroundProperty); }
            set { SetValue(MyBackgroundProperty, value); }
        }
        public static readonly DependencyProperty MyBackgroundProperty =
            DependencyProperty.Register(nameof(MyBackground), typeof(Brush), typeof(ItemData),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion ブラシ

        #endregion 共通

        #region テキスト系


        private string _myText = string.Empty;
        [DataMember] public string MyText { get => _myText; set => SetProperty(ref _myText, value); }


        private double _myFontSize = SystemFonts.MessageFontSize;
        [DataMember] public double MyFontSize { get => _myFontSize; set => SetProperty(ref _myFontSize, value); }

        #endregion テキスト系

        #region 保存しない系


        private Visibility _isWakuVisible;
        public Visibility IsWakuVisible { get => _isWakuVisible; set => SetProperty(ref _isWakuVisible, value); }

        private bool _isActiveGroup;
        public bool IsActiveGroup { get => _isActiveGroup; set => SetProperty(ref _isActiveGroup, value); }

        private bool _isSelectable;
        public bool IsSelectable { get => _isSelectable; set => SetProperty(ref _isSelectable, value); }

        private bool _isSelected;
        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

        private bool _isFocus;
        public bool IsFocus { get => _isFocus; set => SetProperty(ref _isFocus, value); }

        #endregion 保存しない系

        #region Root用

        private double _myOffsetLeft = 32;
        public double MyOffsetLeft { get => _myOffsetLeft; set => SetProperty(ref _myOffsetLeft, value); }

        private double _myOffsetTop = 32;
        public double MyOffsetTop { get => _myOffsetTop; set => SetProperty(ref _myOffsetTop, value); }

        #endregion Root用
    }


}
