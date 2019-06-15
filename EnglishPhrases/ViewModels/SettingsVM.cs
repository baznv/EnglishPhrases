using EnglishPhrases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.ViewModels
{
    public class SettingsVM : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Settings";
            }
        }

        public ObservableCollection<Setting> Settings { get; set; }
        public void Init()
        {
            Settings = Setting.GetItems();
        }
    }
}
