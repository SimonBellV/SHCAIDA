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
using Accord.Fuzzy;

namespace SHCAIDA
{
    public partial class RuleSensors : Window
    {
        public RuleSensors()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSourceNameCB.Items.Clear();
            switch (((ComboBoxItem)DataSourceTypeCB.SelectedItem).Content.ToString())
            {
                case "Siemens":
                    {
                        foreach (var plc in ProgramMainframe.siemensClients.SiemensClients)
                            if (plc.Name != null)
                                DataSourceNameCB.Items.Add(plc.Name);
                        DataSourceNameCB.IsEnabled = true;
                        break;
                    }

                case "Rockwell":
                    {
                        MessageBox.Show("Will be ready soon");
                        DataSourceNameCB.IsEnabled = true;
                    }
                    break;
                case "SQL Server":
                    {
                        MessageBox.Show("Will be ready soon");
                        DataSourceNameCB.IsEnabled = true;
                    }
                    break;
                case "Общее":
                    {
                        foreach (var stat in ProgramMainframe.statusdb.CommonStatuses)
                            if (stat.Name != null)
                                SensorsCB.Items.Add(stat.Name);
                        DataSourceNameCB.IsEnabled = false;
                        break;
                    }
            }
        }

        private void DataSourceNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SensorsCB.Items.Clear();
            if (((ComboBoxItem)DataSourceTypeCB.SelectedItem).Content.ToString() == "Siemens")
                foreach (var sensor in ProgramMainframe.siemensSensors.SiemensSensors)
                    if (DataSourceNameCB.SelectedItem.ToString() == sensor.Source && !UsingSensorsLV.Items.Contains(sensor.Name))
                        SensorsCB.Items.Add(sensor.Name);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UsingSensorsLV.Items.Add(SensorsCB.SelectedItem);
                RulerCreator.AddLingVariable(SensorsCB.SelectedItem.ToString(), Convert.ToSingle(LeftBorderTB.Text), Convert.ToSingle(RightBorderTB.Text));
                SensorsCB.Items.Remove(SensorsCB.SelectedItem);
            }
            catch
            {
                MessageBox.Show("Проверьте корректность заполнения полей!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (UsingSensorsLV.SelectedItem != null)
            {
                UsingSensorsLV.Items.Remove(UsingSensorsLV.SelectedItem);
#pragma warning disable CS0253 // Возможно, использовано непреднамеренное сравнение ссылок: для правой стороны требуется приведение
                _ = RulerCreator.fuzzySensors.Remove(RulerCreator.fuzzySensors.Find(x => x.Name == UsingSensorsLV.SelectedItem));
#pragma warning restore CS0253 // Возможно, использовано непреднамеренное сравнение ссылок: для правой стороны требуется приведение
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RuleVariablesSetup form = new RuleVariablesSetup();
            form.Show();
            this.Close();
        }
    }
}
