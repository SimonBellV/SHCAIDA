using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для StatusLoggingSetup.xaml
    /// </summary>
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
            switch (SensorSourceTypeCB.SelectedItem.ToString())
            {
                case "Siemens":
                    {
                        SensorSourceCB.IsEnabled = true;
                        foreach (var plc in ProgramMainframe.siemensClients.SiemensClients)
                            if (plc.Name != null)
                                SensorSourceCB.Items.Add(plc.Name);
                        break;
                    }

                case "Rockwell":
                    {
                        MessageBox.Show("Will be ready soon");
                    }
                    break;
                case "SQL Server":
                    {
                        MessageBox.Show("Will be ready soon");
                    }
                    break;
                case "Общее":
                    {
                        SensorSourceCB.IsEnabled = false;
                        MessageBox.Show(ProgramMainframe.statusdb.CommonStatuses.Count().ToString());
                        foreach (var stat in ProgramMainframe.statusdb.CommonStatuses)
                            if (stat.Name != null)
                                SensorCB.Items.Add(stat.Name);
                    }
                    break;
            }
        }

        private void SensorSourceCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SensorCB.Items.Clear();
            if (SensorSourceTypeCB.SelectedItem.ToString() == "Siemens")
            {
                if (ProgramMainframe.siemensSensors.SiemensSensors.Count() != 0)
                {
                    foreach (var sensor in ProgramMainframe.siemensSensors.SiemensSensors)
                        if (SensorSourceCB.SelectedItem.ToString() == sensor.Source)
                            SensorCB.Items.Add(sensor.Name);
                }
                else
                    MessageBox.Show("Для этого источника еще не заданы датчики");
            }
        }

        private void SensorCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StateCB.Items.Clear();
            try
            {
                List<Status> statuses = ProgramMainframe.linguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).labels;
                if (statuses.Count != 0)
                    foreach (var status in statuses)
                        StateCB.Items.Add(status.name);
                else
                    MessageBox.Show("Для этого датчика нет заданных состояних");
            }
            catch { }
        }

        private void StateCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StateCB.SelectedItem!=null)
                LogStatusChB.IsChecked = ProgramMainframe.linguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).IsLoggingActive(StateCB.SelectedItem.ToString());
        }

        private void LogUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (StateCB.SelectedItem != null)
                ProgramMainframe.linguisticVariables.Find(x => x.name == SensorCB.SelectedItem.ToString()).UpdateLogging(StateCB.SelectedItem.ToString(), LogStatusChB.IsChecked.Value);
        }
    }
}
