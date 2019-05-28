using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EnglishPhrases
{
    /// <summary>
    /// Логика взаимодействия для AddPhrases.xaml
    /// </summary>
    public partial class AddPhrases : Window
    {
        public AddPhrases()
        {
            InitializeComponent();
        }

        private void PhraseTb_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void PhraseTb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                string[] words = PhraseTb.Text.Split(' ');
                string word = words[words.Length-1];
                new AddWord(word).ShowDialog();
            }
        }
    }
}
