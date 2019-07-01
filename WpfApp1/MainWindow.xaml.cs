using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                new Temp() { One="pppppppppp", Two="ssssssssss", Three="gggggggggggg"},

            };
            DataContext = this;
        }
    }

    public class Temp
    {
        public string One { get; set; }
        public string Two { get; set; }
        public string Three { get; set; }
    }
}
