using System.Windows;

namespace SHCAIDA
{
    public partial class RuleWizard : Window
    {
        string rule;
        public RuleWizard()
        {
            InitializeComponent();
            rule += "IF ";
            foreach (var sensor in RulerCreator.fuzzySensors)
                SensorsLB.Items.Add(sensor.Name);
            foreach (var status in RulerCreator.fuzzySets)
                StatusLB.Items.Add(status.Name);
        }

        public string Rule { get => rule; set => rule = value; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " AND ";
            }
            RuleTB.Text = rule;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " OR ";
            }
            RuleTB.Text = rule;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (SensorsLB.SelectedItem != null && StatusLB.SelectedItem != null)
            {
                string t = " ";
                if ((bool)NotCheck.IsChecked)
                    t = " Not ";
                rule += SensorsLB.SelectedItem + " IS" + t + StatusLB.SelectedItem + " THEN ";
                RuleTB.Text = rule;
                if(RulerCreator.enableStatusVariable)
                    SensorsLB.Items.Add("Статус");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (RuleCollisionDetection())
            {
                rule += SensorsLB.SelectedItem + " IS " + StatusLB.SelectedItem;
                ProgramMainframe.IS.NewRule("", rule);
                CurrentRulesLB.Items.Add(rule);
                rule = "IF ";
                if (RulerCreator.enableStatusVariable)
                    SensorsLB.Items.Remove("Статус");
            }
        }

        private bool RuleCollisionDetection()
        {
            return true;
        }
    }
}
