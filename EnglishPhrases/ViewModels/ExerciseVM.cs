using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EnglishPhrases.ViewModels
{
    class ExerciseVM : IPageViewModel, INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return "Exercise";
            }
        }

        //случайная фраза из базы данных
        private Models.Phrase randomPhrase;
        public Models.Phrase RandomPhrase
        {
            get { return randomPhrase; }
            set
            {
                randomPhrase = value;
                OnPropertyChanged();
            }
        }

        //английская фраза из случайной фразы
        private string englishFromRandomPhrase = null;
        public string EnglishFromRandomPhrase
        {
            get { return englishFromRandomPhrase; }
            set
            {
                englishFromRandomPhrase = RandomPhrase.EnglishPhrase;
                OnPropertyChanged();
            }
        }

        //выводить ли английскую фразу
        private bool englishOutput = false;
        public bool EnglishOutput
        {
            get { return englishOutput; }
            set
            {
                englishOutput = value;
                OnPropertyChanged();
            }
        }

        //русская фраза из случайной фразы
        private string russianFromRandomPhrase = null;
        public string RussianFromRandomPhrase
        {
            get { return russianFromRandomPhrase; }
            set
            {
                russianFromRandomPhrase = RandomPhrase.RussianPhrase;
                OnPropertyChanged();
            }
        }

        //выводить ли русскую фразу
        private bool russianOutput = false;
        public bool RussianOutput
        {
            get { return russianOutput; }
            set
            {
                russianOutput = value;
                OnPropertyChanged();
            }
        }

        //URI файла, содержащего озвучку английской фразы из случайной фразы
        private Uri uriSound = null;
        public Uri UriSound
        {
            get { return uriSound; }
            set
            {
                UriSound = new Uri(Path.Combine(App.fullPathToSounds, RandomPhrase.PathToSound.ToString()));
                OnPropertyChanged();
            }
        }

        //выводить ли звуковую дорожку
        private bool soundOutput = false;
        public bool SoundOutput
        {
            get { return soundOutput; }
            set
            {
                soundOutput = value;
                OnPropertyChanged();
            }
        }

        //использовать ли в тренировке английские фразы
        private bool isEnglish = true;
        public bool IsEnglish
        {
            get { return isEnglish; }
            set
            {
                isEnglish = value;
                OnPropertyChanged();
            }
        }

        //использовать ли в тренировке русские фразы
        private bool isRussian = true;
        public bool IsRussian
        {
            get { return isRussian; }
            set
            {
                isRussian = value;
                OnPropertyChanged();
            }
        }

        //использовать ли в тренировке звуковые дорожки
        private bool isAudio = true;
        public bool IsAudio
        {
            get { return isAudio; }
            set
            {
                isAudio = value;
                OnPropertyChanged();
            }
        }

        public void Init()
        {
            RandomPhrase = Models.Phrase.GetRandomPhrase();
            var PropertiesToShow = typeof(ExerciseVM).GetProperties().Where(c => c.Name.Contains("Output")).ToArray();
            Random random = new Random();
            var randomProperty = PropertiesToShow[random.Next(PropertiesToShow.Length)];
            randomProperty.SetValue(this, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public class PathToSoundConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var temp = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.ToString()));
                return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.ToString()));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }

        }


    }
}
