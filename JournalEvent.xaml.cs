using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace SHCAIDA
{
    public partial class JournalEvent : Window
    {
        public JournalEvent()
        {
            InitializeComponent();
            SensorSourceTypeCB.Items.Add("Siemens");
            SensorSourceTypeCB.Items.Add("Rockwell");
            SensorSourceTypeCB.Items.Add("SQL Server");
            SensorSourceTypeCB.Items.Add("Общее");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AllFieldsNotNull())
            {
                DataTableDG.Items.Clear();
                List<MessageJournal> journalSnapshot = new List<MessageJournal>();
                SQLiteParameter left = new SQLiteParameter("@leftData", LeftTimeDTP.Value.Value.Ticks);
                SQLiteParameter right = new SQLiteParameter("@rightData", RightTimeDTP.Value.Value.Ticks);
                SQLiteParameter state = new SQLiteParameter("@state", StateCB.SelectedItem.ToString());
                SQLiteParameter sensor = new SQLiteParameter("@sensor", SensorCB.SelectedItem.ToString());
                SQLiteParameter[] a = new SQLiteParameter[4];
                a[0] = left;
                a[1] = right;
                a[2] = state;
                a[3] = sensor;
                journalSnapshot = ProgramMainframe.journaldb.MessageJournals.SqlQuery("SELECT *  FROM MessageJournal WHERE State=@state AND Time >= @leftData AND Time <= @rightData AND Sensor = @sensor", a).ToList();
                foreach (var message in journalSnapshot)
                    DataTableDG.Items.Add(message.Sensor + " " + message.State + " " + Convert.ToDateTime(message.Time));
            }
            else
                MessageBox.Show("Проверьте все ли поля заполнены!");
        }

        private bool AllFieldsNotNull()
        {
            if (LeftTimeDTP.Value.Value != null &&
                RightTimeDTP.Value.Value != null &&
                StateCB.SelectedItem != null &&
                SensorCB.SelectedItem != null)
                return true;
            else
                return false;
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
    }
}
