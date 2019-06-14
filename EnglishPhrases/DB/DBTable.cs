using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.DB
{
    public class DBTable
    {
        [Table]
        public class EnglishSentences 
        {
            [PrimaryKey]
            public int ID { get; set; }
            [NotNull]
            public string Sentense { get; set; }
            public string PathToSound { get; set; }
        }

        [Table]
        public class RussianSentences
        {
            [PrimaryKey]
            public int ID { get; set; }
            [NotNull]
            public string Sentense { get; set; }
        }

        [Table]
        public class SentencesWithTranslate
        {
            [PrimaryKey]
            public int ID { get; set; }
            [ForeignKey(typeof(EnglishSentences)), NotNull]
            public int ID_EnglishSentence { get; set; }
            [ForeignKey(typeof(RussianSentences)), NotNull]
            public int ID_RussianSentence { get; set; }
            [NotNull]
            public string DateAdd { get; set; }
            public int CountShow { get; set; } //количество показов (статистика)
            public bool Show { get; set; } //показывать или нет на тренировке 0-false 1-true
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
