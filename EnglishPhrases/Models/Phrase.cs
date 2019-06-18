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
    public class Phrase : INotifyPropertyChanged, IEditableObject
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

        private int countRightAnswer; //количество правильных ответов (статистика)
        public int CountRightAnswer
        {
            get { return countRightAnswer; }
            set
            {
                countRightAnswer = value;
                OnPropertyChanged();
            }
        }

        //private string percentRight;
        public string PercentRight
        {
            get
            {
                if (CountShow != 0)
                {
                    int t = (CountRightAnswer / CountShow) * 100;
                    return $"{t}%";
                }
                return null;
            }
        }


        private bool isShow; //показывать или нет на тренировке 0-false 1-true
        public bool IsShow
        {
            get { return isShow; }
            set
            {
                isShow = value;
                OnPropertyChanged();
            }
        }


        public void Save()
        {
            DataBase.DB.SaveToDB(this);
        }

        public void Update()
        {
            DataBase.DB.UpdateInDB(this);
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

        public void BeginEdit()
        {
            return;
        }

        public void EndEdit()
        {
            this.Update();
        }

        public void CancelEdit()
        {
            return;
        }
    }
}
