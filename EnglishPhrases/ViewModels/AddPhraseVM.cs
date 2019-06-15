using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        private Models.Phrase currentPhrase;
        public Models.Phrase CurrentPhrase
        {
            get { return currentPhrase; }
            set
            {
                currentPhrase = value;
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
                        p =>  !string.IsNullOrEmpty(CurrentPhrase.EnglishPhrase) 
                                && !string.IsNullOrEmpty(CurrentPhrase.RussianPhrase)
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
                        p => !string.IsNullOrEmpty(CurrentPhrase.EnglishPhrase)
                       )
                    );
            }
        }

        private void SavePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //if (Path.GetDirectoryName(openFileDialog.FileName).Equals(App.PathToSounds))
                    CurrentPhrase.PathToSound = openFileDialog.SafeFileName;
            }
        }

        public void SavePhrase()
        {
            CurrentPhrase.Save();
            CurrentPhrase = new Models.Phrase();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Init()
        {
            CurrentPhrase = new Models.Phrase();
        }
    }
}
