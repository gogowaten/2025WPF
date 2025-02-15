using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _20250215
{
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

        [DataMember] public string MyGuid { get; set; } = Guid.NewGuid().ToString();

        private double _myLeft;
        [DataMember] public double MyLeft { get => _myLeft; set => SetProperty(ref _myLeft, value); }

        private double _myTop;
        [DataMember] public double MyTop { get => _myTop; set => SetProperty(ref _myTop, value); }


        private string _myText = string.Empty;
        [DataMember] public string MyText { get => _myText; set => SetProperty(ref _myText, value); }
        


    }
}
