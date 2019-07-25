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
        private int[] id;
        public int[] ID
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

        private string sound;
        public string Sound
        {
            get { return sound; }
            set
            {
                sound = value;
                OnPropertyChanged();
            }
        }

        //private int countShowEnglish; //количество показов (статистика)
        //public int CountShowEnglish
        //{
        //    get { return countShowEnglish; }
        //    set
        //    {
        //        countShowEnglish = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int countRightEnglish; //количество правильных ответов (статистика)
        //public int CountRightEnglish
        //{
        //    get { return countRightEnglish; }
        //    set
        //    {
        //        countRightEnglish = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public string PercentRightEnglish
        //{
        //    get
        //    {
        //        if (CountShowEnglish != 0)
        //        {
        //            int t = (CountRightEnglish / CountShowEnglish) * 100;
        //            return $"{t}%";
        //        }
        //        return null;
        //    }
        //}

        //private bool isShowEnglish = true; //показывать или нет на тренировке 0-false 1-true
        //public bool IsShowEnglish
        //{
        //    get { return isShowEnglish; }
        //    set
        //    {
        //        isShowEnglish = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        //private int countShowRussian; //количество показов (статистика)

        //public int CountShowRussian
        //{
        //    get { return countShowRussian; }
        //    set
        //    {
        //        countShowRussian = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int countRightRussian; //количество правильных ответов (статистика)
        //public int CountRightRussian
        //{
        //    get { return countRightRussian; }
        //    set
        //    {
        //        countRightRussian = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public string PercentRightRussian
        //{
        //    get
        //    {
        //        if (CountShowRussian != 0)
        //        {
        //            int t = (CountRightRussian / CountShowRussian) * 100;
        //            return $"{t}%";
        //        }
        //        return null;
        //    }
        //}

        //private bool isShowRussian = true; //показывать или нет на тренировке 0-false 1-true
        //public bool IsShowRussian
        //{
        //    get { return isShowRussian; }
        //    set
        //    {
        //        isShowRussian = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        internal ObservableCollection<Phrase> GetAnalog()
        {
            return DataBase.DB.GetAnalogPhrase(this);
        }

        //internal static Phrase GetRandomPhrase()
        //{
        //    return DataBase.DB.GetRandomPhrase();
        //}

        public static string GetRandomEnglish()
        {
            return DataBase.DB.GetRandomEnglish();
        }

        internal static string GetRandomRussian()
        {
            return DataBase.DB.GetRandomRussian();
        }

        internal static string GetRandomSound()
        {
            //return DataBase.DB.GetRandomEnglish();
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
