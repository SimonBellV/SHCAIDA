﻿using System.Collections.Generic;
using System.Windows;

namespace SHCAIDA
{

    public partial class StatusListSetup : Window
    {
        private readonly List<string> statuses = new List<string>();
        public StatusListSetup()
        {
            InitializeComponent();
            foreach (var val in ProgramMainframe.Statusdb.CommonStatuses)
            {
                statuses.Add(val.Name);
                StatesLB.Items.Add(val.Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) // add to list
        {
            if (!statuses.Contains(StateTB.Text))
            {
                statuses.Add(StateTB.Text);
                StatesLB.Items.Add(StateTB.Text);
                StateTB.Clear();
            }
            else
                MessageBox.Show("Уже есть такое состояние");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) // edit
        {
            if (StatesLB.SelectedItem != null)
            {
                string t = StatesLB.SelectedItem.ToString();
                StateTB.Text = t;
                _ = statuses.Remove(t);
                StatesLB.Items.Remove(t);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) // save in db
        {
            ProgramMainframe.UpdateStatuses(statuses, TypeOfDataSources.Common);
            MessageBox.Show("Сохранено в базе данных");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
            //возможно добавить обработчик событий для измененных, но не сохраненных данных
        }
    }
}
