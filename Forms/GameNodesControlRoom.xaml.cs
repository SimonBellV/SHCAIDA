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
    /// Логика взаимодействия для GameNodesControlRoom.xaml
    /// </summary>
    public partial class GameNodesControlRoom : Window
    {
        public GameNodesControlRoom()
        {
            InitializeComponent();
            foreach (var node in ProgramMainframe.gameTheoryController)
                NodeListCB.Items.Add(node.nodeName);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var node in ProgramMainframe.gameTheoryController)
                if (node.nodeName == NodeListCB.SelectedItem.ToString())
                {
                    NodeStatsTB.Text = "";
                    NodeStatsTB.Text = node.NodeStats;
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NodeListCB.SelectedItem != null)
            {
                ProgramMainframe.gameTheoryController.Remove(ProgramMainframe.gameTheoryController.Find(x => x.nodeName == NodeListCB.SelectedItem.ToString()));
                ProgramMainframe.WriteGameNodes();
                MessageBox.Show("Список нод обновлен");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (NodeListCB.SelectedItem != null && leftData.Value != null && rightData.Value != null)
            {
                if (leftData.Value.Value < rightData.Value.Value)
                {
                    ProgramMainframe.gameTheoryController.Find(x => x.nodeName == NodeListCB.SelectedItem.ToString()).FillData(leftData.Value.Value, rightData.Value.Value);
                    ProgramMainframe.gameTheoryController.Find(x => x.nodeName == NodeListCB.SelectedItem.ToString()).PlayGame();
                }
                else MessageBox.Show("Проверьте корректность введенных дат");
            }
            else MessageBox.Show("Проверьте все ли поля заполнены");
        }
    }
}
