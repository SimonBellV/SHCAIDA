﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;

namespace SHCAIDA
{
    /// <summary>
    /// Ядро системы
    /// </summary>
    public enum TypeOfDataSources
    {
        Siemens = 0,
        Mssql,
        Rockwell,
        Common
    }

    [Serializable]
    public struct SensorsConnections<T1, T2>
    {
        public T1 Sensor;
        public T2 Client;

        public SensorsConnections(T1 sensor, T2 client)
        {
            this.Sensor = sensor;
            this.Client = client;
        }
    }

    [Serializable]
    public struct Rule
    {
        public string RuleStr { get; set; }
        public string Name { get; set; }
        public Rule(string rule)
        {
            RuleStr = rule;
            ProgramMainframe.RulesCount++;
            Name = "Rule " + ProgramMainframe.RulesCount;
        }

        public Rule(string rule, int num)
        {
            RuleStr = rule;
            Name = "Rule " + num;
        }
    }

    public class ValuesApplicationContext : DbContext
    {
        public ValuesApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<ValuesApplicationContext>(null);
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

    public class MessageJournalContext : DbContext
    {
        public MessageJournalContext() : base("DefaultConnection")
        {
            Database.SetInitializer<MessageJournalContext>(null);
        }
        public DbSet<MessageJournal> MessageJournals { get; set; }
    }

    public class MSSQLClientApplicationContext : DbContext
    {
        public MSSQLClientApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<MSSQLClientApplicationContext>(null);
        }
        public DbSet<MSSQLClient> MSSQLClients { get; set; }
    }

    public class MSSQLSensorApplicationContext : DbContext
    {
        public MSSQLSensorApplicationContext() : base("DefaultConnection")
        {
            Database.SetInitializer<MSSQLSensorApplicationContext>(null);
        }
        public DbSet<MSSQLSensor> MSSQLSensors { get; set; }
    }

    internal static class ProgramMainframe
    {
        public static List<Rule> Rules;
        public static List<LingVariable> LinguisticVariables;
        public static SiemensSensorsApplicationContext SiemensSensors;
        public static ValuesApplicationContext Valuesdb;
        public static CommonStatusContext Statusdb;
        public static MessageJournalContext Journaldb;
        public static SiemensClientApplicationContext SiemensClients;
        public static Accord.Fuzzy.Database FuzzyDb;
        public static Accord.Fuzzy.InferenceSystem IS;
        public static int RulesCount;
        public static List<SensorsConnections<SiemensSensor, SiemensClient>> Ssconnections;
        public static List<SensorsConnections<MSSQLSensor, MSSQLClient>> Mssqlconnections;
        private static bool iSRunning;
        public static long ISTimeout;
        public static MSSQLClientApplicationContext MssqlClients;
        public static MSSQLSensorApplicationContext MssqlSensors;
        public static List<GameNode> gameTheoryController;
        private static bool storagingRunning;
        public static long StoragingTimeout;

        public static bool ISRunning
        {
            get => iSRunning;
            set
            {
                iSRunning = value;
                if (iSRunning)
                {
                    FuzzySystemOnlineAsync();
                }
            }
        }

        public static bool StoragingRunning
        {
            get => storagingRunning;
            set
            {
                storagingRunning = value;
                if (storagingRunning)
                {
                    StartValueExtraction();
                }
            }
        }

