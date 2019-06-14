using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.Models
{
    public class Phrase : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string EnglishPhrase { get; set; }
        public string PathToSound { get; set; }
        public string RussianPhrase { get; set; }
        public string DateAdd { get; set; }
        public int CountShow { get; set; } //количество показов (статистика)
        public bool Show { get; set; } //показывать или нет на тренировке 0-false 1-true

        public void Save(Phrase phrase)
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
