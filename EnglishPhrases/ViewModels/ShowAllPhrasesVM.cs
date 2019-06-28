using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace EnglishPhrases.ViewModels
{
    public class ShowAllPhrasesVM : IPageViewModel//, INotifyPropertyChanged//, IEditableObject
    {
        public string Name
        {
            get
            {
                return "Show All Phrases";
            }
        }

        public ObservableCollection<Models.Phrase> ListPhrases { get; set; }

        private ICommand soundCommand;
        public ICommand SoundCommand
        {
            get
            {
                return soundCommand ??
                    (
                    soundCommand = new Commands.RelayCommand(
                        p => Sound(p),
                        p => !string.IsNullOrEmpty(p?.ToString())
                       )
                    );
            }
        }

        public void Init()
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
            //ListPhrases.CollectionChanged += ListPhrases_CollectionChanged;
        }


        //private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    ListPhrases = Models.Phrase.GetAllPhrases();
        //}


        private void Sound(object p)
        {
            throw new NotImplementedException();
        }
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
