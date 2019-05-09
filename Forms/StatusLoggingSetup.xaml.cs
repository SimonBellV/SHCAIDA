using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SHCAIDA
{
    public partial class StatusLoggingSetup : Window
    {
        public StatusLoggingSetup()
        {
            InitializeComponent();
            SensorSourceTypeCB.Items.Add("Siemens");
            SensorSourceTypeCB.Items.Add("Rockwell");
            SensorSourceTypeCB.Items.Add("SQL Server");
            SensorSourceTypeCB.Items.Add("Общее");
        }

        private void SensorSourceTypeCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SensorSourceCB.Items.Clear();
            var sources = ProgramMainframe.LinguisticVariables.FindAll(x => x.sourceType == SensorSourceTypeCB.SelectedItem.ToString());
            if (SensorSourceTypeCB.SelectedItem.ToString() != "Общее")
            {
                SensorSourceCB.IsEnabled = true;
                foreach (var ling in sources)
                    if (!SensorSourceCB.Items.Contains(ling.source))
                        SensorSourceCB.Items.Add(ling.source);
            }
            else
            {
                SensorSourceCB.IsEnabled = false;
                foreach (var ling in sources)
                    if (!SensorSourceCB.Items.Contains(ling.source))
                        SensorCB.Items.Add(ling.name);
            }
        }

        private void SensorSourceCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SensorCB.Items.Clear();
            switch (SensorSourceTypeCB.SelectedItem.ToString())
            {
                case "Siemens":
                    {
                        if (ProgramMainframe.SiemensSensors.SiemensSensors.Count() != 0)
                        {
                            foreach (var sensor in ProgramMainframe.SiemensSensors.SiemensSensors)
                                if (SensorSourceCB.SelectedItem.ToString() == sensor.Source)
                                    SensorCB.Items.Add(sensor.Name);
                        }
                        else
                            MessageBox.Show("Для этого источника еще не заданы датчики");
                    }
                    break;
                case "SQL Server":
                    {
                        if (ProgramMainframe.MssqlSensors.MSSQLSensors.Count() != 0)
                        {
                            foreach (var conn in ProgramMainframe.Mssqlconnections)
                                if (SensorSourceCB.SelectedItem.ToString() == conn.Client.InitCatalog)
                                    SensorCB.Items.Add(conn.Sensor.Name);
                        }
                        else
                            MessageBox.Show("Для этого источника еще не заданы датчики");
                    }
                    break;
                default:
                    throw new System.Exception("Not ready!");
            }
        }

        private void SensorCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StateCB.Items.Clear();
            try
            {
                List<Status> statuses = ProgramMainframe.LinguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).labels;
                if (statuses.Count != 0)
                    foreach (var status in statuses)
                    {
                        StateCB.Items.Add(status.name);
                    }
                else
                    MessageBox.Show("Для этого датчика нет заданных состояних");
            }
            catch { }
        }

        private void StateCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StateCB.SelectedItem != null)
                LogStatusChB.IsChecked = ProgramMainframe.LinguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).IsLoggingActive(StateCB.SelectedItem.ToString());
        }

        private void LogUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (StateCB.SelectedItem != null)
                ProgramMainframe.LinguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).UpdateLogging(StateCB.SelectedItem.ToString(), LogStatusChB.IsChecked.Value);
            ProgramMainframe.WriteFuzzyDB();
            if (SensorSourceTypeCB.SelectedItem.ToString() == "Siemens")
                ProgramMainframe.Ssconnections.Find(x => x.Sensor.Name == SensorCB.SelectedItem.ToString() &&  x.Client.Name == SensorSourceCB.SelectedItem.ToString()).isStoringInDB = StoreInDBStatusChB.IsChecked.Value;
            if (SensorSourceTypeCB.SelectedItem.ToString() == "SQL Server")
                ProgramMainframe.Mssqlconnections.Find(x => x.Sensor.Name == SensorCB.SelectedItem.ToString() && x.Client.DataSource == SensorSourceCB.SelectedItem.ToString()).isStoringInDB = StoreInDBStatusChB.IsChecked.Value;

        }
    }
}
