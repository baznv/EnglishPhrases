﻿using System;
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
            string titleRequest = $"CREATE TABLE IF NOT EXISTS {type.Name.ToLower()} ";
            string request = "(";
            string foreignText = "";

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                var primary = propertyInfo[i].GetCustomAttributes(typeof(Models.PrimaryKeyAttribute), false).SingleOrDefault() as Models.PrimaryKeyAttribute;
                var notNull = propertyInfo[i].GetCustomAttributes(typeof(Models.NotNullAttribute), false).SingleOrDefault() as Models.NotNullAttribute;

                //будет или primaryKey или notNull
                string constraint = "";
                if (!(primary == null))
                    constraint = primary.Text;
                else if (!(notNull == null))
                    constraint = notNull.Text;

                switch (propertyInfo[i].PropertyType.Name)
                {
                    case "String":
                        request += request.Length == 1 ? "" : ", ";
                        request += $"{propertyInfo[i].Name.ToLower()} TEXT {constraint}";
                        break;
                    case "Boolean":
                    case "Int32":
                        request += request.Length == 1 ? "" : ", ";
                        request += $"{propertyInfo[i].Name.ToLower()} INTEGER {constraint}";
                        break;
                    //default:
                    //    request += $", {propertyInfo[i].Name.ToLower()} TEXT {constraint}";
                    //    break;
                }

                var foreign = propertyInfo[i].GetCustomAttributes(typeof(Models.ForeignKeyAttribute), false).SingleOrDefault() as Models.ForeignKeyAttribute;

                if (!(foreign == null))
                    foreignText += $", {foreign.Text}({propertyInfo[i].Name.ToLower()}) REFERENCES {foreign.type.Name.ToLower()}(id)";
            }
            request += foreignText;
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
