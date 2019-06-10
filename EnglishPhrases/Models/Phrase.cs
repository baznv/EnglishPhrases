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
        public string Sentense { get; set; }
        //public ?? Sound {get; set;} 
    }
    [Table]
    public class RussianSentence
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Sentense { get; set; }
    }
    [Table]
    public class Relation
    {
        [PrimaryKey]
        public int ID { get; set; }
        [ForeignKey(typeof(EnglishSentence))]
        public int ID_EnglishSentence { get; set; }
        [ForeignKey(typeof(RussianSentence))]
        public int ID_RussianSentence { get; set; }
        public string DateAdd { get; set; }
        //public int CountShow { get; set; }
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
    public class ForeignKeyAttribute : Attribute
    {
        private Type type;

        public ForeignKeyAttribute(Type type)
        {
            this.type = type;
        }
    }

}
