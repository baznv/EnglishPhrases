using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.ViewModels
{
    public class ShowAllPhrasesVM : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Show All Phrases";
            }
        }

        public ObservableCollection<Models.Phrase> ListPhrases { get; set; }

        //public ShowAllPhrasesVM()
        //{
        //    ListPhrases = Models.Phrase.GetAllPhrases();
        //}

        public void Init()
        {
            ListPhrases = Models.Phrase.GetAllPhrases();
        }

    }
}
