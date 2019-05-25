using System.Windows;
using System.Windows.Controls;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для PLCSourceAdd.xaml
    /// </summary>
    public partial class SiemensPLCSourceAdd : Window
    {
        public SiemensPLCSourceAdd()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            foreach (SiemensClient client in ProgramMainframe.SiemensClients.SiemensClients)
            {
                if (client.IP == IPTB.Text)
                {
                    MessageBox.Show("Этот IP-адрес уже указан в базе данных");
                    check = true;
                    break;
                }
            }
            if (!check)
                ProgramMainframe.AddSiemensPlcSource(
                    ((ComboBoxItem)PLCTypeCB.SelectedItem).Content.ToString(),
                    IPTB.Text,
                    RackNUD.Value.Value,
                    SlotNUD.Value.Value,
                    SourceNameTB.Text);
        }

        private void CheckStatusButton_Click(object sender, RoutedEventArgs e)
        {
            SiemensClient test = new SiemensClient(
                ((ComboBoxItem)PLCTypeCB.SelectedItem).Content.ToString(),
                IPTB.Text,
                RackNUD.Value.Value,
                SlotNUD.Value.Value,
                SourceNameTB.Text);
            if (test.CheckConnection())
                MessageBox.Show("Соединение установлено");
            else
                MessageBox.Show("Соединение не установлено");
        }
    }
}
