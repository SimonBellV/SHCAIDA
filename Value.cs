using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class Value : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private double date;
        private TypeOfDataSources dataSourceType;
        private int clientID;
        private int deviceID;
        private double deviceValue;

        public Value(DateTime date, TypeOfDataSources dataSource, int clientID, int deviceID, double deviceValue)
        {
            this.date = date.ToOADate();
            this.dataSourceType = dataSource;
            this.clientID = clientID;
            this.deviceID = deviceID;
            this.deviceValue = deviceValue;
        }

        public double Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public TypeOfDataSources DataSourceType
        {
            get { return dataSourceType; }
            set
            {
                dataSourceType = value;
                OnPropertyChanged("DataSourceType");
            }
        }

        public int ClientID
        {
            get { return clientID; }
            set
            {
                clientID = value;
                OnPropertyChanged("ClientID");
            }
        }

        public int DeviceID
        {
            get { return deviceID; }
            set
            {
                deviceID = value;
                OnPropertyChanged("DeviceID");
            }
        }

        public double DeviceValue
        {
            get { return deviceValue; }
            set
            {
                deviceValue = value;
                OnPropertyChanged("DeviceValue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}