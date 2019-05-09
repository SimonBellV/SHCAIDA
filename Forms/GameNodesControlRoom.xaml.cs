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

namespace SHCAIDA.Forms
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
                NodeListLB.Items.Add(node.nodeName);
        }

        private void NodeListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var node in ProgramMainframe.gameTheoryController)
                if (node.nodeName == NodeListLB.SelectedItem.ToString())
                {
                    NodeStatsTB.Text = "";
                    NodeStatsTB.Text = node.NodeStats;
                }
        }
    }
}
