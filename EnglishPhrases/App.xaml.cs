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
        //string dir = AppDomain.CurrentDomain.BaseDirectory;
        private static string PathToData = @"DBData";
        private static string PathToDB = Path.Combine(PathToData, @"DBPhrases.db");
        private static string PathToSounds = Path.Combine(PathToData, "Sounds");

        public static string fullPathToDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToDB);
        public static string fullPathToSounds = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathToSounds);

        //public static string tempPath = Path.Combine(PathToData, @"Phrases.db");


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!Directory.Exists(PathToData))
            {
                Directory.CreateDirectory(PathToData);
            }

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
