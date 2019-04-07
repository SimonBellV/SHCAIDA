using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class MSSQLSensor : Sensor
    {
        private int clientID;
        private string databaseName;
        private string headerName;

        public MSSQLSensor()
        {
            ID = -1;
            clientID = -1;
            name = "";
            databaseName = "";
            headerName = "";
        }

        public MSSQLSensor(int clientID, string databaseName, string headerName, string name)
        {
            ID = -1;
            this.clientID = clientID;
            this.databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.headerName = headerName ?? throw new ArgumentNullException(nameof(headerName));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string DatabaseName
        {
            get => databaseName;
            set
            {
                databaseName = value;
                OnPropertyChanged("DatabaseName");
            }
        }

        public string HeaderName
        {
            get => headerName;
            set
            {
                headerName = value;
                OnPropertyChanged("HeaderName");
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

        public int ClientID
        {
            get => clientID;
            set
            {
                clientID = value;
                OnPropertyChanged("ClientID");
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
