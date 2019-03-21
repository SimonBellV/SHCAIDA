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
    /// Логика взаимодействия для ChooseDataSourceToAdd.xaml
    /// </summary>
    public partial class ChooseDataSourceToAdd : Window
    {
        public ChooseDataSourceToAdd()
        {
            InitializeComponent();
        }

        private void RockwellRB_Checked(object sender, RoutedEventArgs e)
        {
            SiemensRB.IsChecked = false;
            MSSQLRB.IsChecked = false;
        }

        private void SiemensRB_Checked(object sender, RoutedEventArgs e)
        {
            RockwellRB.IsChecked = false;
            MSSQLRB.IsChecked = false;
        }

        private void MSSQLRB_Checked(object sender, RoutedEventArgs e)
        {
            SiemensRB.IsChecked = false;
            RockwellRB.IsChecked = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SiemensRB.IsChecked == true)
            {
                SiemensPLCSourceAdd siemensAdd = new SiemensPLCSourceAdd();
                siemensAdd.Show();
            }
        }
    }
}
