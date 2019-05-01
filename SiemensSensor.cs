using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class SiemensSensor : Sensor
    {
        private string source;
        private string adress;

        public SiemensSensor()
        {
            ID = 0;
            source = "";
            name = "";
            adress = "";
            deviceType = 0;
        }
        public SiemensSensor(string source, string name, string adress, InputDeviceType deviceType)
        {
            this.source = source;
            this.name = name;
            this.adress = adress;
            this.deviceType = deviceType;
        }

        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
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
            get => adress;
            set
            {
                adress = value;
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