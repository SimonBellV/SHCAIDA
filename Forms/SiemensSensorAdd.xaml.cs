﻿using System;
using System.Windows;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для SensorListSetup.xaml
    /// </summary>
    public partial class SiemensSensorAdd : Window
    {
        public SiemensSensorAdd()
        {
            InitializeComponent();
            InputDeviceTypeCB.Items.Add("Датчик");
            InputDeviceTypeCB.Items.Add("Регулятор");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InputDeviceType input;
            try
            {
                switch (InputDeviceTypeCB.SelectedItem.ToString())
                {
                    case "Датчик":
                        input = InputDeviceType.Sensor;
                        break;
                    case "Регулятор":
                        input = InputDeviceType.Regulator;
                        break;
                    default:
                        throw new Exception("Ошибка по выбранному типу устройста");
                }
                ProgramMainframe.SiemensSensors.SiemensSensors.Add(new SiemensSensor(DataSourceNameCB.SelectedItem.ToString(), NameTB.Text, AdressTB.Text, input));
                ProgramMainframe.SiemensSensors.SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Will be ready as soon as ready");
        }
    }
}
