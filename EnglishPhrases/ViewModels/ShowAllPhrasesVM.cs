using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public class ShowAllPhrasesVM : IPageViewModel, INotifyPropertyChanged//, IEditableObject
    {
        public string Name
        {
            get
            {
                return "Show All Phrases";
            }
        }

        //public ObservableCollection<Models.Phrase> ListPhrases { get; set; }
        //public ObservableCollection<Models.Phrase> ListSample { get; set; } 
        private ObservableCollection<Models.Phrase> listPhrases = null;
        public ObservableCollection<Models.Phrase> ListPhrases
        {
            get { return listPhrases; }
            set
            {
                listPhrases = value;
                OnPropertyChanged();
            }
        }


        private Uri uriSound = null;
        public Uri UriSound
        {
            get { return uriSound; }
            set
            {
                uriSound = value;
                OnPropertyChanged();
            }
        }

        private bool analog = false;
        public bool Analog
        {
            get { return analog; }
            set
            {
                analog = value;
                OnPropertyChanged();
            }
        }


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

        private ICommand showAnalog;
        public ICommand ShowAnalog
        {
            get
            {
                return showAnalog ??
                    (
                    showAnalog = new Commands.RelayCommand(
                        p => GetAnalog(p),
                        p => !string.IsNullOrEmpty(p?.ToString())
                       )
                    );
            }
        }

        private void GetAnalog(object p)
        {
            //ObservableCollection<Models.Phrase> temp = (p as Models.Phrase).GetAnalog();
            ListPhrases = (p as Models.Phrase).GetAnalog();
        }

        public void Init()
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
            //ListPhrases.CollectionChanged += ListPhrases_CollectionChanged;
            //ListSample = new ObservableCollection<Models.Phrase>() { ListPhrases[0], ListPhrases[1] };
        }

        private void Sound(object p)
        {
            UriSound = new Uri(Path.Combine(App.fullPathToSounds, p.ToString()));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }


}
