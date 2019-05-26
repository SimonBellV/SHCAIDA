using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public struct Message
    {
        public string Sensor { get; set; }
        public string State { get; set; }
        public string Time { get; set; }

        public Message(string sensor, string state, string time)
        {
            this.Sensor = sensor;
            this.State = state;
            this.Time = time;
        }
    }
    public class MessageJournal : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private int sensorID;
        private int clientID;
        private string state;
        private TypeOfDataSources type;
        private double time;

        public MessageJournal(int sensorID, int clientID, string state, DateTime time, TypeOfDataSources type)
        {
            this.sensorID = sensorID;
            this.state = state;
            this.time = time.ToOADate();
            this.clientID = clientID;
            this.type = type;
        }

        public MessageJournal()
        {
            ID = -1;
            sensorID = -1;
            clientID = -1;
            type = 0;
            state = "";
        }

        public Message ConvertToMessage()
        {
            return new Message(ProgramMainframe.GetSensorNameById(sensorID, type), state, DateTime.FromOADate(time).ToString());
        }

        public int SensorID
        {
            get { return sensorID; }
            set
            {
                sensorID = value;
                OnPropertyChanged("SensorID");
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

        public TypeOfDataSources Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
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
