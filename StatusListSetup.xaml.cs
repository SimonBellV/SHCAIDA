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
    /// Логика взаимодействия для StateListSetup.xaml
    /// </summary>
    public partial class StatusListSetup : Window
    {
        List<string> statuses = new List<string>();
        public StatusListSetup()
        {
            InitializeComponent();
            foreach (var val in ProgramMainframe.statusdb.CommonStatuses)
            {
                statuses.Add(val.Name);
                StatesLB.Items.Add(val.Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!statuses.Contains(StateTB.Text))
            {
                statuses.Add(StateTB.Text);
                StatesLB.Items.Add(StateTB.Text);
            }
            else
                MessageBox.Show("Уже есть такое состояние");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (StatesLB.SelectedItem != null)
            {
                string t = StatesLB.SelectedItem.ToString();
                StateTB.Text = t;
                _ = statuses.Remove(t);
                StatesLB.Items.Remove(t);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ProgramMainframe.UpdateStatuses(statuses);
            MessageBox.Show("Сохранено в базе данных");
        }
    }
}
