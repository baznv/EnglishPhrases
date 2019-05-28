using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;
using System.Data.Common;

namespace EnglishPhrases
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //const string dataBaseName = @"D:\Projects\EnglishPhrases\EnglishPhrases\EnglishPhrases\DB.sqlite";
        //List<string> namesTables = new List<string> {"Words", "EnglishPhrases", "RussianPhrases" };

        public MainWindow()
        {
            InitializeComponent();
            DB.Init();


        //    List<string> lstTables = new List<string>();
        //    if (ExistBD())
        //    {
        //        lstTables = ExistTables();
        //    }

        //}

        //private List<string> ExistTables()
        //{
        //    List<string> lst = new List<string>();
        //    SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", dataBaseName));
        //    connection.Open();
        //    SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;", connection);
        //    SQLiteDataReader reader = command.ExecuteReader();
        //    foreach (DbDataRecord record in reader)
        //    {
        //        lst.Add(record["name"].ToString());
        //    }
        //    connection.Close();
        //    return lst;
        //}

        //private bool ExistBD()
        //{
        //    bool exist = File.Exists(dataBaseName);
        //    if (!(exist))
        //    {
        //        SQLiteConnection.CreateFile(dataBaseName);
        //        exist = File.Exists(dataBaseName);
        //    }
        //    return exist;
        }

        private void AddPhrases_Click(object sender, RoutedEventArgs e)
        {
            AddPhrases ap = new AddPhrases();
            ap.ShowDialog();
        }

        private void AddWords_Click(object sender, RoutedEventArgs e)
        {
            //new AddWord().ShowDialog();
        }
    }
}
