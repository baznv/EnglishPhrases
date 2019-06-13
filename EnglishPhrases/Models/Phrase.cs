using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.Models
{
    [Table]
    public class EnglishSentence : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey]
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string sentence;
        [NotNull]
        public string Sentense
        {
            get { return sentence; }
            set
            {
                sentence = value;
                OnPropertyChanged();
            }
        }

        private string pathToSound;
        public string PathToSound
        {
            get { return pathToSound; }
            set
            {
                pathToSound = value;
                OnPropertyChanged();
            }
        }

        public EnglishSentence(string sentense, string pathToSound)
        {
            Sentense = sentense;
            PathToSound = pathToSound;
        }

        public EnglishSentence() { }

        internal static void Save(EnglishSentence currentEnglish)
        {
            DB.DBSqlite.InsertRow(currentEnglish);
        }

        public override string ToString()
        {
            return $"{ID} - {Sentense}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

    [Table]
    public class RussianSentence : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey]
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string sentence;
        [NotNull]
        public string Sentense
        {
            get { return sentence; }
            set
            {
                sentence = value;
                OnPropertyChanged();
            }
        }

        public RussianSentence(string sentense)
        {
            Sentense = sentense;
        }

        public RussianSentence() { }


        internal static void Save(RussianSentence currentRussian)
        {
            DB.DBSqlite.InsertRow(currentRussian);
        }

        public override string ToString()
        {
            return $"{ID} - {Sentense}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

    [Table]
    public class Phrase : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey]
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private int id_englishSentence;
        [ForeignKey(typeof(EnglishSentence)), NotNull]
        public int ID_EnglishSentence
        {
            get { return id_englishSentence; }
            set
            {
                id_englishSentence = value;
                OnPropertyChanged();
            }
        }

        private int id_russianSentence;
        [ForeignKey(typeof(RussianSentence)), NotNull]
        public int ID_RussianSentence
        {
            get { return id_russianSentence; }
            set
            {
                id_russianSentence = value;
                OnPropertyChanged();
            }
        }

        private string dateAdd;
        [NotNull]
        public string DateAdd
        {
            get { return dateAdd; }
            set
            {
                dateAdd = value;
                OnPropertyChanged();
            }
        }

        private int countShow; //количество показов (статистика)
        public int CountShow
        {
            get { return countShow; }
            set
            {
                countShow = value;
                OnPropertyChanged();
            }
        }

        private bool show; //показывать или нет на тренировке 0-false 1-true
        public bool Show
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged();
            }
        }

        public Phrase()
        {
        }

        internal static void Save(Phrase relation)
        {
            DB.DBSqlite.InsertRow(relation);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

    //Для указания атрибутов у класса для данных (создание DB), чтобы вручную не перебирать
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute() { }
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

        public NotNullAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        public Type type { get; private set; }
        public string Text { get; } = "FOREIGN KEY";

        public ForeignKeyAttribute(Type type)
        {
            this.type = type;
        }
    }

}
