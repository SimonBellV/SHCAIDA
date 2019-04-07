using System;
using System.Windows;
using System.Windows.Controls;

namespace SHCAIDA
{
    public partial class RuleSensors : Window
    {
        public RuleSensors()
        {
            InitializeComponent();
            DataSourceTypeCB.Items.Add("Siemens");
            DataSourceTypeCB.Items.Add("Rockwell");
            DataSourceTypeCB.Items.Add("SQL Server");
            DataSourceTypeCB.Items.Add("Общее");
            foreach (var val in ProgramMainframe.linguisticVariables)
                if (!UsingSourceTypesLV.Items.Contains(val.sourceType))
                    UsingSourceTypesLV.Items.Add(val.sourceType);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSourceNameCB.Items.Clear();
            switch (DataSourceTypeCB.SelectedItem.ToString())
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
            if (DataSourceTypeCB.SelectedItem.ToString() == "Siemens")
                foreach (var sensor in ProgramMainframe.siemensSensors.SiemensSensors)
                    if (DataSourceNameCB.SelectedItem.ToString() == sensor.Source && !UsingSensorsLV.Items.Contains(sensor.Name))
                        SensorsCB.Items.Add(sensor.Name);

        }

        private void Button_Click(object sender, RoutedEventArgs e)//add
        {
            try
            {
                if (LeftBorderTB.Value.Value < RightBorderTB.Value.Value)
                {
                    if (!UsingSourceTypesLV.Items.Contains(DataSourceTypeCB.SelectedItem.ToString()))
                        UsingSourceTypesLV.Items.Add(DataSourceTypeCB.SelectedItem.ToString());
                    if (DataSourceTypeCB.SelectedItem.ToString() != "Общее"
                        && !UsingSourcesLV.Items.Contains(DataSourceNameCB.SelectedItem.ToString())
                        && UsingSourceTypesLV.SelectedItem != null
                        && DataSourceTypeCB.SelectedItem.ToString() == UsingSourceTypesLV.SelectedItem.ToString()
                        )
                        UsingSourcesLV.Items.Add(DataSourceNameCB.SelectedItem.ToString());
                    UsingSensorsLV.Items.Add(SensorsCB.SelectedItem.ToString());
                    if (DataSourceNameCB.SelectedItem != null)
                        ProgramMainframe.AddLingVariable(DataSourceTypeCB.SelectedItem.ToString(), DataSourceNameCB.SelectedItem.ToString(), SensorsCB.SelectedItem.ToString(), LeftBorderTB.Value.Value, RightBorderTB.Value.Value);
                    else
                        ProgramMainframe.AddLingVariable(DataSourceTypeCB.SelectedItem.ToString(), "", SensorsCB.SelectedItem.ToString(), LeftBorderTB.Value.Value, RightBorderTB.Value.Value);
                    SensorsCB.Items.Remove(SensorsCB.SelectedItem);
                }
                else throw new Exception("Несоответствие по границам");
            }
            catch (Exception exp)
            {
                MessageBox.Show("Проверьте корректность заполнения полей! " + exp);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (UsingSensorsLV.SelectedItem != null)
            {
                ProgramMainframe.RemoveLingVariable(UsingSensorsLV.SelectedItem.ToString());
                UsingSensorsLV.Items.Remove(UsingSensorsLV.SelectedItem.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RuleVariablesSetup form = new RuleVariablesSetup();
            form.Show();
            this.Close();
        }

        private void UsingSourceTypesLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UsingSensorsLV.Items.Clear();
            UsingSourcesLV.Items.Clear();
            if (UsingSourceTypesLV.SelectedItem.ToString() != "Общее")
            {
                foreach (var val in ProgramMainframe.linguisticVariables)
                    if (val.sourceType == UsingSourceTypesLV.SelectedItem.ToString() && !UsingSourcesLV.Items.Contains(val.source))
                        UsingSourcesLV.Items.Add(val.source);
            }
            else
            {
                foreach (var val in ProgramMainframe.linguisticVariables)
                    if (val.sourceType == "Общее")
                        UsingSensorsLV.Items.Add(val.name);
            }
        }

        private void UsingSourcesLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsingSourceTypesLV.SelectedItem.ToString() != "Общее")
            {
                UsingSensorsLV.Items.Clear();
                foreach (var val in ProgramMainframe.linguisticVariables)
                    if (val.sourceType == UsingSourceTypesLV.SelectedItem.ToString() && val.source == UsingSourcesLV.SelectedItem.ToString())
                        UsingSensorsLV.Items.Add(val.name);
            }
        }
    }
}
