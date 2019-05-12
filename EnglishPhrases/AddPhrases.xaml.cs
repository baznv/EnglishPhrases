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

        private void Temp_Click(object sender, RoutedEventArgs e)
        {
            AddWord aw = new AddWord();
            aw.ShowDialog();
        }
    }
}
