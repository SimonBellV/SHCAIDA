using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для JournalEvent.xaml
    /// </summary>
    public partial class JournalEvent : Window
    {
        public JournalEvent()
        {
            InitializeComponent();
            SensorSourceTypeCB.Items.Add("Siemens");
            SensorSourceTypeCB.Items.Add("Rockwell");
            SensorSourceTypeCB.Items.Add("SQL Server");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTableDG.Items.Clear();
            List<MessageJournal> journalSnapshot = new List<MessageJournal>();
            SqlParameter left = new SqlParameter("@leftData", LeftTimeDTP.Value.Value.Ticks);
            SqlParameter right = new SqlParameter("@rightData", RightTimeDTP.Value.Value.Ticks);
            SqlParameter state = new SqlParameter("@state", StateCB.SelectedItem.ToString());
            SqlParameter sensor = new SqlParameter("@state", SensorCB.SelectedItem.ToString());
            SqlParameter[] a = new SqlParameter[4];
            a[0] = left;
            a[1] = right;
            a[2] = state;
            a[3] = sensor;
            journalSnapshot = ProgramMainframe.journaldb.MessageJournals.SqlQuery("SELECT *  FROM MessageJournal WHERE State=@state AND Time >= @leftData AND Time <= @rightData AND Sensor = @sensor", a).ToList();
            foreach (var message in journalSnapshot)
                DataTableDG.Items.Add(message.Sensor + " " + message.State + " " + Convert.ToDateTime(message.Time));
        }

        private void SensorSourceTypeCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SensorSourceCB.Items.Clear();
            switch (SensorSourceTypeCB.SelectedItem.ToString())
            {
                case "Siemens":
                    {
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
            }
        }

        private void SensorSourceCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SensorCB.Items.Clear();
            if (SensorSourceTypeCB.SelectedItem.ToString() == "Siemens")
                foreach (var sensor in ProgramMainframe.siemensSensors.SiemensSensors)
                    if (SensorSourceTypeCB.SelectedItem.ToString() == sensor.Source)
                        SensorCB.Items.Add(sensor.Name);
        }

        private void StateCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SensorCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
