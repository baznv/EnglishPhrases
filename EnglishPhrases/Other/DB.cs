using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EnglishPhrases.Other
{
    public class DB
    {
        private static string fullPathToDB;
        private static string stringConnection;

        public static void Init()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            fullPathToDB = Path.Combine(dir, App.PathToDB);
            if (!File.Exists(fullPathToDB))
            {
                SQLiteConnection.CreateFile(fullPathToDB);
                if (File.Exists(fullPathToDB))
                {
                    CreateTables();
                }
                else MessageBox.Show("Возникла ошибка при создании базы данных");
            }
            stringConnection = $"Data Source={fullPathToDB}; foreign keys=true; Version=3;";
        }

        private static void CreateTables()
        {
            Assembly asmbly = Assembly.GetExecutingAssembly();
            List<Type> typeList = asmbly.GetTypes().Where(t => t.GetCustomAttributes(typeof(Models.TableAttribute), false).Length > 0).ToList();
            foreach (var temp in typeList)
            {
                CreateTable(temp);
            }
        }

        private static void CreateTable(Type type)
        {
            PropertyInfo[] propertyInfo = type.GetProperties();
            string titleRequest = $"CREATE TABLE IF NOT EXISTS {type.Name} ";
            string request = "(";
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                //CREATE TABLE books(
                //            Id INTEGER PRIMARY KEY,
                //            title TEXT NOT NULL,
                //            count_page INTEGER NOT NULL,
                //            price REAL,
                //            auth_id INTEGER NOT NULL,
                //            FOREIGN KEY(auth_id) REFERENCES auth(id)
                //            );

                var primary = propertyInfo[i].GetCustomAttributes(typeof(Models.PrimaryKeyAttribute), false);// (propertyInfo[i]); // .CustomAttributes.Where(x => x is (CustomAttributeData)Models.PrimaryKeyAttribute).ToList();
                //var notNull = 
                if (propertyInfo[i].Name == "ID")
                    request += "id INTEGER PRIMARY KEY";
                else
                {
                    switch (propertyInfo[i].PropertyType.Name)
                    {
                        case "String":
                            request += $", {propertyInfo[i].Name.ToLower()} TEXT";
                            break;
                        case "Int32":
                            request += $", {propertyInfo[i].Name.ToLower()} INTEGER";
                            break;
                        default:
                            request += $", {propertyInfo[i].Name.ToLower()} TEXT";
                            break;
                    }
                }
            }
            request += ");";

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={fullPathToDB}; Version=3;"))
            {
                SQLiteCommand command = new SQLiteCommand(titleRequest + request, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }


    }
}
