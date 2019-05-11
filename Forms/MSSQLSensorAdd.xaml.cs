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

namespace SHCAIDA.Forms
{
    /// <summary>
    /// Логика взаимодействия для MSSQLSensorAdd.xaml
    /// </summary>
    public partial class MSSQLSensorAdd : Window
    {
        public MSSQLSensorAdd()
        {
            InitializeComponent();
            foreach (var client in ProgramMainframe.MssqlClients.MSSQLClients)
                SQLClientsCB.Items.Add(client.DataSource);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Will be ready as soon as ready");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FieldsNotNull)
            {
                ProgramMainframe.MssqlSensors.MSSQLSensors.Add(new MSSQLSensor(ProgramMainframe.GetMssqlClientId(SQLClientsCB.SelectedItem.ToString()), DatabaseNameTB.Text, HeaderNameTB.Text, NameTB.Text));
                ProgramMainframe.MssqlSensors.SaveChanges();
                MessageBox.Show("Устройство успешно добавлено");
            }
        }

        private bool FieldsNotNull => SQLClientsCB.SelectedItem != null && DatabaseNameTB.Text != null && HeaderNameTB.Text != null && NameTB.Text != null;
    }
}
