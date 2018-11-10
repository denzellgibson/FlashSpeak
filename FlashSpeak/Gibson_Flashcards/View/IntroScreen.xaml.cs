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

namespace Gibson_Flashcards.View
{
    /// <summary>
    /// Interaction logic for IntroScreen.xaml
    /// </summary>
    public partial class IntroScreen : Window
    {
        private Gibson_Flashcards.View.MainMenu _mm = new View.MainMenu();

        public IntroScreen()
        {
            InitializeComponent();
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            _mm.Show();
            this.Close();

        }
    }
}
