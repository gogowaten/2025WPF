using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _20250216_Serialize
{
    public enum ItemType { None = 0, Text }

    [DataContract]
    public class ItemData : IExtensibleDataObject, INotifyPropertyChanged
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


        private ItemType _myItemType;
        [DataMember] public ItemType MyItemType { get => _myItemType; set => SetProperty(ref _myItemType, value); }


        [DataMember] public string MyGuid { get; set; } = Guid.NewGuid().ToString();

        private double _myLeft;
        [DataMember] public double MyLeft { get => _myLeft; set => SetProperty(ref _myLeft, value); }

        private double _myTop;
        [DataMember] public double MyTop { get => _myTop; set => SetProperty(ref _myTop, value); }


        private string _myText = string.Empty;
        [DataMember] public string MyText { get => _myText; set => SetProperty(ref _myText, value); }



        private byte _myForegroundA;
        [DataMember] public byte MyForegroundA { get => _myForegroundA; set => SetProperty(ref _myForegroundA, value); }
        private byte _myForegroundR;
        [DataMember] public byte MyForegroundR { get => _myForegroundR; set => SetProperty(ref _myForegroundR, value); }
        private byte _myForegroundG;
        [DataMember] public byte MyForegroundG { get => _myForegroundG; set => SetProperty(ref _myForegroundG, value); }
        private byte _myForegroundB;
        [DataMember] public byte MyForegroundB { get => _myForegroundB; set => SetProperty(ref _myForegroundB, value); }


        private byte _myBackgroundA;
        [DataMember] public byte MyBackgroundA { get => _myBackgroundA; set => SetProperty(ref _myBackgroundA, value); }
        private byte _myBackgroundR;
        [DataMember] public byte MyBackgroundR { get => _myBackgroundR; set => SetProperty(ref _myBackgroundR, value); }
        private byte _myBackgroundG;
        [DataMember] public byte MyBackgroundG { get => _myBackgroundG; set => SetProperty(ref _myBackgroundG, value); }
        private byte _myBackgroundB;
        [DataMember] public byte MyBackgroundB { get => _myBackgroundB; set => SetProperty(ref _myBackgroundB, value); }


    }
}
