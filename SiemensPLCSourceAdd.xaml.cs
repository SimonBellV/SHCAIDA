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
            foreach (SiemensClient client in ProgramMainframe.siemensClients.SiemensClients)
            {
                if (client.IP == IPTB.Text)
                {
                    MessageBox.Show("Этот IP-адрес уже указан в базе данных");
                    check = true;
                    break;
                }
            }
            if (!check)
                ProgramMainframe.AddSiemensPLCSource(
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
