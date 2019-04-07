﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHCAIDA
{
    public class MSSQLClient :INotifyPropertyChanged
    {
        public int ID { get; set; }

        public string DataSource
        {
            get => dataSource;
            set
            {
                dataSource = value;
                OnPropertyChanged("DataSource");
            }
        }

        public string InitCatalog
        {
            get => initCatalog;
            set
            {
                initCatalog = value;
                OnPropertyChanged("InitCatalog");
            }
        }

        public string User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private string dataSource;
        private string initCatalog;
        private string user;
        private string password;

        public MSSQLClient(string dataSource, string initCatalog, string user, string password)
        {
            ID = -1;
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            this.initCatalog = initCatalog ?? throw new ArgumentNullException(nameof(initCatalog));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string ConnectionString => @"Data Source=" + dataSource + ";Initial Catalog=" + initCatalog + ";User ID=" + user + ";Password=" + password;

        public bool CheckConnection()
        {
            var cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();                
                cnn.Close();
                return true;
            }
            catch { return false;  }
        }
    }
}
