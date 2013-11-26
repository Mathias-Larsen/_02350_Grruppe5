using _02350_Gruppe5.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02350_Gruppe5.View
{
    /// <summary>
    /// Dette er 'Code-Behind' filen til vinduet MainWindow. Når man benytter sig af MVVM bør der ikke være noget i denne fil.
    /// Det er dog desværre ikke muligt / meget svært at overholde denne praksis da der stadig mangler nogle ting i WPF for at det kan lade sig gøre.
    /// Ved hjælp af MVVM Light Toolkitet som benyttes til dette program er det for det meste muligt, så prøv at holde disse filer tomme hvis i kan.
    /// Denne klasses er halvdelen af vinduets klasse, XAML koden er den anden del.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
