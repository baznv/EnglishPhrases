using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.Models
{
    public class Phrase : INotifyPropertyChanged
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string englishPhrase;
        public string EnglishPhrase
        {
            get { return englishPhrase; }
            set
            {
                englishPhrase = value;
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

        private string russianPhrase;
        public string RussianPhrase
        {
            get { return russianPhrase; }
            set
            {
                russianPhrase = value;
                OnPropertyChanged();
            }
        }

        private string dateAdd;
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


        public void Save()
        {
            DataBase.DB.SaveToDB(this);
        }

        public static ObservableCollection<Phrase> GetAllPhrases()
        {
            return DataBase.DB.GetAllPhrases();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
