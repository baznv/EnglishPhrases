using EnglishPhrases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EnglishPhrases.DataBase
{
    public class DB
    {
        //private static string fullPathToDB;
        private static string stringConnection;

        public static void Init()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullPathToDB = Path.Combine(dir, App.PathToDB);
            stringConnection = $"Data Source={fullPathToDB}; foreign keys=true; Version=3;";

            if (!File.Exists(fullPathToDB))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPathToDB));
                SQLiteConnection.CreateFile(fullPathToDB);
                if (File.Exists(fullPathToDB))
                {
                    CreateTables();
                }
                else MessageBox.Show("Возникла ошибка при создании базы данных");
            }
        }

        private static void CreateTables()
        {
            Assembly asmbly = Assembly.GetExecutingAssembly();
            List<Type> typeList = asmbly.GetTypes().Where(t => t.GetCustomAttributes(typeof(TableAttribute), false).Length > 0).ToList();
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
                var primary = propertyInfo[i].GetCustomAttributes(typeof(PrimaryKeyAttribute), false).SingleOrDefault() as PrimaryKeyAttribute;
                var notNull = propertyInfo[i].GetCustomAttributes(typeof(NotNullAttribute), false).SingleOrDefault() as NotNullAttribute;

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

                var foreign = propertyInfo[i].GetCustomAttributes(typeof(ForeignKeyAttribute), false).SingleOrDefault() as ForeignKeyAttribute;

                if (!(foreign == null))
                    foreignText += $", {foreign.Text}({propertyInfo[i].Name.ToLower()}) REFERENCES {foreign.TypeRef.Name.ToLower()}(id)";
            }
            request += foreignText;
            request += ");";

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(titleRequest + request, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void InsertRow<T>(T obj)
        {
            Type type = typeof(T);
            List<string> fields = GetNameProperties(type);
            string comm = GetRowINSERT<T>(type, fields);
            comm += "SELECT last_insert_rowid();";

            int id;

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = comm;

                for (int i = 0; i < fields.Count; i++)
                {
                    PropertyInfo fi = type.GetProperty(fields[i]);
                    command.Parameters.AddWithValue(fields[i], fi.GetValue(obj));
                }

                conn.Open();
                //command.ExecuteNonQuery();
                id = int.Parse(command.ExecuteScalar().ToString());
                PropertyInfo fi_id = type.GetProperty("ID");
                fi_id.SetValue(obj, id);

                conn.Close();
            }
        }

        private static List<string> GetNameProperties(Type type)
        {
            List<string> fields = new List<string>();
            PropertyInfo[] propertyInfo = type.GetProperties();
            foreach (var prop in propertyInfo)
            {
                if (prop.Name == "ID") continue;
                else fields.Add(prop.Name);
            }
            return fields;
        }

        private static string GetRowINSERT<T>(Type type, List<string> fields)
        {
            string comm = $"INSERT INTO {type.Name} (";

            for (int i = 0; i < fields.Count; i++)
            {
                if (i != 0)
                    comm += ", ";
                comm += $"{fields[i]}";
            }

            comm += ") VALUES (";

            for (int i = 0; i < fields.Count; i++)
            {
                if (i != 0)
                    comm += ", ";
                comm += $"@{fields[i]}";
            }

            comm += ");";
            return comm;
        }




        public static void SaveToDB(Phrase phrase)
        {
            English english = new English();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = $"SELECT id FROM english WHERE sentence = \"{phrase.EnglishPhrase}\"";
                conn.Open();
                object temp = command.ExecuteScalar();
                if (!(temp == null))
                    english.ID = int.Parse(temp.ToString());
                //using (SQLiteDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        english.ID = int.Parse(reader["id"].ToString());
                //        english.Sentence = reader["sentence"].ToString();
                //        english.PathToSound = reader["pathToSound"].ToString();
                //    }
                //}
                conn.Close();
            }

            Russian russian = new Russian();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = $"SELECT id FROM russian WHERE sentence = \"{phrase.RussianPhrase}\"";
                conn.Open();
                object temp = command.ExecuteScalar();
                if (!(temp == null))
                    russian.ID = int.Parse(temp.ToString());

                //using (SQLiteDataReader reader = command.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        russian.ID = int.Parse(reader["id"].ToString());
                //        russian.Sentence = reader["sentence"].ToString();
                //    }
                //}
                conn.Close();
            }

            if (english.ID == 0)
            {
                english.Sentence = phrase.EnglishPhrase;
                english.PathToSound = phrase.PathToSound;
                InsertRow(english);
            }

            if (russian.ID == 0)
            {
                russian.Sentence = phrase.RussianPhrase;
                InsertRow(russian);
            }

            Translate swt = new Translate();

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = $"SELECT id FROM translate WHERE id_english = {english.ID} AND id_russian = {russian.ID}";
                conn.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        swt.ID = int.Parse(reader["id"].ToString());
                    }
                }
                conn.Close();
            }

            if (swt.ID == 0)
            {
                swt.ID_English = english.ID;
                swt.ID_Russian = russian.ID;
                swt.DateAdd = DateTime.Now.ToString("yyyy.MM.dd");
                swt.Show = true;
                swt.CountShow = 0;
            }
            DB.InsertRow(swt);
        }
















        internal static ObservableCollection<Models.Setting> GetAllSettings()
        {
            string comm = "SELECT * FROM setting";

            ObservableCollection<Models.Setting> result = new ObservableCollection<Models.Setting>();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = comm;

                conn.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Models.Setting setting = new Models.Setting();
                        setting.ID = reader.GetInt32(0);
                        setting.Name = reader.GetString(1);
                        setting.ValueSetting = reader.GetString(2);

                        result.Add(setting);
                    }
                }
                conn.Close();
            }
            return result;
        }


        internal static void UpdateInDB(Phrase phrase)
        {
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        command.Transaction = transaction;
                        int id_english = -1;
                        int id_russian = -1;
                        command.CommandText = $"SELECT * FROM translate WHERE id = \"{phrase.ID}\"";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id_english = int.Parse(reader["id_english"].ToString());
                                id_russian = int.Parse(reader["id_russian"].ToString());
                            }
                        }

                        command.CommandText = $"UPDATE translate SET show={phrase.IsShow} WHERE id={phrase.ID};";
                        command.ExecuteNonQuery();

                        command.CommandText = $"UPDATE english SET sentence=\"{phrase.EnglishPhrase}\" WHERE id={id_english};";
                        command.ExecuteNonQuery();

                        command.CommandText = $"UPDATE russian SET sentence=\"{phrase.RussianPhrase}\" WHERE id={id_russian};";
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }

                conn.Close();
            }
        }






        public static ObservableCollection<Phrase> GetAllPhrases()
        {
            string comm = "SELECT * FROM translate INNER JOIN english ON translate.id_english = english.id INNER JOIN russian ON translate.id_russian = russian.id";

            ObservableCollection<Phrase> result = new ObservableCollection<Phrase>();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {

                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = comm;

                conn.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Phrase phrase = new Phrase();
                        phrase.ID = reader.GetInt32(0);
                        phrase.DateAdd = reader.GetString(3);
                        phrase.CountShow = reader.GetInt32(4);
                        phrase.IsShow = reader.GetBoolean(5);
                        phrase.EnglishPhrase = reader.GetString(7);
                        phrase.PathToSound = reader[8].ToString();
                        phrase.RussianPhrase = reader.GetString(10);

                        result.Add(phrase);
                    }
                }

                conn.Close();
            }
            return result;
        }



        [Table]
        public class English
        {
            [PrimaryKey]
            public int ID { get; set; }
            [NotNull]
            public string Sentence { get; set; }
            public string PathToSound { get; set; }
        }

        [Table]
        public class Russian
        {
            [PrimaryKey]
            public int ID { get; set; }
            [NotNull]
            public string Sentence { get; set; }
        }

        [Table]
        public class Translate
        {
            [PrimaryKey]
            public int ID { get; set; }
            [ForeignKey(typeof(English)), NotNull]
            public int ID_English { get; set; }
            [ForeignKey(typeof(Russian)), NotNull]
            public int ID_Russian { get; set; }
            [NotNull]
            public string DateAdd { get; set; }
            public int CountShow { get; set; } //количество показов (статистика)
            public bool Show { get; set; } //показывать или нет на тренировке 0-false 1-true
        }

        [Table]
        public class Setting
        {
            [PrimaryKey]
            public int ID { get; set; }
            [NotNull]
            public string Name { get; set; }
            [NotNull]
            public string Value { get; set; }

        }

        //Для указания атрибутов у класса для данных (создание DB), чтобы вручную не перебирать
        [AttributeUsage(AttributeTargets.Class)]
        public class TableAttribute : Attribute
        {
            //public TableAttribute() { }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class PrimaryKeyAttribute : Attribute
        {
            public string Text { get; } = "PRIMARY KEY";
            //public PrimaryKeyAttribute() { }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class NotNullAttribute : Attribute
        {
            public string Text { get; } = "NOT NULL";

            //public NotNullAttribute() { }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class ForeignKeyAttribute : Attribute
        {
            public Type TypeRef { get; private set; }
            public string Text { get; } = "FOREIGN KEY";

            public ForeignKeyAttribute(Type type)
            {
                this.TypeRef = type;
            }
        }

    }
}
