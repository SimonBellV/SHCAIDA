using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public enum InputDeviceType
    {
        Sensor = 0,
        Regulator
    }
    [Serializable]
    public class Sensor : INotifyPropertyChanged
    {
        public InputDeviceType deviceType;
        public int ID { get; set; }
        public string Name;
        public string SourceName;
        public string SourcePath;
        public bool isStoragingEnable;
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
