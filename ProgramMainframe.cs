using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

    public class SiemensSensor : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string source;
        private string name;
        private string adress;
        public bool ruleType;

        public SiemensSensor()
        {
            ID = 0;
            source = "";
            name = "";
            adress = "";
        }
        public SiemensSensor(string source, string name, string adress)
        {
            this.source = source;
            this.name = name;
            this.adress = adress;
        }

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Adress
        {
            get { return adress; }
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class CommonStatus
    {
        public int ID { get; set; }
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public CommonStatus(string name)
        {
            ID = -1;
            this.name = name;
        }

        public CommonStatus()
        {
            ID = -1;
            name = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class SiemensValuesApplicationContext : DbContext
    {
        public SiemensValuesApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<SiemensValuesApplicationContext>(null);
        }
        public DbSet<Value> Values { get; set; }
    }

    public class SiemensSensorsApplicationContext : DbContext
    {
        public SiemensSensorsApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<SiemensSensorsApplicationContext>(null);
        }
        public DbSet<SiemensSensor> SiemensSensors { get; set; }
    }

    public class SiemensClientApplicationContext : DbContext
    {
        public SiemensClientApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<SiemensClientApplicationContext>(null);
        }
        public DbSet<SiemensClient> SiemensClients { get; set; }
    }

    public class CommonStatusContext : DbContext
    {
        public CommonStatusContext() : base("DefaultConnection")
        {
            Database.SetInitializer<CommonStatusContext>(null);
        }
        public DbSet<CommonStatus> CommonStatuses { get; set; }
    }


    static class ProgramMainframe
    {
        public static SiemensSensorsApplicationContext siemensSensors = new SiemensSensorsApplicationContext();
        public static SiemensValuesApplicationContext valuesdb = new SiemensValuesApplicationContext();
        public static CommonStatusContext statusdb = new CommonStatusContext();
        //public static List<RockwellClient> rockwellClients = new List<RockwellClient>();
        public static SiemensClientApplicationContext siemensClients = new SiemensClientApplicationContext();
        public static Accord.Fuzzy.Database fuzzyDB = new Accord.Fuzzy.Database();
        public static Accord.Fuzzy.InferenceSystem IS = new Accord.Fuzzy.InferenceSystem(fuzzyDB, new Accord.Fuzzy.CentroidDefuzzifier(1000));


        public static bool IsSiemensClientAddedByIP(string IP)
        {
            foreach (SiemensClient client in siemensClients.SiemensClients)
                if (client.IP == IP)
                    return true;
            return false;
        }

        public static void AddSiemensPLCSource(string cpuType, string ip, short rack, short slot, string name)
        {
            siemensClients.SiemensClients.Add(new SiemensClient(cpuType, ip, rack, slot, name));
            siemensClients.SaveChanges();
        }

        public static void AddValueToSQLite(Value val)
        {
            valuesdb.Values.Add(val);
            valuesdb.SaveChanges();
        }

        public static void AddSensorToSQLite(SiemensSensor sensor)
        {
            siemensSensors.SiemensSensors.Add(sensor);
            siemensSensors.SaveChanges();
        }

        public static void AddValueToSQLite(List<Value> vals)
        {
            foreach (Value val in vals)
                valuesdb.Values.Add(val);
            valuesdb.SaveChanges();
        }

        public static List<Value> GetValuesFromSQLite(int sensorID)
        {
            List<Value> result = new List<Value>();
            result = valuesdb.Values.Where(x => x.SensorID == sensorID).ToList();
            return result;
        } /*all data by sensor name*/

        public static List<Value> GetValuesFromSQLite(int sensorID, DateTime start) /*from start date to now and by sensor name*/
        {
            List<Value> result = new List<Value>();
            result = valuesdb.Values.Where(x => x.SensorID == sensorID && x.Date >= start).ToList();
            return result;
        }

        public static List<Value> GetValuesFromSQLite(int sensorID, DateTime start, DateTime end) /*from start date to end date and by sensor name*/
        {
            List<Value> result = new List<Value>();
            result = valuesdb.Values.Where(x => x.SensorID == sensorID && x.Date >= start && x.Date <= end).ToList();
            return result;
        }

        public static void UpdateStatuses(List<string> vals)
        {
            foreach (var val in statusdb.CommonStatuses)
                statusdb.CommonStatuses.Remove(val);
            foreach (var val in vals)
                statusdb.CommonStatuses.Add(new CommonStatus(val));
            statusdb.SaveChanges();
        }
    }
}