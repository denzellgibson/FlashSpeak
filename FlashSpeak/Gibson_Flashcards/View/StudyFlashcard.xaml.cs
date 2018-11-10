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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class StudyFlashcard : Window
    {
        public StudyFlashcard()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close    
            this.Hide();      // Programmatically hides the window
        }

        public void HideWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
