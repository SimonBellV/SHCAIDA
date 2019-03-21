using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Corsinvest.AllenBradley.PLC.Api;

namespace SHCAIDA
{
    class RockwellClient
    {
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

        public List<double> ReadData(List<string> variables)
        {
            return new List<double>();
        }
    }
}
