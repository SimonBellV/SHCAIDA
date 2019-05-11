using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
