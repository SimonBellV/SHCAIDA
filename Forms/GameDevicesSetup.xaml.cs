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
    /// Логика взаимодействия для GameDevicesSetup.xaml
    /// </summary>
    public partial class GameDevicesSetup : Window
    {
        public GameDevicesSetup()
        {
            InitializeComponent();
            foreach (var device in ProgramMainframe.SiemensSensors.SiemensSensors)
            {
                switch (device.DeviceType)
                {
                    case InputDeviceType.Regulator:
                        RegulatorsToSelectLB.Items.Add(device.Name);
                        break;
                    case InputDeviceType.Sensor:
                        {
                            StateSensorsToSelectLB.Items.Add(device.Name);
                            OutputSensorsToSelectLB.Items.Add(device.Name);
                        }
                        break;
                    default:
                        throw new Exception("Ошибка типов устройств");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RegulatorsToSelectLB.SelectedItem != null)
            {
                SelectedRegulatorsLB.Items.Add(RegulatorsToSelectLB.SelectedItem.ToString());
                RegulatorsToSelectLB.Items.RemoveAt(RegulatorsToSelectLB.SelectedIndex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedRegulatorsLB.SelectedItem != null)
            {
                RegulatorsToSelectLB.Items.Add(SelectedRegulatorsLB.SelectedItem.ToString());
                SelectedRegulatorsLB.Items.RemoveAt(SelectedRegulatorsLB.SelectedIndex);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (StateSensorsToSelectLB.SelectedItem != null)
            {
                SelectedStateSensorsLB.Items.Add(StateSensorsToSelectLB.SelectedItem.ToString());                
                OutputSensorsToSelectLB.Items.Remove(StateSensorsToSelectLB.SelectedItem);
                StateSensorsToSelectLB.Items.Remove(StateSensorsToSelectLB.SelectedItem);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (OutputSensorsToSelectLB.SelectedItem != null)
            {
                SelectedOutputSensorsLB.Items.Add(OutputSensorsToSelectLB.SelectedItem.ToString());
                StateSensorsToSelectLB.Items.Remove(OutputSensorsToSelectLB.SelectedItem);
                OutputSensorsToSelectLB.Items.Remove(OutputSensorsToSelectLB.SelectedItem);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (SelectedStateSensorsLB.SelectedItem != null)
            {
                StateSensorsToSelectLB.Items.Add(SelectedStateSensorsLB.SelectedItem.ToString());
                OutputSensorsToSelectLB.Items.Add(SelectedStateSensorsLB.SelectedItem.ToString());
                SelectedStateSensorsLB.Items.RemoveAt(SelectedStateSensorsLB.SelectedIndex);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (SelectedOutputSensorsLB.SelectedItem != null)
            {
                StateSensorsToSelectLB.Items.Add(SelectedOutputSensorsLB.SelectedItem.ToString());
                OutputSensorsToSelectLB.Items.Add(SelectedOutputSensorsLB.SelectedItem.ToString());
                SelectedOutputSensorsLB.Items.RemoveAt(SelectedOutputSensorsLB.SelectedIndex);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (nodeNameTB.Text != null && nodeDescriptionTB.Text != null && RegulatorsIntervalsIUD.Value != null && StateSensorsIntervalsIUD.Value != null && SelectedOutputSensorsLB.Items.Count != 0 && SelectedRegulatorsLB.Items.Count != 0 && SelectedStateSensorsLB.Items.Count != 0)
            {
                GameNode node = new GameNode(nodeNameTB.Text.ToString(), nodeDescriptionTB.Text.ToString(), RegulatorsIntervalsIUD.Value.Value, StateSensorsIntervalsIUD.Value.Value);
                foreach (var item in SelectedRegulatorsLB.Items)
                    node.usedSensors.Add(new RoledSiemensSensor(ProgramMainframe.GetSensorIDByName(item.ToString()), GameRole.Regulator));
                foreach(var item in SelectedStateSensorsLB.Items)
                    node.usedSensors.Add(new RoledSiemensSensor(ProgramMainframe.GetSensorIDByName(item.ToString()), GameRole.StateSensor));
                foreach (var item in SelectedOutputSensorsLB.Items)
                    node.usedSensors.Add(new RoledSiemensSensor(ProgramMainframe.GetSensorIDByName(item.ToString()), GameRole.OutputSensor));
                ProgramMainframe.gameTheoryController.Add(node);
                ProgramMainframe.WriteGameNodes();
            }
            else
                MessageBox.Show("Проверьте корректность полей");
        }
    }
}
