using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gibson_Flashcards.ViewModel
{
    public class MainViewModel : Gibson_Flashcards.Interface.ObservableObject
    {
        private Gibson_Flashcards.View.CreateSetView _createSetView = new View.CreateSetView();
        private Gibson_Flashcards.View.StudyFlashcard _sfc = new View.StudyFlashcard();
        private string folderPath = @"C:\Flashcards\";

        //Command for starting the creation for a set.
        public ICommand CreateSetCommand
        {
            get;
            private set;
        }

        public ICommand MainMenuCommand
        {
            get;
            private set;
        }

        public ICommand StudyCommand
        {
            get;
            private set;
        }

        //Declarations, Instantiations, Assignments.
        public MainViewModel()
        {

            //Assigns Button Commands to buttons.
            CreateSetCommand = new Gibson_Flashcards.Interface.DelegateCommand(CreateNewSet);
            StudyCommand = new Gibson_Flashcards.Interface.DelegateCommand(Study);
        }

        //Shows the window for the card creation.
        private void CreateNewSet(object obj)
        {

            //Shows the create new set window.
            _createSetView.Show();
        }

        //Shows the Study Menu window.
        private void Study(object obj)
        {
            _sfc.Show();
        }
    }
}
