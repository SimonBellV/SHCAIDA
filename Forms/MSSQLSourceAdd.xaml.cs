using System.Windows;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для SQLSourceConnectionPropertiesSetup.xaml
    /// </summary>
    public partial class MSSQLSourceAdd : Window
    {
        public MSSQLSourceAdd()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //uncomment additional condition on release
            if (DataSourceTB.Text != null && InitCatalogTB.Text != null && UserTB.Text != null && PasswordTB.Password != null/* && CheckConnection()*/)
            {
                ProgramMainframe.MssqlClients.MSSQLClients.Add(new MSSQLClient(DataSourceTB.Text, InitCatalogTB.Text, UserTB.Text, PasswordTB.Password));
                ProgramMainframe.MssqlClients.SaveChanges();
            }

        }

        private void CheckStatusButton_Click(object sender, RoutedEventArgs e)
        {
            CheckConnection();
        }

        /// <summary>
        /// Проверяет успешность соединения - без этого клиент не подвяжется к БД
        /// </summary>
        private bool CheckConnection()
        {
            if (new MSSQLClient(DataSourceTB.Text, InitCatalogTB.Text, UserTB.Text, PasswordTB.Password).CheckConnection())
            {
                MessageBox.Show("Соединение успешно проверено");
                return true;
            }
            else
            {
                MessageBox.Show("Соединение не установлено");
                return false;
            }
        }
    }
}
