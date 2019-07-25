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

        internal static ObservableCollection<Phrase> GetAnalogPhrase(Phrase phrase)
        {
            ObservableCollection<Phrase> result = new ObservableCollection<Phrase>();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                conn.Open();

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.CommandText = $"SELECT * FROM translate WHERE translate.id_english = {phrase.ID[0]} OR translate.id_russian = {phrase.ID[1]}";
                    command.Transaction = transaction;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phrase tempPhrase = new Phrase();
                            tempPhrase.ID = new int[] { Convert.ToInt32(reader["id_english"]), Convert.ToInt32(reader["id_russian"]) };
                            tempPhrase.DateAdd = reader["dateadd"].ToString();
                            result.Add(tempPhrase);
                        }
                    }

                    for (int i = 0; i < result.Count; i++)
                    {
                        command.CommandText = $"SELECT * FROM english WHERE id = {result[i].ID[0]}";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result[i].EnglishPhrase = reader["sentencee"].ToString();
                                result[i].Sound = reader["pathtosounde"].ToString();
                                //result[i].CountShowEnglish = Convert.ToInt32(reader["countshowe"]);
                                //result[i].IsShowEnglish = Convert.ToBoolean(Convert.ToInt32(reader["showsentencestorussian"]));
                                //result[i].CountRightEnglish = Convert.ToInt32(reader["countrighte"]);
                            }
                        }

                        command.CommandText = $"SELECT * FROM russian WHERE id = {result[i].ID[1]}";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result[i].RussianPhrase = reader["sentencer"].ToString();
                                //result[i].CountShowRussian = Convert.ToInt32(reader["countshowr"]);
                                //result[i].IsShowRussian = Convert.ToBoolean(Convert.ToInt32(reader["showr"]));
                                //result[i].CountRightRussian = Convert.ToInt32(reader["countrightr"]);
                            }
                        }
                    }

                    transaction.Commit();
                }
                conn.Close();
            }
            return result;
        }



        internal static Phrase GetRandomPhrase()
        {
            Phrase phrase = new Phrase();
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                //SELECT * FROM (SELECT * FROM translate WHERE (id_english, id_russian) IN (SELECT id_english, id_russian FROM translate ORDER BY RANDOM() LIMIT 1)) INNER JOIN english ON translate.id_english = english.id INNER JOIN russian ON translate.id_russian = russian.id
                command.CommandText = @"SELECT * FROM 
                                        (SELECT * FROM translate WHERE (same_english, same_russian) IN (SELECT same_english, same_russian FROM translate ORDER BY RANDOM() LIMIT 1)) 
                                        AS rand
                                        INNER JOIN english ON rand.same_english = english.same_eng 
                                        INNER JOIN russian ON rand.same_russian = russian.same_ru;";
                conn.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        phrase.ID = new int[] { Convert.ToInt32(reader[nameof(Translate.Same_English).ToString().ToLower()]), Convert.ToInt32(reader[nameof(Translate.Same_Russian).ToString().ToLower()]) };
                        phrase.DateAdd = reader[nameof(Translate.DateAdd).ToString().ToLower()].ToString();
                        phrase.EnglishPhrase = reader[nameof(English.Sentence_eng).ToString().ToLower()].ToString();
                        phrase.Sound = reader[nameof(English.Sound_eng).ToString().ToLower()].ToString();
                        phrase.RussianPhrase = reader[nameof(Russian.Sentence_ru).ToString().ToLower()].ToString();
                    }
                }
                conn.Close();
            }
            return phrase;
        }

        internal static string GetRandomEnglish()
        {
            string englishSentence = "";
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = @"SELECT sentence_eng FROM english ORDER BY RANDOM() LIMIT 1);";
                conn.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        englishSentence = reader[nameof(English.Sentence_eng).ToString().ToLower()].ToString();
                    }
                }
                conn.Close();
            }
            return englishSentence;
        }

        internal static string GetRandomRussian()
        {
            string russianSentence = "";
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = @"SELECT sentence_ru FROM russian ORDER BY RANDOM() LIMIT 1);";
                conn.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        russianSentence = reader[nameof(Russian.Sentence_ru).ToString().ToLower()].ToString();
                    }
                }
                conn.Close();
            }
            return russianSentence;
        }

        internal static string GetRandomSound()
        {
            string sound = "";
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                SQLiteCommand command = new SQLiteCommand(conn);
                command.CommandText = @"SELECT sound_eng FROM english ORDER BY RANDOM() LIMIT 1 WHERE sound_eng NOT NULL;";
                conn.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sound = reader[nameof(English.Sound_eng).ToString().ToLower()].ToString();
                    }
                }
                conn.Close();
            }
            return sound;
        }



        //public static void Init()
        //{
        //    string dir = AppDomain.CurrentDomain.BaseDirectory;
        //    string fullPathToDB = App.fullPathToDB;
        //    string tempFullPathToDB = Path.Combine(dir, App.tempPath);

        //    stringConnection = $"Data Source={fullPathToDB}; foreign keys=true; Version=3;";
        //    string strConn = $"Data Source={tempFullPathToDB}; foreign keys=true; Version=3;";

        //    if (!File.Exists(App.fullPathToDB))
        //    {
        //        SQLiteConnection.CreateFile(App.fullPathToDB);
        //        if (File.Exists(App.fullPathToDB))
        //        {
        //            CreateTables();
        //        }
        //        else MessageBox.Show("Возникла ошибка при создании базы данных");
        //    }

        //    List<Phrase> phr = new List<Phrase>();
        //    using (SQLiteConnection conn = new SQLiteConnection(strConn))
        //    {
        //        conn.Open();
        //        using (SQLiteTransaction transaction = conn.BeginTransaction())
        //        {
        //            SQLiteCommand command = new SQLiteCommand(conn);
        //            command.Transaction = transaction;

        //            command.CommandText = @"SELECT * FROM translate 
        //                                    INNER JOIN english ON translate.id_english = english.id
        //                                    INNER JOIN russian ON translate.id_russian = russian.id";

        //            using (SQLiteDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Phrase phrase = new Phrase();
        //                    phrase.ID = new int[] { Convert.ToInt32(reader["id_english"]), Convert.ToInt32(reader["id_russian"]) };
        //                    phrase.EnglishPhrase = reader["englishphrase"].ToString();
        //                    string tmp = reader["sound"].ToString();
        //                    if (tmp == "")
        //                        tmp = null;
        //                    phrase.Sound = tmp;
        //                    phrase.RussianPhrase = reader["russianphrase"].ToString();
        //                    phrase.DateAdd = reader["dateadd"].ToString();

        //                    phr.Add(phrase);
        //                }
        //            }
        //            transaction.Commit();
        //        }
        //        conn.Close();
        //    }

        //    using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
        //    {
        //        conn.Open();
        //        using (SQLiteTransaction transaction = conn.BeginTransaction())
        //        {
        //            SQLiteCommand command = new SQLiteCommand(conn);
        //            command.Transaction = transaction;

        //            foreach (var p in phr)
        //            {
        //                English e = new English()
        //                {
        //                    Sentence = p.EnglishPhrase,
        //                    Sound = p.Sound
        //                };

        //                Russian r = new Russian()
        //                {
        //                    Sentence = p.RussianPhrase
        //                };

        //                command.CommandText = $"SELECT same FROM russian WHERE sentence = \"{r.Sentence}\"";
        //                e.Same = Convert.ToInt32(command.ExecuteScalar());

        //                command.CommandText = $"SELECT same FROM english WHERE sentence = \"{e.Sentence}\"";
        //                r.Same = Convert.ToInt32(command.ExecuteScalar());


        //                if (e.Same > 0 && r.Same == 0)
        //                {
        //                    command.CommandText = $"SELECT same_russian FROM translate WHERE same_english == {e.Same}";
        //                    r.Same = Convert.ToInt32(command.ExecuteScalar());
        //                }

        //                if (e.Same == 0 && r.Same > 0)
        //                {
        //                    command.CommandText = $"SELECT same_english FROM translate WHERE same_russian == {r.Same}";
        //                    e.Same = Convert.ToInt32(command.ExecuteScalar());
        //                }


        //                if (e.Same == 0)
        //                {
        //                    command.CommandText = $"SELECT same FROM english ORDER BY same DESC LIMIT 1";
        //                    int lastSame = Convert.ToInt32(command.ExecuteScalar());
        //                    e.Same = lastSame + 1;
        //                }

        //                if (r.Same == 0)
        //                {
        //                    command.CommandText = $"SELECT same FROM russian ORDER BY same DESC LIMIT 1";
        //                    int lastSame = Convert.ToInt32(command.ExecuteScalar());
        //                    r.Same = lastSame + 1;
        //                }

        //                if (e.Sound == null || e.Sound == "")
        //                {
        //                    command.CommandText = $"INSERT OR IGNORE INTO english (sentence, sound, same) VALUES (\"{e.Sentence}\", null, {e.Same})";
        //                    command.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    command.CommandText = $"INSERT OR IGNORE INTO english (sentence, sound, same) VALUES (\"{e.Sentence}\", \"{e.Sound}\", {e.Same})";
        //                    command.ExecuteNonQuery();
        //                }

        //                command.CommandText = $"INSERT OR IGNORE INTO russian (sentence, same) VALUES (\"{r.Sentence}\", {r.Same})";
        //                command.ExecuteNonQuery();

        //                command.CommandText = $"INSERT OR IGNORE INTO translate (same_english, same_russian, dateadd) VALUES ({e.Same}, {r.Same}, \"{p.DateAdd}\")";
        //                command.ExecuteNonQuery();
        //            }
        //            transaction.Commit();
        //        }
        //        conn.Close();
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
                try
                {
                    var t = command.ExecuteScalar();
                    id = int.Parse(t.ToString());
                    PropertyInfo fi_id = type.GetProperty("ID");
                    fi_id?.SetValue(obj, id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

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


        public static void SaveToDB(Phrase p)
        {
            using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    SQLiteCommand command = new SQLiteCommand(conn);
                    command.Transaction = transaction;

                        English e = new English()
                        {
                            Sentence_eng = p.EnglishPhrase,
                            Sound_eng = p.Sound
                        };

                        Russian r = new Russian()
                        {
                            Sentence_ru = p.RussianPhrase
                        };

                        command.CommandText = $"SELECT same_ru FROM russian WHERE sentence_ru = \"{r.Sentence_ru}\"";
                        e.Same_eng = Convert.ToInt32(command.ExecuteScalar());

                        command.CommandText = $"SELECT same_eng FROM english WHERE sentence_eng = \"{e.Sentence_eng}\"";
                        r.Same_ru = Convert.ToInt32(command.ExecuteScalar());


                    if (e.Same_eng > 0 && r.Same_ru == 0)
                    {
                        command.CommandText = $"SELECT same_russian FROM translate WHERE same_english == {e.Same_eng}";
                        r.Same_ru = Convert.ToInt32(command.ExecuteScalar());
                    }

                    if (e.Same_eng == 0 && r.Same_ru > 0)
                    {
                        command.CommandText = $"SELECT same_english FROM translate WHERE same_russian == {r.Same_ru}";
                        e.Same_eng = Convert.ToInt32(command.ExecuteScalar());
                    }


                    if (e.Same_eng == 0)
                    {
                        command.CommandText = $"SELECT same_eng FROM english ORDER BY same_eng DESC LIMIT 1";
                        int lastSame = Convert.ToInt32(command.ExecuteScalar());
                        e.Same_eng = lastSame + 1;
                    }

                    if (r.Same_ru == 0)
                    {
                        command.CommandText = $"SELECT same_ru FROM russian ORDER BY same_ru DESC LIMIT 1";
                        int lastSame = Convert.ToInt32(command.ExecuteScalar());
                        r.Same_ru = lastSame + 1;
                    }

                    if (e.Sound_eng == null || e.Sound_eng == "")
                    {
                        command.CommandText = $"INSERT OR IGNORE INTO english (sentence_eng, sound_eng, same_eng) VALUES (\"{e.Sentence_eng}\", null, {e.Same_eng})";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = $"INSERT OR IGNORE INTO english (sentence_eng, sound_eng, same_eng) VALUES (\"{e.Sentence_eng}\", \"{e.Sound_eng}\", {e.Same_eng})";
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = $"INSERT OR IGNORE INTO russian (sentence_ru, same_ru) VALUES (\"{r.Sentence_ru}\", {r.Same_ru})";
                    command.ExecuteNonQuery();

                    Translate t = new Translate()
                    {
                        Same_English = e.Same_eng,
                        Same_Russian = r.Same_ru,
                        DateAdd = DateTime.Now.ToString("yyyy.MM.dd")
                    };

                    command.CommandText = $"INSERT OR IGNORE INTO translate (same_english, same_russian, dateadd) VALUES ({t.Same_English}, {t.Same_Russian}, \"{t.DateAdd}\")";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                conn.Close();
            }

            //English english = new English();
            //Russian russian = new Russian();

            ////List<int> listAnalogEnglish = new List<int>();
            ////List<int> listAnalogRussian = new List<int>();

            //using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            //{
            //    conn.Open();
            //    using (SQLiteTransaction transaction = conn.BeginTransaction())
            //    {
            //        using (SQLiteCommand command = new SQLiteCommand(conn))
            //        {
            //            command.Transaction = transaction;
            //            command.CommandText = $"SELECT id FROM english WHERE {nameof(English.Sentence).ToString().ToLower()} = \"{phrase.EnglishPhrase}\"";
            //            object temp = command.ExecuteScalar();
            //            if (!(temp == null))
            //                english.ID = int.Parse(temp.ToString());

            //            command.CommandText = $"SELECT id FROM russian WHERE {nameof(Russian.Sentence).ToString().ToLower()} = \"{phrase.RussianPhrase}\"";
            //            temp = command.ExecuteScalar();
            //            if (!(temp == null))
            //                russian.ID = int.Parse(temp.ToString());
            //        }
            //        transaction.Commit();
            //    }
            //    conn.Close();
            //}

            //if (english.ID == 0)
            //{
            //    english.Sentence = phrase.EnglishPhrase;
            //    english.Sound = phrase.Sound;
            //    InsertRow(english);
            //}

            //if (russian.ID == 0)
            //{
            //    russian.Sentence = phrase.RussianPhrase;
            //    InsertRow(russian);
            //}

            ////using (SQLiteConnection conn = new SQLiteConnection(stringConnection))
            ////{
            ////    conn.Open();
            ////    using (SQLiteTransaction transaction = conn.BeginTransaction())
            ////    {
            ////        using (SQLiteCommand command = new SQLiteCommand(conn))
            ////        {
            ////            command.Transaction = transaction;
            ////            command.CommandText = $"SELECT id_english FROM translate WHERE id_russian IN (SELECT id_russian FROM translate WHERE {nameof(Translate.ID_English).ToString().ToLower()} = \"{english.ID}\")";
            ////            using (SQLiteDataReader reader = command.ExecuteReader())
            ////            {
            ////                while (reader.Read())
            ////                {
            ////                    int id = Convert.ToInt32(reader[nameof(Translate.ID_English).ToString().ToLower()]);
            ////                    listAnalogEnglish.Add(id);
            ////                }
            ////            }

            ////            command.CommandText = $"SELECT id_russian FROM translate WHERE id_english IN (SELECT id_english FROM translate WHERE {nameof(Translate.ID_Russian).ToString().ToLower()} = \"{russian.ID}\")";
            ////            using (SQLiteDataReader reader = command.ExecuteReader())
            ////            {
            ////                while (reader.Read())
            ////                {
            ////                    int id = Convert.ToInt32(reader[nameof(Translate.ID_Russian).ToString().ToLower()]);
            ////                    listAnalogRussian.Add(id);
            ////                }
            ////            }
            ////        }
            ////        transaction.Commit();
            ////    }
            ////    conn.Close();
            ////}

            //Translate trans = new Translate();
            //trans.Same_English = english.Same;
            //trans.Same_Russian = russian.Same;
            //trans.DateAdd = DateTime.Now.ToString("yyyy.MM.dd");
            //try
            //{
            //    InsertRow(trans);
            //}
            //catch
            //{
            //    MessageBox.Show($"Row same_eng={trans.Same_English} - same_rus={trans.Same_Russian}");
            //}


            ////foreach (var item in listAnalogRussian)
            ////{
            ////    Translate translate = new Translate();
            ////    translate.ID_English = english.ID;
            ////    translate.ID_Russian = item;
            ////    translate.DateAdd = DateTime.Now.ToString("yyyy.MM.dd");
            ////    try
            ////    {
            ////        InsertRow(translate);
            ////    }
            ////    catch
            ////    {
            ////        MessageBox.Show($"Row id_eng={translate.ID_English} - id_rus={translate.ID_Russian}");
            ////    }
            ////}

            ////foreach (var item in listAnalogEnglish)
            ////{
            ////    Translate translate = new Translate();
            ////    translate.ID_English = item;
            ////    translate.ID_Russian = russian.ID;
            ////    translate.DateAdd = DateTime.Now.ToString("yyyy.MM.dd");
            ////    try
            ////    {
            ////        InsertRow(translate);
            ////    }
            ////    catch
            ////    {
            ////        MessageBox.Show($"Row id_eng={translate.ID_English} - id_rus={translate.ID_Russian}");
            ////    }
            ////}
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

                        //command.CommandText = $"UPDATE english SET sentencee=\"{phrase.EnglishPhrase}\", showe={phrase.IsShowEnglish} WHERE id={phrase.ID[0]};";
                        //command.ExecuteNonQuery();

                        //command.CommandText = $"UPDATE russian SET sentencer=\"{phrase.RussianPhrase}\", showr={phrase.IsShowRussian} WHERE id={phrase.ID[1]};";
                        //command.ExecuteNonQuery();

                        command.CommandText = $"UPDATE english SET sentencee=\"{phrase.EnglishPhrase}\" WHERE id={phrase.ID[0]};";
                        command.ExecuteNonQuery();

                        command.CommandText = $"UPDATE russian SET sentencer=\"{phrase.RussianPhrase}\" WHERE id={phrase.ID[1]};";
                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }

                conn.Close();
            }
        }



        public static ObservableCollection<Phrase> GetAllPhrases()
        {
            string comm = "SELECT * FROM translate INNER JOIN english ON translate.same_english = english.same_eng INNER JOIN russian ON translate.same_russian = russian.same_ru";

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
                        phrase.ID = new int[] { Convert.ToInt32(reader[nameof(Translate.Same_English).ToString().ToLower()]), Convert.ToInt32(reader[nameof(Translate.Same_Russian).ToString().ToLower()]) };
                        phrase.DateAdd = reader[nameof(Translate.DateAdd).ToString().ToLower()].ToString();
                        phrase.EnglishPhrase = reader[nameof(English.Sentence_eng).ToString().ToLower()].ToString();
                        phrase.Sound = reader[nameof(English.Sound_eng).ToString().ToLower()].ToString();
                        //phrase.CountShowEnglish = Convert.ToInt32(reader[nameof(English.ShowSentenceToRussian).ToString().ToLower()]);
                        //phrase.IsShowEnglish = Convert.ToBoolean(Convert.ToInt32(reader[nameof(English.IsShowSentenceToRussianE).ToString().ToLower()]));
                        //phrase.CountRightEnglish = Convert.ToInt32(reader[nameof(English.RightSentenceToRussianE).ToString().ToLower()]);
                        phrase.RussianPhrase = reader[nameof(Russian.Sentence_ru).ToString().ToLower()].ToString();
                        //phrase.CountShowRussian = Convert.ToInt32(reader[nameof(Russian.ShowSentenceToEnglish).ToString().ToLower()]);
                        //phrase.IsShowRussian = Convert.ToBoolean(Convert.ToInt32(reader[nameof(Russian.IsShowSentenceToEnglishR).ToString().ToLower()]));
                        //phrase.CountRightRussian = Convert.ToInt32(reader[nameof(Russian.RightSentenceToEnglish).ToString().ToLower()]);

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
            public int ID_eng { get; set; }
            [Unique, NotNull]
            public string Sentence_eng { get; set; }
            [Unique]
            public string Sound_eng { get; set; } = null;
            [NotNull]
            public int Same_eng { get; set; }

            //public int ShowSentenceToRussian { get; set; } //количество показов предложения (статистика)
            //public int ShowSoundToRussianE { get; set; } //количество показов озвучка (статистика)
            //public int ShowSoundToEnglishE { get; set; } //количество показов озвучка (статистика)
            //public int RightSentenceToRussianE { get; set; } //количество правильных ответов (статистика)
            //public int RightSoundToRussianE { get; set; } //количество правильных ответов (статистика)
            //public int RightSoundToEnglishE { get; set; } //количество правильных ответов (статистика)
            //public bool IsShowSentenceToRussianE { get; set; } //показывать или нет на тренировке 0-false 1-true
            //public bool IsShowSoundToRussianE { get; set; } //показывать или нет на тренировке 0-false 1-true
            //public bool IsShowSoundToEnglishE { get; set; } //показывать или нет на тренировке 0-false 1-true
        }

        [Table]
        public class Russian
        {
            [PrimaryKey]
            public int ID_ru { get; set; }
            [Unique, NotNull]
            public string Sentence_ru { get; set; }
            [NotNull]
            public int Same_ru { get; set; }

            //public int ShowSentenceToEnglish { get; set; } //количество показов (статистика)
            //public bool IsShowSentenceToEnglishR { get; set; } //показывать или нет на тренировке 0-false 1-true
            //public int RightSentenceToEnglish { get; set; } //количество правильных ответов (статистика)
        }

        [Table]
        public class Translate
        {
            //[PrimaryKey]
            //public int ID { get; set; }
            //[PrimaryKey, ForeignKey(typeof(English))]
            [PrimaryKey]
            public int Same_English { get; set; }
            //[PrimaryKey, ForeignKey(typeof(Russian))]
            [PrimaryKey]
            public int Same_Russian { get; set; }
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
