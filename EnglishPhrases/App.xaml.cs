using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EnglishPhrases
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string PathToDB = @"Phrases.db";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DB.DBSqlite.Init();
            Views.MainWindowV mw = new Views.MainWindowV();
            mw.Show();
        }
    }
}
