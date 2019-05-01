using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class SiemensSensor : Sensor
    {
        public SiemensSensor()
        {
            ID = 0;
            SourceName = "";
            base.Name = "";
            SourcePath = "";
            deviceType = 0;
        }
        public SiemensSensor(string source, string name, string adress, InputDeviceType deviceType)
        {
            SourceName = source;
            this.Name = name;
            this.SourcePath = adress;
            this.deviceType = deviceType;
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
            set {
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