        private static void StartValueExtraction()
        {
            var timer = new System.Threading.Timer(async (e) =>
            {
                await Task.Run(StoreDataOnline);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(StoragingTimeout));
        }

        public static void StoreDataOnline()
        {
            foreach (var device in SiemensSensors.SiemensSensors)
                if (device.isStoragingEnable)
                {
                    var client = Ssconnections.Find(x => x.Sensor == device).Client;
                    Valuesdb.Values.Add(new Value(
                        DateTime.Now,
                        TypeOfDataSources.Siemens,
                        client.ID,
                        device.ID,
                        client.ReadData(device.Name)));
                }
            foreach (var device in MssqlSensors.MSSQLSensors)
                if (device.isStoragingEnable)
                    Valuesdb.Values.Add(new Value(
                        DateTime.Now,
                        TypeOfDataSources.Siemens,
                        Mssqlconnections.Find(x => x.Sensor == device).Client.ID,
                        device.ID,
                        Mssqlconnections.Find(x => x.Sensor == device).Client.ReadData(device)));
            Valuesdb.SaveChangesAsync();
        }

        public static void InitMainframe()
        {
            SiemensSensors = new SiemensSensorsApplicationContext();
            Valuesdb = new ValuesApplicationContext();
            LinguisticVariables = new List<LingVariable>();
            SiemensClients = new SiemensClientApplicationContext();
            Journaldb = new MessageJournalContext();
            Statusdb = new CommonStatusContext();
            MssqlClients = new MSSQLClientApplicationContext();
            MssqlSensors = new MSSQLSensorApplicationContext();
            Rules = new List<Rule>();
            ReadFuzzyDB();
            ReadRules();
            UpdateFuzzyDBRuleDB();
            Ssconnections = new List<SensorsConnections<SiemensSensor, SiemensClient>>();
            Mssqlconnections = new List<SensorsConnections<MSSQLSensor, MSSQLClient>>();
            foreach (var sensor in SiemensSensors.SiemensSensors)
                foreach (var client in SiemensClients.SiemensClients)
                    if (sensor.Source == client.Name)
                    {
                        Ssconnections.Add(new SensorsConnections<SiemensSensor, SiemensClient>(sensor, client));
                        break;
                    }
            foreach (var sensor in MssqlSensors.MSSQLSensors)
                foreach (var client in MssqlClients.MSSQLClients)
                    if (sensor.ClientID == client.ID)
                    {
                        Mssqlconnections.Add(new SensorsConnections<MSSQLSensor, MSSQLClient>(sensor, client));
                        break;
                    }
            ISRunning = false;
            storagingRunning = false;
            ReadGameNodes();
        }

        /// <summary>
        ///Апдейт нечеткой базы данных и IS
        /// </summary>
        public static void UpdateFuzzyDBRuleDB()
        {
            FuzzyDb = new Accord.Fuzzy.Database();
            foreach (var val in LinguisticVariables)
            {
                FuzzyDb.AddVariable(val.ConvertToLinguisticVariable);
                //MessageBox.Show(val.name);
            }
            IS = new Accord.Fuzzy.InferenceSystem(FuzzyDb, new Accord.Fuzzy.CentroidDefuzzifier(1000));
            foreach (var val in Rules)
                IS.NewRule(val.Name, val.RuleStr);
            WriteFuzzyDB();
        }

        public static void AddSiemensPlcSource(string cpuType, string ip, short rack, short slot, string name)
        {
            SiemensClients.SiemensClients.Add(new SiemensClient(cpuType, ip, rack, slot, name));
            SiemensClients.SaveChanges();
        }

        public static void UpdateStatuses(List<string> vals, TypeOfDataSources type)
        {
            foreach (var val in Statusdb.CommonStatuses)
                Statusdb.CommonStatuses.Remove(val);
            foreach (var val in vals)
                Statusdb.CommonStatuses.Add(new CommonStatus(val, type));
            Statusdb.SaveChanges();
        }

        public static int FindLabelIndex(string sensor, string label)
        {
            var pos1 = -1;
            var pos2 = -1;
            pos1 = LinguisticVariables.FindIndex(x => x.name == sensor);
            if (pos1 != -1)
                pos2 = LinguisticVariables[pos1].labels.FindIndex(x => x.name == label);
            return pos2;
        }

        public static void AddLingVariable(string sourceType, string source, string name, float start, float end)
        {
            if (LinguisticVariables.FindIndex(x => x.name == name) == -1)
                LinguisticVariables.Add(new LingVariable(sourceType, source, name, start, end));
        }

        public static List<string> GetSensorLabels(string sensor)
        {
            List<string> result = new List<string>();
            foreach (var val in LinguisticVariables)
                if (val.name == sensor)
                    foreach (var label in val.labels)
                        result.Add(label.name);
            return result;
        }

        public static void RemoveSensorLabel(string sensor, string label)
        {
            foreach (var val in LinguisticVariables)
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

        public static void RemoveLingVariable(string name) => LinguisticVariables.Remove(LinguisticVariables.Find(x => x.name == name));

        /// <summary>
        /// Записать в txt все значения листа <see cref="LinguisticVariables"/>, являющегося отображением <see cref="FuzzyDb"/>
        /// </summary>
        public static void WriteFuzzyDB()
        {
            using (var sw = new StreamWriter("fuzzyDB.txt"))
            {
                sw.WriteLine(LinguisticVariables.Count);
                foreach (var val in LinguisticVariables)
                {
                    sw.WriteLine(val.sourceType);
                    sw.WriteLine(val.source);
                    sw.WriteLine(val.name);
                    sw.WriteLine(val.start);
                    sw.WriteLine(val.end);
                    sw.WriteLine(val.labels.Count);
                    foreach (var label in val.labels)
                    {
                        sw.WriteLine(label.isLogging);
                        sw.WriteLine(label.name);
                        sw.WriteLine(label.V1);
                        sw.WriteLine(label.V2);
                        sw.WriteLine(label.V3);
                        sw.WriteLine(label.V4);
                    }
                }
            }
        }//рассмотреть целесообразность записи в json

        private static void ReadFuzzyDB()
        {
            LinguisticVariables = new List<LingVariable>();
            if (!File.Exists("fuzzyDB.txt")) return;
            using (var sr = new StreamReader("fuzzyDB.txt"))
            {
                var totalCount = Convert.ToInt32(sr.ReadLine());
                for (var i = 0; i < totalCount; i++)
                {
                    var t = new LingVariable(sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), Convert.ToSingle(sr.ReadLine()), Convert.ToSingle(sr.ReadLine()));
                    var count = Convert.ToInt32(sr.ReadLine());
                    for (var j = 0; j < count; j++)
                    {
                        var t1 = new Status
                        {
                            isLogging = Convert.ToBoolean(sr.ReadLine()),
                            name = sr.ReadLine(),
                            V1 = Convert.ToInt32(sr.ReadLine()),
                            V2 = Convert.ToInt32(sr.ReadLine()),
                            V3 = Convert.ToInt32(sr.ReadLine()),
                            V4 = Convert.ToInt32(sr.ReadLine())
                        };
                        t.labels.Add(t1);
                    }
                    LinguisticVariables.Add(t);
                }
            }
        }

        public static void WriteRules()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var sw = new FileStream("rules.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(sw, Rules);
            }
        }

