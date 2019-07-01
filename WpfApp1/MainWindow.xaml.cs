using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Temp> oc { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            oc = new ObservableCollection<Temp>()
            {
                new Temp() { One="qqq", Two="rrrrrr", Three="tttttttttt"},
                new Temp() { One="tttttt", Two="nnnnnnnnnnn", Three="iiiiiiiiiiiii"},
                new Temp() { One="eeeeeeeeeeee", Two="fffffffff", Three="ttttooooooooooootttttt"},
                new Temp() { One="ggggggg", Two="nnnnnn", Three="kkkkkkkkkk"},
                new Temp() { One="ddddddd", Two="eeeeeeee", Three="tttthhhhhhhhhtttttt"},
                new Temp() { One="pppppppppp", Two="ssssssssss", Three="gggggggggggg"}
            };

            DataContext = this;
        }
    }

    public class Temp : INotifyPropertyChanged
    {
        private string one;
        public string One
        {
            get { return one; }
            set
            {
                one = value;
                OnPropertyChanged();
            }
        }

        private string two;
        public string Two
        {
            get { return two; }
            set
            {
                two = value;
                OnPropertyChanged();
            }
        }

        private string three;
        public string Three
        {
            get { return three; }
            set
            {
                three = value;
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
