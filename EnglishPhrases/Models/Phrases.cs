using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases
{
        [Data]
        public class EnglishWords
        {
            public int ID { get; set; }
            public string Word { get; set; }
            public string Transcription { get; set; }
        }
        [Data]
        public class EnglishSentence
        {
            public int ID { get; set; }
            //public EnglishWords[] words { get; private set; }
            public string Sentense { get; set; }
            //private string sentense;
            //public string Sentense
            //{
            //    get {
            //        return string.Join(" ", words.Select(x => x.Word));
            //    }
            //    set {
            //        sentense = value;
            //    }
            //}
        }
        [Data]
        public class RussianSentence
        {
            public int ID { get; set; }
            public string Sentense { get; set; }
        }
        [Data]
        public class Relation
        {
            public int ID { get; set; }
            public int ID_EnglishSentence { get; set; }
            public int ID_RussianSentence { get; set; }

        }

    [AttributeUsage(AttributeTargets.Class)]
    class DataAttribute : Attribute
    {
        //Для указания атрибутов у класса для данных (создание DB), чтобы вручную не перебирать
        public DataAttribute()
        { }
    }
}
