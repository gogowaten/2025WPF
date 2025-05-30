﻿using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace _20250216_Serialize;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string filePath = "E:\\20250215.xml";
        DataSerealize(MyTT.MyItemData, filePath);
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        string filePath = "E:\\20250215.xml";
        if (Deserialize<ItemData>(filePath) is ItemData data)
        {
            if (data.MyItemType == ItemType.Text)
            {
                MyCanvas.Children.Add(new TextThumb(data));
            }
        }
    }

    private void DataSerealize<T>(T data, string filePath)
    {
        XmlWriterSettings settings = new()
        {
            Indent = true,
            Encoding = new UTF8Encoding(false),
            NewLineOnAttributes = true
        };
        DataContractSerializer serializer = new(typeof(T));
        using XmlWriter writer = XmlWriter.Create(filePath, settings);
        try
        {
            serializer.WriteObject(writer, data);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    private T? Deserialize<T>(string filePath)
    {
        DataContractSerializer serializer = new(typeof(T));
        using XmlReader reader = XmlReader.Create(filePath);
        try
        {
            if (serializer.ReadObject(reader) is T t) { return t; }
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
        return default;
    }


}