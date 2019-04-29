using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class Value : INotifyPropertyChanged
    {
        private DateTime date;
        private double sensorValue;
        private int sensorID;
        public int ID { get; set; }
        public Value(DateTime date, double value, int sensorID)
        {
            this.sensorID = sensorID;
            this.date = date;
            sensorValue = value;
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
        public double SensorValue
        {
            get { return sensorValue; }
            set
            {
                sensorValue = value;
                OnPropertyChanged("Value");
            }
        }
        public int SensorID
        {
            get { return sensorID; }
            set
            {
                sensorID = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}