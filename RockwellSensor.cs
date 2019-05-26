using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    class RockwellSensor : Sensor
    {
        public bool IsStoragingEnable
        {
            get { return isStoragingEnable; }
            set
            {
                isStoragingEnable = value;
                OnPropertyChanged("IsStoragingEnable");
            }
        }

        public string Source
        {
            get => SourceName;
            set
            {
                SourceName = value;
                OnPropertyChanged("Source");
            }
        }

        public new string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public InputDeviceType DeviceType
        {
            get => deviceType;
            set
            {
                deviceType = value;
                OnPropertyChanged("DeviceType");
            }
        }

        public string Adress
        {
            get => SourcePath;
            set
            {
                SourcePath = value;
                OnPropertyChanged("Adress");
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
