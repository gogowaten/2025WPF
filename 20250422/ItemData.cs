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
using System.Windows.Media.Imaging;


namespace _20250422
{

    //Thumbの種類の識別用
    public enum ThumbType { None = 0, Root, Group, Text, Ellipse, Rect, GeoShape, Image }





    //[KnownType(typeof(SolidColorBrush))]
    //[KnownType(typeof(MatrixTransform))]
    public class ItemDataKiso : DependencyObject, IExtensibleDataObject, INotifyPropertyChanged
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

    }


    /// <summary>
    /// ManageExCanvas用の設定保存データ
    /// </summary>
    public class ManageData : ItemDataKiso
    {
        public ManageData() { MyBindBrushes(); }

        #region 通知プロパティ、依存関係プロパティ

        #region 範囲選択Thumb用

        private double _areaLeft;
        public double AreaLeft { get => _areaLeft; set => SetProperty(ref _areaLeft, value); }

        private double _areaTop;
        public double AreaTop { get => _areaTop; set => SetProperty(ref _areaTop, value); }

        private double _areaThumbWidth = 100.0;
        public double AreaThumbWidth { get => _areaThumbWidth; set => SetProperty(ref _areaThumbWidth, value); }

        private double _areaThumbHeight = 100.0;
        public double AreaThumbHeight { get => _areaThumbHeight; set => SetProperty(ref _areaThumbHeight, value); }

        private Visibility _areaThumbVisibility = Visibility.Collapsed;
        public Visibility AreaThumbVisibility { get => _areaThumbVisibility; set => SetProperty(ref _areaThumbVisibility, value); }

        private double _areaThumbOpacity = 0.3;
        public double AreaThumbOpacity { get => _areaThumbOpacity; set => SetProperty(ref _areaThumbOpacity, value); }


        private byte _areaThumbBackgroundA = 255;
        [DataMember] public byte AreaThumbBackgroundA { get => _areaThumbBackgroundA; set => SetProperty(ref _areaThumbBackgroundA, value); }
        private byte _areaThumbBackgroundR;
        [DataMember] public byte AreaThumbBackgroundR { get => _areaThumbBackgroundR; set => SetProperty(ref _areaThumbBackgroundR, value); }
        private byte _areaThumbBackgroundG;
        [DataMember] public byte AreaThumbBackgroundG { get => _areaThumbBackgroundG; set => SetProperty(ref _areaThumbBackgroundG, value); }
        private byte _areaThumbBackgroundB;
        [DataMember] public byte AreaThumbBackgroundB { get => _areaThumbBackgroundB; set => SetProperty(ref _areaThumbBackgroundB, value); }

        //オプションでBindsTwoWayByDefault必須、Binding時にはTwoWayに設定しても反映されないので、ここで指定
        [IgnoreDataMember]
        public Brush AreaThumbBackground
        {
            get { return (Brush)GetValue(AreaThumbBackgroundProperty); }
            set { SetValue(AreaThumbBackgroundProperty, value); }
        }
        public static readonly DependencyProperty AreaThumbBackgroundProperty =
            DependencyProperty.Register(nameof(AreaThumbBackground), typeof(Brush), typeof(ManageData),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion 範囲選択Thumb用
        #endregion 通知プロパティ、依存関係プロパティ

        private void MyBindBrushes()
        {
            var mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(AreaThumbBackgroundA)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(AreaThumbBackgroundR)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(AreaThumbBackgroundG)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(AreaThumbBackgroundB)) { Source = this });
            _ = BindingOperations.SetBinding(this, AreaThumbBackgroundProperty, mb);
        }

    }


    //[DataContract]
    [KnownType(typeof(ItemData))]

    [DebuggerDisplay("{MyThumbType}")]
    //public class ItemData : DependencyObject, IExtensibleDataObject, INotifyPropertyChanged
    public class ItemData : ItemDataKiso
    {

        public ItemData()
        {
            MyInitBind();
        }
        public ItemData(ThumbType type) : this()
        {
            MyThumbType = type;
        }

        #region シリアライズ


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
                data.MyGuid = Guid.NewGuid().ToString();
                return data;
            }
            else { return null; }
        }

        #endregion シリアライズ


        #region ブラシのバインド初期設定


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

            mb = new MultiBinding() { Converter = new MyConverterARGBtoSolidBrush() };
            mb.Bindings.Add(new Binding(nameof(MyStrokeA)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyStrokeR)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyStrokeG)) { Source = this });
            mb.Bindings.Add(new Binding(nameof(MyStrokeB)) { Source = this });
            _ = BindingOperations.SetBinding(this, MyStrokeProperty, mb);

        }

        #endregion ブラシのバインド初期設定


        #region 特殊


        private ObservableCollection<ItemData> _myThumbsItemData = [];
        public ObservableCollection<ItemData> MyThumbsItemData { get => _myThumbsItemData; set => SetProperty(ref _myThumbsItemData, value); }



        private ThumbType _myThumbType;
        [DataMember] public ThumbType MyThumbType { get => _myThumbType; set => SetProperty(ref _myThumbType, value); }

        [DataMember] public string MyGuid { get; set; } = Guid.NewGuid().ToString();

        [IgnoreDataMember] private BitmapSource? _myBitmapSource;
        [IgnoreDataMember] public BitmapSource? MyBitmapSource { get => _myBitmapSource; set => SetProperty(ref _myBitmapSource, value); }


        #endregion 特殊

        #region 図形Geometry系


        private HeadType _myGeoShapeHeadCapType = HeadType.None;
        public HeadType MyGeoShapeHeadCapType { get => _myGeoShapeHeadCapType; set => SetProperty(ref _myGeoShapeHeadCapType, value); }


        private double _myStrokeThickness = 10.0;
        public double MyStrokeThickness { get => _myStrokeThickness; set => SetProperty(ref _myStrokeThickness, value); }


        private ShapeType _myShapeType = ShapeType.Line;
        public ShapeType MyShapeType { get => _myShapeType; set => SetProperty(ref _myShapeType, value); }



        //アンカーポイント群
        //通知プロパティだとリアルタイムで動作確認できないので依存関係プロパティにしている
        [DataMember]
        public PointCollection MyPoints
        {
            get { return (PointCollection)GetValue(MyPointsProperty); }
            set { SetValue(MyPointsProperty, value); }
        }
        public static readonly DependencyProperty MyPointsProperty =
            DependencyProperty.Register(nameof(MyPoints), typeof(PointCollection), typeof(ItemData), new PropertyMetadata(null));


        #endregion 図形Geometry系


        #region 共通

        private double _myAngle;
        public double MyAngle { get => _myAngle; set => SetProperty(ref _myAngle, value); }


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


        #endregion 共通

        #region ブラシ

        private byte _myFillA;
        [DataMember] public byte MyFillA { get => _myFillA; set => SetProperty(ref _myFillA, value); }
        private byte _myFillR;
        [DataMember] public byte MyFillR { get => _myFillR; set => SetProperty(ref _myFillR, value); }
        private byte _myFillG;
        [DataMember] public byte MyFillG { get => _myFillG; set => SetProperty(ref _myFillG, value); }
        private byte _myFillB;
        [DataMember] public byte MyFillB { get => _myFillB; set => SetProperty(ref _myFillB, value); }
        //オプションでBindsTwoWayByDefault必須、Binding時にはTwoWayに設定しても反映されないので、ここで指定
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
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));// TwoWay


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
        public Brush? MyBackground
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

        private byte _myStrokeA;
        [DataMember] public byte MyStrokeA { get => _myStrokeA; set => SetProperty(ref _myStrokeA, value); }
        private byte _myStrokeR;
        [DataMember] public byte MyStrokeR { get => _myStrokeR; set => SetProperty(ref _myStrokeR, value); }
        private byte _myStrokeG;
        [DataMember] public byte MyStrokeG { get => _myStrokeG; set => SetProperty(ref _myStrokeG, value); }
        private byte _myStrokeB;
        [DataMember] public byte MyStrokeB { get => _myStrokeB; set => SetProperty(ref _myStrokeB, value); }

        [IgnoreDataMember]
        public Brush MyStroke
        {
            get { return (Brush)GetValue(MyStrokeProperty); }
            set { SetValue(MyStrokeProperty, value); }
        }
        public static readonly DependencyProperty MyStrokeProperty =
            DependencyProperty.Register(nameof(MyStroke), typeof(Brush), typeof(ItemData),
                new FrameworkPropertyMetadata(Brushes.Red,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));




        #endregion ブラシ

        #region テキスト系


        private string _myText = string.Empty;
        [DataMember] public string MyText { get => _myText; set => SetProperty(ref _myText, value); }


        private double _myFontSize = SystemFonts.MessageFontSize;
        [DataMember] public double MyFontSize { get => _myFontSize; set => SetProperty(ref _myFontSize, value); }

        #endregion テキスト系

        #region Root用

        private int _myJpegQuality;
        public int MyJpegQuality { get => _myJpegQuality; set => SetProperty(ref _myJpegQuality, value); }

        private double _myAddOffsetLeft = 32;
        public double MyAddOffsetLeft { get => _myAddOffsetLeft; set => SetProperty(ref _myAddOffsetLeft, value); }

        private double _myAddOffsetTop = 32;
        public double MyAddOffsetTop { get => _myAddOffsetTop; set => SetProperty(ref _myAddOffsetTop, value); }

        #endregion Root用

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

    }


}