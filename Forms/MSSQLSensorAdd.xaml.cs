using System.Windows;

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
