using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{
    public partial class RuleVariablesSetup : Window
    {
        //private readonly List<Status> someStr = new List<Status>();
        public RuleVariablesSetup()
        {
            InitializeComponent();
            foreach (var val in ProgramMainframe.linguisticVariables)
            {
                SensorsCB.Items.Add(val.name);
                SensorsLB.Items.Add(val.name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//add
        {            
            Status t = new Status(ValueNameTB.Text, V1TB.Text, V2TB.Text, V3TB.Text, V4TB.Text);
            if(t.V1<t.V2 && t.V2<t.V3 && t.V3<t.V4 || t.V1 < t.V2 && (t.V3 == float.MinValue || t.V3 == float.MaxValue))
            ProgramMainframe.AddLabel(SensorsCB.SelectedItem.ToString(), t);
            if (!SensorsLB.Items.Contains(SensorsCB.SelectedItem.ToString()))
                SensorsLB.Items.Add(SensorsCB.SelectedItem.ToString());
            if (SensorsLB.SelectedItem != null && SensorsLB.SelectedItem.ToString() == SensorsCB.SelectedItem.ToString())
                StatesLB.Items.Add(t.String);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//delete
        {
            ProgramMainframe.RemoveSensorLabel(SensorsLB.SelectedItem.ToString(), StatesLB.SelectedItem.ToString());
            StatesLB.Items.Remove(StatesLB.SelectedItem.ToString());
            if (StatesLB.Items.Count == 0)
                SensorsLB.Items.Remove(SensorsLB.SelectedItem.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//edit
        {
            if (SensorsLB.SelectedItem != null && StatesLB.SelectedItem != null)
            {
                var variable = ProgramMainframe.linguisticVariables.Find(x => x.name == SensorsLB.SelectedItem.ToString());
                var label = variable.labels.Find(x => x.name == StatesLB.SelectedItem.ToString());
                SensorsCB.SelectedItem = SensorsLB.SelectedItem;
                ValueNameTB.Text = label.name;
                V1TB.Text = label.V1.ToString();
                V2TB.Text = label.V2.ToString();
                V3TB.Text = label.V3.ToString();
                V4TB.Text = label.V4.ToString();
                StatesLB.Items.Remove(StatesLB.SelectedItem);

                ProgramMainframe.linguisticVariables.Remove(variable);
                variable.labels.Remove(label);

                if (variable.labels.Count == 0)
                    SensorsCB.Items.Remove(SensorsLB.SelectedItem);

                ProgramMainframe.linguisticVariables.Add(variable);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RuleWizard f = new RuleWizard();
            f.Show();
            this.Close();
        }

        private void SensorLB_SensorChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            List<string> labels = ProgramMainframe.GetSensorLabels(SensorsLB.SelectedItem.ToString());
            StatesLB.Items.Clear();
            foreach (string state in labels)
                StatesLB.Items.Add(state);
        }
    }
}