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

        public void Init()
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
            //ListPhrases.CollectionChanged += ListPhrases_CollectionChanged;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}

        //public void BeginEdit()
        //{
        //    return;
        //}

        //public void EndEdit()
        //{
        //    ListPhrases = Models.Phrase.GetAllPhrases();
        //}

        //public void CancelEdit()
        //{
        //    return;
        //}
    }
}
