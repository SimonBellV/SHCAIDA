using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SHCAIDA
{
    public class Message
    {
        public string sensor { get; set; }
        public string state { get; set; }
        public string time { get; set; }

        public Message(string sensor, string state, string time)
        {
            this.sensor = sensor;
            this.state = state;
            this.time = time;
        }
    }
    public class MessageJournal : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string sensor;
        private string state;
        private double time;

        public MessageJournal(string sensor, string state, DateTime time)
        {
            this.sensor = sensor;
            this.state = state;
            this.time = time.ToOADate();
        }

        public MessageJournal()
        {
            ID = -1;
            sensor = "";
            state = "";
        }

        public Message ConvertToMessage()
        {
            return new Message(sensor, state, DateTime.FromOADate(time).ToString());
;       }

        public string Sensor
        {
            get { return sensor; }
            set
            {
                sensor = value;
                OnPropertyChanged("Sensor");
            }
        }

        public double Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
