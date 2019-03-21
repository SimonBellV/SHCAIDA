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
    public partial class SensorListSetup : Window
    {
        public SensorListSetup()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSourceNameCB.Items.Clear();
            if (((ComboBoxItem)DataSourceTypeCB.SelectedItem).Content.ToString() == "Siemens")
            {
                foreach (var plc in ProgramMainframe.siemensClients.SiemensClients)
                    if(plc.Name!=null)
                        DataSourceNameCB.Items.Add(plc.Name);
            }
            else if (((ComboBoxItem)DataSourceTypeCB.SelectedItem).Content.ToString() == "Rockwell")
            {
                MessageBox.Show("Will be ready soon");
                /*foreach (var plc in ProgramMainframe.rockwellClients)
                    DataSourceNameCB.Items.Add(plc.name);*/
            }
            else if (((ComboBoxItem)DataSourceTypeCB.SelectedItem).Content.ToString() == "SQL Server")
                MessageBox.Show("Will be ready soon");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgramMainframe.siemensSensors.SiemensSensors.Add(new SiemensSensor(((ComboBoxItem)DataSourceNameCB.SelectedItem).Content.ToString(), NameTB.Text, AdressTB.Text));
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
