using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _20250219
{
    public static class Serialize
    {
        private static void DataSerealize<T>(T data, string filePath)
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

        private static T? Deserialize<T>(string filePath)
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
}
