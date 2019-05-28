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
    /// Логика взаимодействия для AddWord.xaml
    /// </summary>
    public partial class AddWord : Window
    {
        public AddWord(string word)
        {
            InitializeComponent();

            int i = 0;
            int j = 0;

            foreach (var temp in Transcript.dictTranscript)
            {
                Label lbl = new Label();
                lbl.Content = temp.Key;
                lbl.FontSize = 18;
                lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalAlignment = VerticalAlignment.Stretch;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                lbl.Margin = new Thickness(10);
                Grid.SetColumn(lbl, j++);
                Grid.SetRow(lbl, i);
                lbl.MouseLeftButtonDown += lbl_MouseLeftButtonDown;
                SymbolsGr.Children.Add(lbl);
                if (j >= SymbolsGr.ColumnDefinitions.Count)
                {
                    j = 0;
                    i++;
                }
            }
        }

        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TranscriptTb.Text += (sender as Label).Content;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DB.SaveWord(new EnglishWords() { Word = WordTb.Text, Transcription = TranscriptTb.Text});
            this.Close();
        }
    }
}
