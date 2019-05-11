using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    [Serializable]
    public class MSSQLSensor : Sensor
    {
        private int clientID;

        public MSSQLSensor()
        {
            ID = -1;
            clientID = -1;
            base.Name = "";
            SourceName = "";
            SourcePath = "";
        }

        public MSSQLSensor(int clientID, string databaseName, string headerName, string name)
        {
            ID = -1;
            this.clientID = clientID;
            SourceName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            SourcePath = headerName ?? throw new ArgumentNullException(nameof(headerName));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            isStoragingEnable = false;
        }

        public bool IsStoragingEnable
        {
            get { return isStoragingEnable; }
            set
            {
                isStoragingEnable = value;
                OnPropertyChanged("IsStoragingEnable");
            }
        }

        public string DatabaseName
        {
            get => SourceName;
            set
            {
                SourceName = value;
                OnPropertyChanged("DatabaseName");
            }
        }

        public string HeaderName
        {
            get => SourcePath;
            set
            {
                SourcePath = value;
                OnPropertyChanged("HeaderName");
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