        private static void ReadRules()
        {
            Rules = new List<Rule>();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("rules.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                    Rules = (List<Rule>)formatter.Deserialize(fs);
            }
            RulesCount = Rules.Count + 1;
        }

        public static void AddLabel(string varName, Status label)
        {
            var pos = FindLabelIndex(varName, label.name);
            if (pos == -1 || pos != -1 && LinguisticVariables.Find(x => x.name == varName).start <= label.GetFuzzy().LeftLimit && LinguisticVariables.Find(x => x.name == varName).end >= label.GetFuzzy().RightLimit)
                LinguisticVariables.Find(x => x.name == varName).labels.Add(label);
            else
                MessageBox.Show("Произошла ошибка при добавлении статуса переменной");
        }

        public static void FuzzySystemOnlineAsync()
        {
            var timer = new System.Threading.Timer(async (e) =>
            {
                await Task.Run(LoadIS);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(ISTimeout));
        }

        public static void LoadIS()
        {
            foreach (var variable in LinguisticVariables)
            {
                switch (variable.sourceType)
                {
                    case "Siemens":
                        {
                            var var = Ssconnections.Find(x => x.Sensor.Name == variable.name);
                            if (var.Sensor != null && var.Client != null)
                                IS.SetInput(variable.name, var.Client.ReadData(var.Sensor.Adress));
                            break;
                        }
                    case "Rockwell":
                        MessageBox.Show("Not ready");
                        break;
                    case "SQL Server":
                        {
                            var var = Mssqlconnections.Find(x => x.Sensor.Name == variable.name);
                            IS.SetInput(variable.name, var.Client.ReadData(var.Sensor));
                            break;
                        }
                }
            }
            foreach (var variable in LinguisticVariables)
            {
                var list = IS.ExecuteInference(variable.name).OutputList;
                foreach (var outv in list)
                    if (LinguisticVariables.Find(x => x.name == variable.name).labels.Find(y => y.name == outv.Label).isLogging)
                    {
                        var dev = FindDevice(variable.name);
                        Journaldb.MessageJournals.Add(new MessageJournal(dev.Item1, dev.Item2, outv.Label, DateTime.Now, dev.Item3));
                        Journaldb.SaveChanges();
                    }
            }
        }

        public static Tuple<int, int, TypeOfDataSources> FindDevice(string name)
        {
            foreach (var device in SiemensSensors.SiemensSensors)
                if (device.Name == name)
                {
                    int clientID = -1;
                    foreach (var client in SiemensClients.SiemensClients)
                        if (client.Name == device.SourceName)
                        {
                            clientID = client.ID;
                            break;
                        }
                    return new Tuple<int, int, TypeOfDataSources>(device.ID, clientID, TypeOfDataSources.Siemens);
                }
            foreach (var device in MssqlSensors.MSSQLSensors)
                if (device.Name == name)
                    return new Tuple<int, int, TypeOfDataSources>(device.ID, device.ClientID, TypeOfDataSources.Mssql);
            throw new ArgumentOutOfRangeException();
        }

        private static void ReadGameNodes()
        {
            gameTheoryController = new List<GameNode>();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("gamenodes.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                    gameTheoryController = (List<GameNode>)formatter.Deserialize(fs);
            }
        }

        public static void WriteGameNodes()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var sw = new FileStream("gamenodes.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(sw, gameTheoryController);
            }
        }

        public static int GetSensorIDByName(string name, TypeOfDataSources type)
        {
            switch (type)
            {
                case TypeOfDataSources.Siemens:
                    {
                        foreach (var sensor in SiemensSensors.SiemensSensors)
                            if (name == sensor.Name)
                                return sensor.ID;
                    }
                    break;
                case TypeOfDataSources.Mssql:
                    {
                        foreach (var sensor in MssqlSensors.MSSQLSensors)
                            if (name == sensor.Name)
                                return sensor.ID;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
            throw new Exception("Этот датчик не найден");
        }

        public static string GetSensorNameById(int ID, TypeOfDataSources type)
        {
            switch (type)
            {
                case TypeOfDataSources.Siemens:
                    {
                        foreach (var sensor in SiemensSensors.SiemensSensors)
                            if (ID == sensor.ID)
                                return sensor.Name;
                    }
                    break;
                case TypeOfDataSources.Mssql:
                    {
                        foreach (var sensor in MssqlSensors.MSSQLSensors)
                            if (ID == sensor.ID)
                                return sensor.Name;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
            throw new Exception("Этот датчик не найден");
        }
    }
}