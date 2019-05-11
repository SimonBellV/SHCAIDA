using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    [Serializable]
    public class BasicValue : INotifyPropertyChanged
    {
        protected DateTime date;
        protected double deviceValue;
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public BasicValue(double value, DateTime date)
        {
            deviceValue = value;
            this.date = date;
        }
    }
    public class Value : BasicValue
    {
        public int ID { get; set; }        
        private TypeOfDataSources dataSourceType;
        private int clientID;
        private int deviceID;        

        public Value(DateTime date, TypeOfDataSources dataSource, int clientID, int deviceID, double deviceValue) : base(deviceValue, date)
        {
            this.dataSourceType = dataSource;
            this.clientID = clientID;
            this.deviceID = deviceID;
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

        

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}