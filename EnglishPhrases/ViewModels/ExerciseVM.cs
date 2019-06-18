using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishPhrases.ViewModels
{
    class ExerciseVM : IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Exercise";
            }
        }

        public void Init()
        {
            //throw new NotImplementedException();
        }
    }
}
