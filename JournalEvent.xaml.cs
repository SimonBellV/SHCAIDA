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
using System.Data.SqlClient;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для JournalEvent.xaml
    /// </summary>
    public partial class JournalEvent : Window
    {
        public JournalEvent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTableDG.Items.Clear();
            List<MessageJournal> journalSnapshot = new List<MessageJournal>();
            SqlParameter left = new SqlParameter("@leftData", LeftTimeDTP.Value.Value.Ticks);
            SqlParameter right = new SqlParameter("@rightData", RightTimeDTP.Value.Value.Ticks);
            SqlParameter state = new SqlParameter("@state", StateCB.SelectedItem.ToString());
            SqlParameter[] a = new SqlParameter[3];
            a[0] = left;
            a[1] = right;
            a[2] = state;
            //journalSnapshot = ProgramMainframe.journaldb.MessageJournals.SqlQuery<MessageJournal>("SELECT *  ", a);
        }
    }
}
