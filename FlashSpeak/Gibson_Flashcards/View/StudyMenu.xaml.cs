using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for StudyMenu.xaml
    /// </summary>
    public partial class StudyMenu : Window
    {
        public StudyMenu()
        {
            InitializeComponent();
        }

        //Hides the window instead when closing.
        protected override void OnClosing(CancelEventArgs e)
        { 
            this.Hide();      // Programmatically hides the window
        }

        //Hides the window upon click.
        public void HideWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //Returns DialogResult as true upon click.
        public void FrontTrue(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        //Returns DialogResult as false upon click.
        public void FrontFalse(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
