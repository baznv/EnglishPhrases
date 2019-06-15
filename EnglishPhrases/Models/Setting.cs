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
    public class Setting : INotifyPropertyChanged
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public static ObservableCollection<Setting> GetItems()
        {
            return DataBase.DB.GetAllSettings();
        }

        private string valueSetting;
        public string ValueSetting
        {
            get { return valueSetting; }
            set
            {
                valueSetting = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
