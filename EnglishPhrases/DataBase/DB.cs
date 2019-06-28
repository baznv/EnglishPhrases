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
        private static string stringConnection;

        public static void Init()
        {
            //string dir = AppDomain.CurrentDomain.BaseDirectory;
            //string fullPathToDB = Path.Combine(dir, App.PathToDB);
            stringConnection = $"Data Source={App.fullPathToDB}; foreign keys=true; Version=3;";

            if (!File.Exists(App.fullPathToDB))
            {
                SQLiteConnection.CreateFile(App.fullPathToDB);
                if (File.Exists(App.fullPathToDB))
                {
                    CreateTables();
                }
                else MessageBox.Show("Возникла ошибка при создании базы данных");
            }
        }

        //public static void Init()
        //{
        //    string dir = AppDomain.CurrentDomain.BaseDirectory;
        //    string fullPathToDB = Path.Combine(dir, App.PathToDB);

        //    string tempFullPathToDB = Path.Combine(dir, App.tempPath);

        //    stringConnection = $"Data Source={fullPathToDB}; foreign keys=true; Version=3;";
        //    string strConn = $"Data Source={tempFullPathToDB}; foreign keys=true; Version=3;";

        //    //List<English> Eng = new List<English>();
        //    //List<Russian> Rus = new List<Russian>();
        //    List<Translate> Trans = new List<Translate>();

        //    using (SQLiteConnection conn = new SQLiteConnection(strConn))
        //    {
        //        conn.Open();

        //        using (SQLiteTransaction transaction = conn.BeginTransaction())
        //        {
        //            SQLiteCommand command = new SQLiteCommand(conn);
        //            command.CommandText = "SELECT * FROM english";
        //            command.Transaction = transaction;

        //            //using (SQLiteDataReader reader = command.ExecuteReader())
        //            //{
        //            //    while (reader.Read())
        //            //    {
        //            //        English phrase = new English();
        //            //        phrase.ID = Convert.ToInt32(reader["id"]);
        //            //        phrase.SentenceE = reader["sentence"].ToString();
        //            //        string tmp = reader["pathtosound"].ToString();
        //            //        if (tmp == "")
        //            //            tmp = null;
        //            //        phrase.PathToSoundE = tmp;
        //            //        Eng.Add(phrase);
        //            //    }
        //            //}

        //            //command.CommandText = "SELECT * FROM russian";
        //            //using (SQLiteDataReader reader = command.ExecuteReader())
        //            //{
        //            //    while (reader.Read())
        //            //    {
        //            //        Russian phrase = new Russian();
        //            //        phrase.ID = Convert.ToInt32(reader["id"]);
        //            //        phrase.SentenceR = reader["sentence"].ToString();
        //            //        Rus.Add(phrase);
        //            //    }
        //            //}

        //            command.CommandText = "SELECT * FROM translate";
        //            using (SQLiteDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Translate phrase = new Translate();
        //                    phrase.ID_English = Convert.ToInt32(reader["id_english"]);
        //                    phrase.ID_Russian = Convert.ToInt32(reader["id_russian"]);
        //                    phrase.DateAdd = reader["dateadd"].ToString();
        //                    Trans.Add(phrase);
        //                }
        //            }


        //            transaction.Commit();
        //        }
        //        conn.Close();

        //    }

        //    //foreach (var t in Eng)
        //    //{
        //    //    InsertRow(t);
        //    //}

        //    //foreach (var t in Rus)
        //    //{
        //    //    InsertRow(t);
        //    //}

        //    foreach (var t in Trans)
        //    {
        //        InsertRow(t);
        //    }
        //}




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
            string titleRequest = $"CREATE TABLE IF NOT EXISTS {type.Name.ToLower()}";
            List<string> columns = new List<string>(); //обычные столбцы таблицы
            List<string> primaryColumns = new List<string>(); //столбцы таблицы с первичным ключом
            string foreignText = ""; //для столбцов таблицы с внешним ключом

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (!(propertyInfo[i].GetCustomAttributes(typeof(PrimaryKeyAttribute), false).SingleOrDefault() == null))
                    primaryColumns.Add(propertyInfo[i].Name.ToLower());
                var notNull = propertyInfo[i].GetCustomAttributes(typeof(NotNullAttribute), false).SingleOrDefault() as NotNullAttribute;
                var unique = propertyInfo[i].GetCustomAttributes(typeof(UniqueAttribute), false).SingleOrDefault() as UniqueAttribute;


                switch (propertyInfo[i].PropertyType.Name)
                {
                    case "String":
                        columns.Add($"{propertyInfo[i].Name.ToLower()} TEXT {unique?.Text} {notNull?.Text}".TrimEnd(' '));
                        break;
                    case "Boolean":
                    case "Int32":
                        columns.Add($"{propertyInfo[i].Name.ToLower()} INTEGER {unique?.Text} {notNull?.Text}".TrimEnd(' '));
                        break;
                }

                var foreign = propertyInfo[i].GetCustomAttributes(typeof(ForeignKeyAttribute), false).SingleOrDefault() as ForeignKeyAttribute;

                //принимаем, что ссылка у foreign key может быть только на столбец id 
                if (!(foreign == null))
                    foreignText += $", {foreign.Text}({propertyInfo[i].Name.ToLower()}) REFERENCES {foreign.TypeRef.Name.ToLower()}(id)";
            }
            string request = $"{titleRequest} ({string.Join(", ", columns.ToArray())}{foreignText}, PRIMARY KEY ({string.Join(", ", primaryColumns.ToArray())}));";

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(request, conn);
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
            comm += $"SELECT last_insert_rowid();";

            int id;

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                conn.Open();

                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = comm;

                for (int i = 0; i < fields.Count; i++)
                {
                    PropertyInfo fi = type.GetProperty(fields[i]);
                    command.Parameters.AddWithValue(fields[i], fi.GetValue(obj));
                }
                var t = command.ExecuteScalar();
                id = int.Parse(t.ToString());
                PropertyInfo fi_id = type.GetProperty("ID");
                fi_id?.SetValue(obj, id);

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
            string comm = $"INSERT INTO {type.Name.ToLower()} (";

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
            Russian russian = new Russian();
            Translate translate = new Translate();

            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(conn);

                command.CommandText = $"SELECT id FROM english WHERE sentenceE = \"{phrase.EnglishPhrase}\"";
                object temp = command.ExecuteScalar();
                if (!(temp == null))
                    english.ID = int.Parse(temp.ToString());

                command.CommandText = $"SELECT id FROM russian WHERE sentenceR = \"{phrase.RussianPhrase}\"";
                temp = command.ExecuteScalar();
                if (!(temp == null))
                    russian.ID = int.Parse(temp.ToString());

                conn.Close();
            }

            if (english.ID == 0)
            {
                english.SentenceE = phrase.EnglishPhrase;
                english.PathToSoundE = phrase.PathToSound;
                english.ShowE = phrase.IsShowEnglish;
                english.CountShowE = phrase.CountShowEnglish;
                english.CountRightE = phrase.CountRightEnglish;
                InsertRow(english);
            }

            if (russian.ID == 0)
            {
                russian.SentenceR = phrase.RussianPhrase;
                russian.ShowR = phrase.IsShowRussian;
                russian.CountShowR = phrase.CountShowRussian;
                russian.CountRightR = phrase.CountRightRussian;
                InsertRow(russian);
            }

            translate.ID_English = english.ID;
            translate.ID_Russian = russian.ID;
            translate.DateAdd = DateTime.Now.ToString("yyyy.MM.dd");
            InsertRow(translate);
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

                        command.CommandText = $"UPDATE english SET sentencee=\"{phrase.EnglishPhrase}\", showe={phrase.IsShowEnglish} WHERE id={phrase.ID[0]};";
                        command.ExecuteNonQuery();

                        command.CommandText = $"UPDATE russian SET sentencer=\"{phrase.RussianPhrase}\", showr={phrase.IsShowRussian} WHERE id={phrase.ID[1]};";
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
                        //string str0 = ""; // $"{reader.GetName(0)} - {reader.GetValue(0)} \n";
                        //string str1 = ""; // $"{reader.GetName(1)} - {reader.GetValue(1)} \n";
                        //string str2 = ""; // $"{reader.GetName(2)} - {reader.GetValue(2)} \n";
                        //string str3 = ""; // $"{reader.GetName(3)} - {reader.GetValue(3)} \n";
                        //string str4 = ""; // $"{reader.GetName(4)} - {reader.GetValue(4)} \n";
                        //string str5 = ""; // $"{reader.GetName(5)} - {reader.GetValue(5)} \n";
                        //string str6 = ""; // $"{reader.GetName(6)} - {reader.GetValue(6)} \n";
                        //string str7 = ""; // $"{reader.GetName(7)} - {reader.GetValue(7)} \n";
                        //string str8 = $"{reader.GetName(8)} - {reader.GetValue(8)} \n";
                        //string str9 = $"{reader.GetName(9)} - {reader.GetValue(9)} \n";
                        //string str10 = $"{reader.GetName(10)} - {reader.GetValue(10)} \n";
                        //string str11 = $"{reader.GetName(11)} - {reader.GetValue(11)} \n";
                        //string str12 = $"{reader.GetName(12)} - {reader.GetValue(12)} \n";
                        //string str13 = $"{reader.GetName(13)} - {reader.GetValue(13)} \n";
                        ////string str14 = $"{reader.GetName(14)} - {reader.GetValue(14)} \n";

                        //MessageBox.Show($"{str0} {str1} {str2} {str3} {str4} {str5} {str6} {str7} {str8} {str9} {str10} {str11} {str12} {str13}");
                        Phrase phrase = new Phrase();
                        phrase.ID = new int[] { Convert.ToInt32(reader["id_english"]), Convert.ToInt32(reader["id_russian"]) };
                        phrase.DateAdd = reader["dateadd"].ToString();
                        phrase.EnglishPhrase = reader["sentencee"].ToString();
                        phrase.PathToSound = reader["pathtosounde"].ToString();
                        phrase.CountShowEnglish = Convert.ToInt32(reader["countshowe"]);
                        phrase.IsShowEnglish = Convert.ToBoolean(Convert.ToInt32(reader["showe"]));
                        phrase.CountRightEnglish = Convert.ToInt32(reader["countrighte"]);
                        phrase.RussianPhrase = reader["sentencer"].ToString();
                        phrase.CountShowRussian = Convert.ToInt32(reader["countshowr"]);
                        phrase.IsShowRussian = Convert.ToBoolean(Convert.ToInt32(reader["showr"]));
                        phrase.CountRightRussian = Convert.ToInt32(reader["countrightr"]);

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
            [Unique, NotNull]
            public string SentenceE { get; set; }
            [Unique]
            public string PathToSoundE { get; set; } = null;
            public int CountShowE { get; set; } //количество показов (статистика)
            public bool ShowE { get; set; } //показывать или нет на тренировке 0-false 1-true
            public int CountRightE { get; set; } //количество правильных ответов (статистика)
        }

        [Table]
        public class Russian
        {
            [PrimaryKey]
            public int ID { get; set; }
            [Unique, NotNull]
            public string SentenceR { get; set; }
            public int CountShowR { get; set; } //количество показов (статистика)
            public bool ShowR { get; set; } //показывать или нет на тренировке 0-false 1-true
            public int CountRightR { get; set; } //количество правильных ответов (статистика)
        }

        [Table]
        public class Translate
        {
            [PrimaryKey, ForeignKey(typeof(English))]
            public int ID_English { get; set; }
            [PrimaryKey, ForeignKey(typeof(Russian))]
            public int ID_Russian { get; set; }
            [NotNull]
            public string DateAdd { get; set; }
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
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class PrimaryKeyAttribute : Attribute
        {
            public string Text { get; } = "PRIMARY KEY";
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class NotNullAttribute : Attribute
        {
            public string Text { get; } = "NOT NULL";
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class UniqueAttribute : Attribute
        {
            public string Text { get; } = "UNIQUE";
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
