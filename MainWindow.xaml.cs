using SHCAIDA.Forms;
using System.Windows;

namespace SHCAIDA
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ProgramMainframe.DownloadSiemensSources();
            //ProgramMainframe.DownloadRockwellSources();
            //ProgramMainframe.DownloadSQLServerSources();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChooseDataSourceToAdd f = new ChooseDataSourceToAdd();
            f.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //ProgramMainframe.AddValue();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SiemensSensorAdd f = new SiemensSensorAdd();
            f.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RuleSensors f = new RuleSensors();
            f.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            StatusListSetup f = new StatusListSetup();
            f.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            JournalEvent f = new JournalEvent();
            f.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            StatusLoggingSetup f = new StatusLoggingSetup();
            f.Show();
        }

        private void ISLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemCooldownLUD.Value != null && SystemCooldownLUD.Value.Value > 0)
            {
                if (!ProgramMainframe.ISRunning)
                {
                    ISLaunchButton.Content = "Отключить СНП";
                    ProgramMainframe.ISTimeout = SystemCooldownLUD.Value.Value;
                    ProgramMainframe.ISRunning = true;
                }
                else
                {
                    ISLaunchButton.Content = "Включить СНП";
                    ProgramMainframe.ISRunning = false;
                }
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MSSQLSensorAdd f = new MSSQLSensorAdd();
            f.Show();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            GameDevicesSetup f = new GameDevicesSetup();
            f.Show();
        }
    }
}
