using System;
using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{
    public struct DeviceType
    {
        public string name;
        public TypeOfDataSources type;

        public DeviceType(string name, TypeOfDataSources type)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.type = type;
        }
    }
    /// <summary>
    /// Логика взаимодействия для GameDevicesSetup.xaml
    /// </summary>
    public partial class GameDevicesSetup : Window
    {
        private readonly List<DeviceType> allDevices;
        private List<DeviceType> usedDevices;
        public GameDevicesSetup()
        {
            InitializeComponent();
            allDevices = new List<DeviceType>();
            usedDevices = new List<DeviceType>();
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
                allDevices.Add(new DeviceType(device.Name, TypeOfDataSources.Siemens));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RegulatorsToSelectLB.SelectedItem != null)
            {
                SelectedRegulatorsLB.Items.Add(RegulatorsToSelectLB.SelectedItem.ToString());
                usedDevices.Add(allDevices.Find(x => x.name == RegulatorsToSelectLB.SelectedItem.ToString()));
                RegulatorsToSelectLB.Items.RemoveAt(RegulatorsToSelectLB.SelectedIndex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedRegulatorsLB.SelectedItem != null)
            {
                RegulatorsToSelectLB.Items.Add(SelectedRegulatorsLB.SelectedItem.ToString());
                usedDevices.Remove(allDevices.Find(x => x.name == SelectedRegulatorsLB.SelectedItem.ToString()));
                SelectedRegulatorsLB.Items.RemoveAt(SelectedRegulatorsLB.SelectedIndex);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (StateSensorsToSelectLB.SelectedItem != null)
            {
                SelectedStateSensorsLB.Items.Add(StateSensorsToSelectLB.SelectedItem.ToString());
                usedDevices.Add(allDevices.Find(x => x.name == StateSensorsToSelectLB.SelectedItem.ToString()));
                OutputSensorsToSelectLB.Items.Remove(StateSensorsToSelectLB.SelectedItem);
                StateSensorsToSelectLB.Items.Remove(StateSensorsToSelectLB.SelectedItem);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (OutputSensorsToSelectLB.SelectedItem != null)
            {
                SelectedOutputSensorsLB.Items.Add(OutputSensorsToSelectLB.SelectedItem.ToString());
                usedDevices.Add(allDevices.Find(x => x.name == OutputSensorsToSelectLB.SelectedItem.ToString()));
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
                usedDevices.Remove(allDevices.Find(x => x.name == SelectedStateSensorsLB.SelectedItem.ToString()));
                SelectedStateSensorsLB.Items.RemoveAt(SelectedStateSensorsLB.SelectedIndex);
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (SelectedOutputSensorsLB.SelectedItem != null)
            {
                StateSensorsToSelectLB.Items.Add(SelectedOutputSensorsLB.SelectedItem.ToString());
                OutputSensorsToSelectLB.Items.Add(SelectedOutputSensorsLB.SelectedItem.ToString());
                usedDevices.Remove(allDevices.Find(x => x.name == SelectedOutputSensorsLB.SelectedItem.ToString()));
                SelectedOutputSensorsLB.Items.RemoveAt(SelectedOutputSensorsLB.SelectedIndex);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (nodeNameTB.Text != null && nodeDescriptionTB.Text != null && RegulatorsIntervalsIUD.Value != null && StateSensorsIntervalsIUD.Value != null && SelectedOutputSensorsLB.Items.Count != 0 && SelectedRegulatorsLB.Items.Count != 0 && SelectedStateSensorsLB.Items.Count != 0)
            {
                GameNode node = new GameNode(nodeNameTB.Text.ToString(), nodeDescriptionTB.Text.ToString(), RegulatorsIntervalsIUD.Value.Value, StateSensorsIntervalsIUD.Value.Value);
                foreach (var item in SelectedRegulatorsLB.Items)
                {
                    var type = usedDevices.Find(x => x.name == item.ToString()).type;
                    node.usedSensors.Add(new RoledDevice(ProgramMainframe.GetSensorIDByName(item.ToString(), type), GameRole.Regulator, type));
                }
                foreach (var item in SelectedStateSensorsLB.Items)
                {
                    var type = usedDevices.Find(x => x.name == item.ToString()).type;
                    node.usedSensors.Add(new RoledDevice(ProgramMainframe.GetSensorIDByName(item.ToString(), type), GameRole.StateSensor, type));
                }
                foreach (var item in SelectedOutputSensorsLB.Items)
                {
                    var type = usedDevices.Find(x => x.name == item.ToString()).type;
                    node.usedSensors.Add(new RoledDevice(ProgramMainframe.GetSensorIDByName(item.ToString(), type), GameRole.OutputSensor, type));
                }
                ProgramMainframe.gameTheoryController.Add(node);
                ProgramMainframe.WriteGameNodes();
            }
            else
                MessageBox.Show("Проверьте корректность полей");
        }
    }
}
