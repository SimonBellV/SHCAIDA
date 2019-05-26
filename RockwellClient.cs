using Corsinvest.AllenBradley.PLC.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SHCAIDA
{
    class RockwellClient :INotifyPropertyChanged
    {
        public int ID { get; set; }
        private Controller plc;
        private bool isPlcConnected;
        public string name;

        public void ConnectToPlc(string name, string ip, string path, string cpuType)
        {
            try
            {
                plc = new Controller(ip, path, ParseCpuType(cpuType));
                isPlcConnected = true;
                this.name = name;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private CPUType ParseCpuType(string cpuType)
        {
            switch (cpuType)
            {
                case "LGX":
                    return CPUType.LGX;
                case "PLC5":
                    return CPUType.PLC5;
                case "SLC":
                    return CPUType.SLC;
                default:
                    throw new Exception("Неизвестный тип PLC Rockwell");
            }
        }

        public float ReadData(string variable)
        {
            return 0;
        }

        private void WriteData(List<string> variables, List<double> values)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
