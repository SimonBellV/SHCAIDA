﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SHCAIDA
{
    public class Sensor : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string name;
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
