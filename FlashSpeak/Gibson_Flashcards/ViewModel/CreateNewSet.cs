using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gibson_Flashcards.Interface;
using static Gibson_Flashcards.Interface.DelegateCommand;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;

namespace Gibson_Flashcards.ViewModel
{
    class CreateNewSet : Gibson_Flashcards.Interface.ObservableObject
    {
        private string _filePath;
        private string _folderPath = @"C:\Flashcards\";

        private string _firstWord;
        public string FirstWord
        {
            get
            {
                return _firstWord;
            }
            set
            {
                _firstWord = value;
                RaisePropertyChangedEvent("FirstWord");
            }
        }

        private string _secondWord;
        public string SecondWord
        {
            get
            {
                return _secondWord;
            }
            set
            {
                _secondWord = value;
                RaisePropertyChangedEvent("SecondWord");
            }
        }

        private string _cardStatus;
        public string CardStatus
        {
            get
            {
                return _cardStatus;
            }
            set
            {
                _cardStatus = value;
                RaisePropertyChangedEvent("CardStatus");
            }
        }

        //Command for adding card to a set.
        public ICommand AddCardCommand
        {
            get;
            private set;
        }

        //Command for saving a set.
        public ICommand FinishSetCommand
        {
            get;
            private set;
        }

        //Command for canceling set creation.
        public ICommand CancelSetCommand
        {
            get;
            private set;
        }

        public CreateNewSet()
        {
            AddCardCommand = new Gibson_Flashcards.Interface.DelegateCommand(AddCard);
            FinishSetCommand = new Gibson_Flashcards.Interface.DelegateCommand(FinishSet);
            CancelSetCommand = new Gibson_Flashcards.Interface.DelegateCommand(CancelSet);
        }

        //Adds a card to the set.
        private void AddCard(object obj)
        {
            try
            {
                //Checks if there are words to add.
                if (FirstWord == "" || FirstWord == null || SecondWord == null || SecondWord == "")
                    return;

                //Checks if there is no file path.
                if (string.IsNullOrEmpty(_filePath)) //if not...
                {
                    Directory.CreateDirectory(_folderPath); //Create a new directory if not already created.

                    //Creates save dialog box that allows user to name and save the file they are going to write to.
                    var saveDialog = new Microsoft.Win32.SaveFileDialog();
                    saveDialog.InitialDirectory = @"C:\Flashcards\";
                    saveDialog.Filter = "Text Document(*.txt)|*.txt";
                    saveDialog.Title = "Card Set Name";
                    bool? result = saveDialog.ShowDialog();
                    if (!(bool)result)
                        return;

                    //Creates the file.
                    if (saveDialog.FileName != "") //As long the file has a name...
                    {
                        // Saves the File via a FileStream created by the OpenFile method.  
                        _filePath = saveDialog.FileName;
                        System.IO.FileStream fileStream = (System.IO.FileStream)saveDialog.OpenFile();
                        fileStream.Close(); //Can use StreamWriter to write onto files.
                    }
                }
                //Writes to files using StreamWriter, taking the filepath as parameter.
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(_filePath, true))
                {
                    file.WriteLine(FirstWord + ";" + SecondWord); //Writes the words into file.
                }

                //Resets the variables.
                FirstWord = "";
                SecondWord = "";

                //User Feedback.
                CardStatus = "Added!";
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("File Does Not Exist.");
                CardStatus = "Not Added";
                return;
            }
        }

        //Resets filepath and variables; used in conjuction with hiding the window.
        private void FinishSet(object obj)
        {
            _filePath = "";
            FirstWord = "";
            SecondWord = "";
        }

        //Deletes the newly created file, and resets variables; used in conjucion with hiding the window.
        private void CancelSet(object obj)
        {
            try
            {
                if (_filePath != null)
                {
                    File.Delete(_filePath);
                }
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("File Does Not Exist");
                return;
            }
        }
    }
}
