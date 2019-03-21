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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            SensorListSetup f = new SensorListSetup();
            f.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            RulerCreator.InitRuleCreation();
            RuleSensors f = new RuleSensors();
            f.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            StatusListSetup f = new StatusListSetup();
            f.Show();
        }
    }
}
