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

        private string GetDataSourceTypeString(TypeOfDataSources type)
        {
            switch (type)
            {
                case TypeOfDataSources.Siemens:
                    return "Siemens";
                case TypeOfDataSources.Mssql:
                    return "MSSQL";
                case TypeOfDataSources.Rockwell:
                    return "Rockwell";
                case TypeOfDataSources.Common:
                    return "Общее";
                default:
                    return "Неопределенно";
            }
        }

        public CommonStatus(string name, TypeOfDataSources type)
        {
            ID = -1;
            this.name = name;
            this.type = GetDataSourceTypeString(type);
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