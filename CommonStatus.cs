using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class CommonStatus
    {
        public int ID { get; set; }
        private string name;
        private string type;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public CommonStatus(string name, TypeOfDataSources type)
        {
            ID = -1;
            this.name = name;
            switch (type)
            {
                case TypeOfDataSources.Siemens:
                    this.type = "Siemens";
                    break;
                case TypeOfDataSources.Mssql:
                    this.type = "MSSQL";
                    break;
                case TypeOfDataSources.Rockwell:
                    this.type = "Rockwell";
                    break;
                case TypeOfDataSources.Common:
                    this.type = "Общее";
                    break;
                default:
                    this.type = "Неопределенно";
                    break;
            }
        }

        public CommonStatus()
        {
            ID = -1;
            name = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}