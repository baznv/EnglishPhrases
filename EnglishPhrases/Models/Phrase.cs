using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.Models
{
    public class Phrase
    {

        public void AddPhrase()
        {

        }
    }

    [Table]
    public class EnglishSentence
    {
        [PrimaryKey]
        public int ID { get; set; }
        [NotNull]
        public string Sentense { get; set; }
        public string PathToSound {get; set;}

        public EnglishSentence(string sentense, string pathToSound)
        {
            Sentense = sentense;
            PathToSound = pathToSound;
        }

        internal static void Save(EnglishSentence currentEnglish)
        {
            throw new NotImplementedException();
        }
    }
    [Table]
    public class RussianSentence
    {
        [PrimaryKey]
        public int ID { get; set; }
        [NotNull]
        public string Sentense { get; set; }

        public RussianSentence(string sentense)
        {
            Sentense = sentense;
        }

        internal static void Save(RussianSentence currentRussian)
        {
            throw new NotImplementedException();
        }
    }
    [Table]
    public class Relation
    {
        [PrimaryKey]
        public int ID { get; set; }
        [ForeignKey(typeof(EnglishSentence)), NotNull]
        public int ID_EnglishSentence { get; set; }
        [ForeignKey(typeof(RussianSentence)), NotNull]
        public int ID_RussianSentence { get; set; }
        [NotNull]
        public string DateAdd { get; set; }
        public int CountShow { get; set; } //количество показов (статистика)
        public bool Show { get; set; } //показывать или нет на тренировке 0-false 1-true

        internal static void Save(Relation relation)
        {
            throw new NotImplementedException();
        }

        public Relation()
        {
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
