using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.ViewModels
{
    public class ShowAllPhrasesVM : IPageViewModel, INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return "Show All Phrases";
            }
        }

        private ObservableCollection<Models.Phrase> listPhrases; 
        public ObservableCollection<Models.Phrase> ListPhrases
        {
            get { return listPhrases; }
            private set
            {
                if (value == listPhrases)
                    return;

                listPhrases = value;
                OnPropertyChanged();
            }
        }



        public void Init()
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
            ListPhrases.CollectionChanged += ListPhrases_CollectionChanged;
        }

        private void ListPhrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged += Phrase_PropertyChanged;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged -= Phrase_PropertyChanged;
                }
            }
        }

        private void Phrase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Models.Phrase row = sender as Models.Phrase;
            row.Update();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
