using Microsoft.Win32;
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

        private Models.EnglishSentence currentEnglish = new Models.EnglishSentence();
        public Models.EnglishSentence CurrentEnglish
        {
            get { return currentEnglish; }
            set
            {
                currentEnglish = value;
                OnPropertyChanged();
            }
        }

        private Models.RussianSentence currentRussian = new Models.RussianSentence();
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
                    addPhraseCommand = new Commands.RelayCommand(
                        p => SavePhrase(),
                        p =>  !string.IsNullOrEmpty(CurrentEnglish.Sentense) 
                                && !string.IsNullOrEmpty(CurrentRussian.Sentense)
                       )
                    );
            }
        }

        private ICommand savePathCommand;
        public ICommand SavePathCommand
        {
            get
            {
                return savePathCommand ??
                    (
                    savePathCommand = new Commands.RelayCommand(
                        p => SavePath(),
                        p => !string.IsNullOrEmpty(CurrentEnglish.Sentense)
                       )
                    );
            }
        }

        private void SavePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                CurrentEnglish.PathToSound = openFileDialog.FileName;
                //return true;
            }
            //return false;
        }

        public void SavePhrase()
        {
            Models.EnglishSentence.Save(CurrentEnglish);
            Models.RussianSentence.Save(CurrentRussian);
            Models.Phrase.Save(new Models.Phrase()
            {
                ID_EnglishSentence = CurrentEnglish.ID,
                ID_RussianSentence = CurrentRussian.ID,
                DateAdd = DateTime.Now.ToString("yyyy.MM.dd"),
                Show = true,
                CountShow = 0
            });
            CurrentEnglish = new Models.EnglishSentence();
            CurrentRussian = new Models.RussianSentence();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
