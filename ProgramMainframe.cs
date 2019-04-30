﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
        MSSQL,
        Rockwell,
        Common
    }

    public struct SensorsConnections<T1, T2>
    {
        public T1 sensor;
        public T2 client;

        public SensorsConnections(T1 sensor, T2 client)
        {
            this.sensor = sensor;
            this.client = client;
        }
    }

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

    static class ProgramMainframe
    {
        public static List<Rule> rules;
        public static List<LingVariable> linguisticVariables;
        public static SiemensSensorsApplicationContext siemensSensors;
        public static SiemensValuesApplicationContext valuesdb;
        public static CommonStatusContext statusdb;
        public static MessageJournalContext journaldb;
        //public static List<RockwellClient> rockwellClients = new List<RockwellClient>();
        public static SiemensClientApplicationContext siemensClients;
        public static Accord.Fuzzy.Database fuzzyDB;
        public static Accord.Fuzzy.InferenceSystem IS;
        public static int rulesCount;
        public static List<SensorsConnections<SiemensSensor, SiemensClient>> ssconnections;
        public static List<SensorsConnections<MSSQLSensor, MSSQLClient>> mssqlconnections;
        private static bool iSRunning;
        public static long ISTimeout;
        public static MSSQLClientApplicationContext mssqlClients;
        public static MSSQLSensorApplicationContext mssqlSensors;

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

        public static void InitMainframe()
        {
            siemensSensors = new SiemensSensorsApplicationContext();
            valuesdb = new SiemensValuesApplicationContext();
            linguisticVariables = new List<LingVariable>();
            siemensClients = new SiemensClientApplicationContext();
            journaldb = new MessageJournalContext();
            statusdb = new CommonStatusContext();
            mssqlClients = new MSSQLClientApplicationContext();
            mssqlSensors = new MSSQLSensorApplicationContext();
            rules = new List<Rule>();
            ReadFuzzyDB();
            ReadRules();
            UpdateFuzzyDBRuleDB();
            ssconnections = new List<SensorsConnections<SiemensSensor, SiemensClient>>();
            mssqlconnections = new List<SensorsConnections<MSSQLSensor, MSSQLClient>>();
            foreach (var sensor in siemensSensors.SiemensSensors)
                foreach (var client in siemensClients.SiemensClients)
                    if (sensor.Source == client.Name)
                    {
                        ssconnections.Add(new SensorsConnections<SiemensSensor, SiemensClient>(sensor, client));
                        break;
                    }
            foreach (var sensor in mssqlSensors.MSSQLSensors)
                foreach (var client in mssqlClients.MSSQLClients)
                    if (sensor.ClientID == client.ID)
                    {
                        mssqlconnections.Add(new SensorsConnections<MSSQLSensor, MSSQLClient>(sensor, client));
                        break;
                    }
            ISRunning = false;
        }

        /// <summary>
        ///Апдейт нечеткой базы данных и IS
        /// </summary>
        public static void UpdateFuzzyDBRuleDB()
        {
            fuzzyDB = new Accord.Fuzzy.Database();
            foreach (var val in linguisticVariables)
            {
                fuzzyDB.AddVariable(val.ConvertToLinguisticVariable);
                //MessageBox.Show(val.name);
            }
            IS = new Accord.Fuzzy.InferenceSystem(fuzzyDB, new Accord.Fuzzy.CentroidDefuzzifier(1000));
            foreach (var val in rules)
                IS.NewRule(val.Name, val.RuleStr);
            WriteFuzzyDB();
        }

        public static void AddSiemensPLCSource(string cpuType, string ip, short rack, short slot, string name)
        {
            siemensClients.SiemensClients.Add(new SiemensClient(cpuType, ip, rack, slot, name));
            siemensClients.SaveChanges();
        }

        public static void UpdateStatuses(List<string> vals, TypeOfDataSources type)
        {
            foreach (var val in statusdb.CommonStatuses)
                statusdb.CommonStatuses.Remove(val);
            foreach (var val in vals)
                statusdb.CommonStatuses.Add(new CommonStatus(val, type));
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
                linguisticVariables.Add(new LingVariable(sourceType, source, name, start, end));
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

        /// <summary>
        /// Записать в txt все значения листа <see cref="linguisticVariables"/>, являющегося отображением <see cref="fuzzyDB"/>
        /// </summary>
        public static void WriteFuzzyDB()
        {
            string output = JsonConvert.SerializeObject(linguisticVariables);
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
                        sw.WriteLine(label.isLogging);
                        sw.WriteLine(label.name);
                        sw.WriteLine(label.V1);
                        sw.WriteLine(label.V2);
                        sw.WriteLine(label.V3);
                        sw.WriteLine(label.V4);
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter("fuzzyDBexperimental.txt"))
            {
                sw.WriteLine(output);
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
                                isLogging = Convert.ToBoolean(sr.ReadLine()),
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
            if (pos == -1 || pos != -1 && linguisticVariables.Find(x => x.name == varName).start <= label.GetGetFuzzy().LeftLimit && linguisticVariables.Find(x => x.name == varName).end >= label.GetGetFuzzy().RightLimit)
                linguisticVariables.Find(x => x.name == varName).labels.Add(label);
            else
                MessageBox.Show("Произошла ошибка при добавлении статуса переменной");
        }

        public static void FuzzySystemOnlineAsync()
        {
            var timer = new System.Threading.Timer(async (e) =>
            {
                await Task.Run(() => LoadIS());
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(ISTimeout));
        }

        public static void LoadIS()
        {
            foreach (var variable in linguisticVariables)
            {
                if (variable.sourceType == "Siemens")
                {
                    var var = ssconnections.Find(x => x.sensor.Name == variable.name);
                    IS.SetInput(variable.name, var.client.ReadData(var.sensor.Adress));
                }
                else if (variable.sourceType == "Rockwell")
                {
                    MessageBox.Show("Not ready");
                }
                else if (variable.sourceType == "SQL Server")
                {
                    var var = mssqlconnections.Find(x => x.sensor.Name == variable.name);
                    IS.SetInput(variable.name, var.client.ReadData(var.sensor));
                }
            }
            foreach (var variable in linguisticVariables)
            {
                var list = IS.ExecuteInference(variable.name).OutputList;
                foreach (var outv in list)
                    if (linguisticVariables.Find(x => x.name == variable.name).labels.Find(y => y.name == outv.Label).isLogging)
                    {
                        journaldb.MessageJournals.Add(new MessageJournal(variable.name, outv.Label, DateTime.Now));
                        journaldb.SaveChanges();
                    }
            }
        }

        public static int GetMSSQLClientID(string dataSource)
        {
            foreach (var client in mssqlClients.MSSQLClients)
                if (client.DataSource == dataSource)
                    return client.ID;
            return -1;
        }
    }
}