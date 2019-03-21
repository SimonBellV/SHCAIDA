using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHCAIDA
{
    public class SiemensClient : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private readonly Plc plc;
        private bool isPlcConnected;
        private string ip;
        private readonly CpuType cpuTypeCT;
        private string cpuType;
        private short rack;
        private short slot;
        private string name;

        public SiemensClient()
        {
            ID = 0;
            isPlcConnected = false;
            ip = "";
            name = "";
            rack = 0;
            slot = 0;
        }

        public SiemensClient(string cpuType, string ip, short rack, short slot, string name)
        {
            this.ip = ip;
            this.cpuTypeCT = ParseCpuType(cpuType);
            this.cpuType = cpuType;
            this.rack = rack;
            this.slot = slot;
            this.name = name;
            plc = new Plc(this.cpuTypeCT, ip, rack, slot);
        }

        public string Name
        {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public short Rack
        {
            get { return rack; }
            set {
                rack = value;
                OnPropertyChanged("Rack");
            }
        }

        public short Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                OnPropertyChanged("Slot");
            }
        }

        public string CpuTypeS
        {
            get { return cpuType; }
            set
            {
                cpuType = value;
                OnPropertyChanged("CpuType");
            }
        }

        public string IP
        {
            get { return ip; }
            set {
                ip = value;
                OnPropertyChanged("IP");
            }
        }

        public void ConnectToPlc()
        {            
            try
            {
                plc.Open();
                isPlcConnected = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        public bool CheckConnection()
        {
            try
            {
                plc.Open();
            }
            catch
            {
                return false;
            }
            plc.Close();
            return true;
        }

        private CpuType ParseCpuType(string cpu)
        {
            switch(cpu)
            { 
                case "S7200":
                    return CpuType.S7200;
                case "S7300":
                    return CpuType.S7300;
                case "S7400":
                    return CpuType.S7400;
                case "S71200":
                    return CpuType.S71200;
                case "S71500":
                    return CpuType.S71500;
                default:
                    throw new Exception("Неизвестный вид CPU Siemens S7");
            }

        }

        public void DisconnectToPlc()
        {
            if (isPlcConnected)
            {
                plc.Close();
                isPlcConnected = false;
            }
        }

        public List<double> ReadData(List<string> variables)
        {
            List<double> data = new List<double>();

            foreach (string var in variables)
                data.Add((double)plc.Read(var));

            return data;
        }

        public void WriteData(List<string> variables, List<double> values)
        {
            if (variables.Count != values.Count)
                throw new Exception("Ошибка при записи данных");
            try
            {
                for (var i = 0; i < variables.Count; i++)
                {
                    plc.Write(variables[i], values[i]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}