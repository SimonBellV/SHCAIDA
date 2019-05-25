using System.Windows;

namespace SHCAIDA
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            ProgramMainframe.InitMainframe();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ProgramMainframe.WriteRules();
            ProgramMainframe.WriteFuzzyDB();
            ProgramMainframe.WriteGameNodes();
        }
    }
}
