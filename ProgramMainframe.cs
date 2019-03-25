using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SHCAIDA
{
    public struct Rule
    {
        public string RuleStr { get; set; }
        public string Name { get; set; }
        public Rule(string rule)
        {
            RuleStr = rule;
            ProgramMainframe.rulesCount++;
            Name = "Rule " + ProgramMainframe.rulesCount;
        }

        public Rule(string rule, int num)
        {
            RuleStr = rule;
            Name = "Rule " + num;
        }
    }

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
        public static List<Rule> rules = new List<Rule>();
        public static List<LingVariable> linguisticVariables = new List<LingVariable>();
        public static SiemensSensorsApplicationContext siemensSensors = new SiemensSensorsApplicationContext();
        public static SiemensValuesApplicationContext valuesdb = new SiemensValuesApplicationContext();
        public static CommonStatusContext statusdb = new CommonStatusContext();
        //public static List<RockwellClient> rockwellClients = new List<RockwellClient>();
        public static SiemensClientApplicationContext siemensClients = new SiemensClientApplicationContext();
        public static Accord.Fuzzy.Database fuzzyDB;
        public static Accord.Fuzzy.InferenceSystem IS;
        public static int rulesCount;

        public static void InitMainframe()
        {
            ReadFuzzyDB();
            ReadRules();
            UpdateFuzzyDBRuleDB();
        }

        public static void UpdateFuzzyDBRuleDB()
        {
            fuzzyDB = new Accord.Fuzzy.Database();
            foreach (var val in linguisticVariables)
            {
                fuzzyDB.AddVariable(val.ConvertToLinguisticVariable);
                MessageBox.Show(val.name);
            }
            IS = new Accord.Fuzzy.InferenceSystem(fuzzyDB, new Accord.Fuzzy.CentroidDefuzzifier(1000));
            foreach (var val in rules)
                IS.NewRule(val.Name, val.RuleStr);
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

        public static int FindLabelIndex(string sensor, string label)
        {
            int pos_1 = -1;
            int pos_2 = -1;
            pos_1 = linguisticVariables.FindIndex(x => x.name == sensor);
            if (pos_1 != -1)
                pos_2 = linguisticVariables[pos_1].labels.FindIndex(x => x.name == label);
            return pos_2;
        }

        public static void AddLingVariable(string sourceType, string source, string name, float start, float end)
        {
            if (linguisticVariables.FindIndex(x => x.name == name) == -1)
            {
                linguisticVariables.Add(new LingVariable(sourceType, source, name, start, end));
            }
        }

        public static List<string> GetSensorLabels(string sensor)
        {
            List<string> result = new List<string>();
            foreach (var val in linguisticVariables)
                if (val.name == sensor)
                    foreach (var label in val.labels)
                        result.Add(label.name);
            return result;
        }

        public static void RemoveSensorLabel(string sensor, string label)
        {
            foreach (var val in linguisticVariables)
                if (val.name == sensor)
                {
                    for (var i = 0; i < val.labels.Count; i++)
                        if (val.labels[i].name == label)
                        {
                            val.labels.RemoveAt(i);
                            break;
                        }
                }
        }

        public static void RemoveLingVariable(string name)
        {
            linguisticVariables.Remove(linguisticVariables.Find(x => x.name == name));
        }

        public static void WriteFuzzyDB()
        {
            using (StreamWriter sw = new StreamWriter("fuzzyDB.txt"))
            {
                sw.WriteLine(linguisticVariables.Count);
                foreach (var val in linguisticVariables)
                {
                    sw.WriteLine(val.sourceType);
                    sw.WriteLine(val.source);
                    sw.WriteLine(val.name);
                    sw.WriteLine(val.start);
                    sw.WriteLine(val.end);
                    sw.WriteLine(val.labels.Count);
                    foreach (var label in val.labels)
                    {
                        sw.WriteLine(label.name);
                        sw.WriteLine(label.V1);
                        sw.WriteLine(label.V2);
                        sw.WriteLine(label.V3);
                        sw.WriteLine(label.V4);
                    }
                }
            }
        }//рассмотреть целесообразность записи в json

        public static void ReadFuzzyDB()
        {
            linguisticVariables = new List<LingVariable>();
            if (File.Exists("fuzzyDB.txt"))
                using (StreamReader sr = new StreamReader("fuzzyDB.txt"))
                {
                    int totalCount = Convert.ToInt32(sr.ReadLine());
                    for (var i = 0; i < totalCount; i++)
                    {
                        LingVariable t = new LingVariable(sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), Convert.ToSingle(sr.ReadLine()), Convert.ToSingle(sr.ReadLine()));
                        int count = Convert.ToInt32(sr.ReadLine());
                        for (var j = 0; j < count; j++)
                        {
                            Status t1 = new Status
                            {
                                name = sr.ReadLine(),
                                V1 = Convert.ToInt32(sr.ReadLine()),
                                V2 = Convert.ToInt32(sr.ReadLine()),
                                V3 = Convert.ToInt32(sr.ReadLine()),
                                V4 = Convert.ToInt32(sr.ReadLine())
                            };
                            t.labels.Add(t1);
                        }
                        linguisticVariables.Add(t);
                    }
                }
        }

        public static void WriteRules()
        {
            using (StreamWriter sw = new StreamWriter("rules.txt"))
            {
                sw.WriteLine(rules.Count);
                foreach (var val in rules)
                    sw.WriteLine(val.RuleStr);
            }
        }

        public static void ReadRules()
        {
            rules = new List<Rule>();
            if (File.Exists("rules.txt"))
                using (StreamReader sr = new StreamReader("rules.txt"))
                {
                    rulesCount = Convert.ToInt32(sr.ReadLine());
                    for (int i = 0; i < rulesCount; i++)
                        rules.Add(new Rule(sr.ReadLine(), i + 1));
                }
        }

        public static void AddLabel(string varName, Status label)
        {
            int pos = FindLabelIndex(varName, label.name);
            if (pos == -1 || pos != -1 && linguisticVariables.Find(x => x.name == varName).start <= label.GetFuzzy.LeftLimit && linguisticVariables.Find(x => x.name == varName).end >= label.GetFuzzy.RightLimit)
                linguisticVariables.Find(x => x.name == varName).labels.Add(label);
            else
                MessageBox.Show("Произошла ошибка при добавлении статуса переменной");
        }

        public static async void FuzzySystemOnlineAsync()
        {
            await Task.Run(() => LoadIS());
        }

        public static void LoadIS()
        {
            foreach (var variable in linguisticVariables)
            {
                /* if(variable.sourceType=="Siemens")
                     IS.SetInput(variable.name, siemensClients.SiemensClients.*/
            }
        }
    }
}