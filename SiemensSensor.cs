using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class SiemensSensor : Sensor
    {
        public int ID { get; set; }
        private string source;
        private string name;
        private string adress;

        public SiemensSensor()
        {
            ID = 0;
            source = "";
            name = "";
            adress = "";
        }
        public SiemensSensor(string source, string name, string adress)
        {
            this.source = source;
            this.name = name;
            this.adress = adress;
        }

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Adress
        {
            get { return adress; }
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        public new void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}