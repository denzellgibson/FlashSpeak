using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using Gibson_Flashcards.ViewModel;

namespace Gibson_Flashcards.View
{
    /// <summary>
    /// Interaction logic for CreateSetView.xaml
    /// </summary>
    public partial class CreateSetView : Window
    {

        public CreateSetView()
        {
            InitializeComponent();
        }

        public void HideWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
