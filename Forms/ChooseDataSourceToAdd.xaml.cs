using System.Windows;

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
            else if (MSSQLRB.IsChecked == true)
            {
                MSSQLSourceAdd f = new MSSQLSourceAdd();
                f.Show();
            }
        }
    }
}
