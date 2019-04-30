using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{
    public partial class RuleWizard : Window
    {
        public RuleWizard()
        {
            InitializeComponent();
            Rule += "IF ";
            foreach (var sensor in ProgramMainframe.LinguisticVariables)
                SensorsLB.Items.Add(sensor.name);
            foreach (var rule in ProgramMainframe.Rules)
                CurrentRulesLB.Items.Add(rule.RuleStr);
        }

        public string Rule { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                Rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " AND ";
            }
            RuleTB.Text = Rule;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                Rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " OR ";
            }
            RuleTB.Text = Rule;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                Rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " THEN ";
                RuleTB.Text = Rule;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (RuleCollisionDetection())
            {
                
                Rule += SensorsLB.SelectedItem + " IS " + StatusLB.SelectedItem;
                CurrentRulesLB.Items.Add(Rule);
                ProgramMainframe.Rules.Add(new Rule(Rule));
                Rule = "IF ";
                RuleTB.Text = Rule;
            }
        }

        private bool RuleCollisionDetection()
        {
            return true;
        }

        private void SensorsLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StatusLB.Items.Clear();
            string sensorName = SensorsLB.SelectedItem.ToString();
            List<string> labels = new List<string>(ProgramMainframe.GetSensorLabels(sensorName));
            foreach (var val in labels)
                StatusLB.Items.Add(val);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ProgramMainframe.UpdateFuzzyDBRuleDB();
            MessageBox.Show("Обновление успешно завершено");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ProgramMainframe.Rules.RemoveAt(CurrentRulesLB.SelectedIndex);
            CurrentRulesLB.Items.Remove(CurrentRulesLB.SelectedItem);
        }
    }
}
