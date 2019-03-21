using System;
using System.Collections.Generic;
using System.Windows;
using Accord.Fuzzy;

namespace SHCAIDA
{
    public class Status
    {
        public string name;
        public float V1;
        public float V2;
        public float V3;
        public float V4;
        public string sensor;

        public string String => name + " - " + V1 + " ," + V2 + " ," + V3 + " ," + V4;

        public FuzzySet GetFuzzy => new FuzzySet(name, new TrapezoidalFunction(V1, V2, V3, V4));

        public Status(string name, string V1, string V2, string V3, string V4, string sensor)
        {
            this.name = name;
            this.sensor = sensor;
            try
            {
                this.V1 = Convert.ToSingle(V1);
                this.V2 = Convert.ToSingle(V2);
                this.V3 = Convert.ToSingle(V3);
                this.V4 = Convert.ToSingle(V4);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                this.V1 = 0;
                this.V2 = 0;
                this.V3 = 0;
                this.V4 = 0;
            }
        }
    }
    public partial class RuleVariablesSetup : Window
    {
        private readonly List<Status> someStr = new List<Status>();
        public RuleVariablesSetup()
        {
            InitializeComponent();
            foreach (var val in RulerCreator.fuzzySensors)
                SensorsCB.Items.Add(val.Name);
            foreach (var val in ProgramMainframe.statusdb.CommonStatuses)
                SensorsCB.Items.Add(val.Name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Status t = new Status(ValueNameTB.Text, V1TB.Text, V2TB.Text, V3TB.Text, V4TB.Text, SensorsCB.SelectedItem.ToString());
            int res = RulerCreator.FSContainsVar(t.sensor);
            if (res != -1)
            {
                RulerCreator.fuzzySensors[res].AddLabel(t.GetFuzzy);
            }
            else
            {
                //RulerCreator.fuzzySensors.Add(new LinguisticVariable())
            }
            RulerCreator.fuzzySets.Add();
           // RulerCreator.fuzzySetsSensorNames.Add(SensorCB.SelectedItem.ToString());
            someStr.Add(t);
            StatesLB.Items.Add(t.String);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int pos = someStr.FindIndex(x => x.String == StatesLB.SelectedItem.ToString());
            someStr.RemoveAt(pos);
            StatesLB.Items.RemoveAt(pos);
            RulerCreator.fuzzySets.RemoveAt(pos);
           // RulerCreator.fuzzySetsSensorNames.RemoveAt(pos);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int pos = someStr.FindIndex(x => x.String == StatesLB.SelectedItem.ToString());
            //SensorCB.SelectedItem = RulerCreator.fuzzySetsSensorNames[pos];
            ValueNameTB.Text = someStr[pos].name;
            V1TB.Text = someStr[pos].V1.ToString();
            V2TB.Text = someStr[pos].V2.ToString();
            V3TB.Text = someStr[pos].V3.ToString();
            V4TB.Text = someStr[pos].V4.ToString();
            someStr.RemoveAt(pos);
            StatesLB.Items.RemoveAt(pos);
            RulerCreator.fuzzySets.RemoveAt(pos);
           // RulerCreator.fuzzySetsSensorNames.RemoveAt(pos);            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<string> usedSensors = new List<string>();
            foreach (var status in someStr)
                if (!usedSensors.Contains(status.sensor))
                    usedSensors.Add(status.sensor);
            foreach (var sensor in usedSensors)
            {
                if (ProgramMainframe.fuzzyDB.GetVariable(sensor) != null)
                {                    
                    //RulerCreator.fuzzySensors.Find(x => x.Name == sensor).AddLabel;

                    //добавить лингвистические переменные в IS с состояниями
                }
                else
                {
                    MessageBox.Show("Переменная " + sensor + " уже описана в системе!");
                }
            }
            RuleWizard f = new RuleWizard();
            f.Show();
            this.Close();
        }
    }
}