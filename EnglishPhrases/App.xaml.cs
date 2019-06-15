using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
        private static string PathToData = @"DBData";
        public static string PathToDB = Path.Combine(PathToData, @"Phrases.db");
        public static string PathToSounds = Path.Combine(PathToData, "Sounds");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DataBase.DB.Init();

            if (!Directory.Exists(PathToSounds))
            {
                Directory.CreateDirectory(PathToSounds);
            }

            Views.MainWindowV mw = new Views.MainWindowV();
            mw.Show();
        }
    }
}
