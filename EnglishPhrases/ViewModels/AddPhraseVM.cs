using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnglishPhrases.ViewModels
{
    public class AddPhraseVM : IPageViewModel, INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return "Add Phrase";
            }
        }

        private Models.EnglishSentence currentEnglish;
        public Models.EnglishSentence CurrentEnglish
        {
            get { return currentEnglish; }
            set
            {
                currentEnglish = value;
                OnPropertyChanged();
            }
        }

        private Models.RussianSentence currentRussian;
        public Models.RussianSentence CurrentRussian
        {
            get { return currentRussian; }
            set
            {
                currentRussian = value;
                OnPropertyChanged();
            }
        }

        private ICommand addPhraseCommand;
        public ICommand AddPhraseCommand
        {
            get
            {
                return addPhraseCommand ??
                    (
                    addPhraseCommand = new Other.RelayCommand(
                        p => SavePhrase(),
                        p => CurrentEnglish.Sentense != "" && CurrentRussian.Sentense != "")
                    );
            }
        }

        public void SavePhrase()
        {
            Models.EnglishSentence.Save(CurrentEnglish);
            Models.RussianSentence.Save(CurrentRussian);
            Models.Relation.Save(new Models.Relation()
            {
                ID_EnglishSentence = CurrentEnglish.ID,
                ID_RussianSentence = CurrentRussian.ID,
                DateAdd = DateTime.Now.ToString("yyyy.MM.dd"),
                Show = true,
                CountShow = 0
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
