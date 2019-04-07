using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCAIDA
{
    class MSSQLClient :INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string dataSource;
        private string initCatalog;
        private string user;
        private string password;
    }
}
