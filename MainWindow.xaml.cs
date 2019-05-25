using BPMN;
using Microsoft.Win32;
using SHCAIDA.Forms;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChooseDataSourceToAdd f = new ChooseDataSourceToAdd();
            f.Show();
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

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            GameNodesControlRoom f = new GameNodesControlRoom();
            f.Show();
        }

        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemStoringCooldownLUD.Value != null && SystemStoringCooldownLUD.Value.Value > 0)
            {
                if (!ProgramMainframe.StoragingRunning)
                {
                    StorageButton.Content = "Отключить запись данных в БД";
                    ProgramMainframe.StoragingTimeout = SystemStoringCooldownLUD.Value.Value;
                    ProgramMainframe.StoragingRunning = true;
                }
                else
                {
                    StorageButton.Content = "Включить запись данных в БД";
                    ProgramMainframe.StoragingRunning = false;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "BPMN файлы(*.bpmn)|*.bpmn"
            };
            openFileDialog1.ShowDialog();
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            Model model = Model.Read(filename);
            System.Drawing.Image img = model.GetImage(0, 2.0f);
            string path = "@" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
            img.Save(path, ImageFormat.Png);
            Process.Start(path);
        }
    }
}
