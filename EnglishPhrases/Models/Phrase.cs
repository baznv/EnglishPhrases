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
        public bool Show { get; set; } //показывать или нет на тренировке

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
        public PrimaryKeyAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : Attribute
    {
        public NotNullAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        private Type type;

        public ForeignKeyAttribute(Type type)
        {
            this.type = type;
        }
    }

}
