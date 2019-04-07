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
    /// Логика взаимодействия для SensorListSetup.xaml
    /// </summary>
    public partial class SiemensSensorAdd : Window
    {
        public SiemensSensorAdd()
        {
            InitializeComponent();
            DataSourceTypeCB.Items.Add("Siemens");
            DataSourceTypeCB.Items.Add("Rockwell");
            DataSourceTypeCB.Items.Add("SQL Server");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSourceNameCB.Items.Clear();
            if (DataSourceTypeCB.SelectedItem.ToString() == "Siemens")
            {
                foreach (var plc in ProgramMainframe.siemensClients.SiemensClients)
                    if(plc.Name!=null)
                        DataSourceNameCB.Items.Add(plc.Name);
            }
            else if (DataSourceTypeCB.SelectedItem.ToString() == "Rockwell")
            {
                MessageBox.Show("Will be ready soon");
                /*foreach (var plc in ProgramMainframe.rockwellClients)
                    DataSourceNameCB.Items.Add(plc.name);*/
            }
            else if (DataSourceTypeCB.SelectedItem.ToString() == "SQL Server")
                MessageBox.Show("Will be ready soon");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgramMainframe.siemensSensors.SiemensSensors.Add(new SiemensSensor(DataSourceNameCB.SelectedItem.ToString(), NameTB.Text, AdressTB.Text));
                ProgramMainframe.siemensSensors.SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Will be ready as soon as ready");
        }
    }
}
