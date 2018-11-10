using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibson_Flashcards.Model;
using System.Windows.Input;
using System.Diagnostics;
using Google.Cloud.Speech.V1;
using Google.Apis.Auth.OAuth2;
using System.Threading;

namespace Gibson_Flashcards.ViewModel
{
    class StudyFlashcard : Gibson_Flashcards.Interface.ObservableObject
    {
        private string _filePath;
        private string _folderPath = @"C:\Flashcards\";
        private bool? firstFront = true;
        private int i = -1;
        private List<Flashcard> cardSet = new List<Flashcard>();

        private string _frontSide; //Holds the status for the application.
        public string FrontSide //Getter and Setter.
        {
            get
            {
                return _frontSide;
            }
            set
            {
                _frontSide = value;
                RaisePropertyChangedEvent("FrontSide");
            }
        }

        private string _backSide; //Holds the status for the application.
        public string BackSide //Getter and Setter.
        {
            get
            {
                return _backSide;
            }
            set
            {
                _backSide = value;
                RaisePropertyChangedEvent("BackSide");
            }
        }

        private string _voiceCapture; //Holds the status for the application.
        public string VoiceCapture //Getter and Setter.
        {
            get
            {
                return _voiceCapture;
            }
            set
            {
                _voiceCapture = value;
                RaisePropertyChangedEvent("VoiceCapture");
            }
        }

        private bool _notRunning = true; //Holds the status for VoiceCapture.
        public bool NotRunning
        {
            get
            {
                return _notRunning;
            }
            set
            {
                _notRunning = value;
                RaisePropertyChangedEvent("NotRunning");
            }
        }


        //Command for selecting card set text file.
        public ICommand SelectSetCommand
        {
            get;
            private set;
        }

        //Command for going to the next card in the list.
        public ICommand NextCardCommand
        {
            get;
            private set;
        }

        //Command for showing the backside of the current card.
        public ICommand FlipCardCommand
        {
            get;
            private set;
        }

        //Command for returning to main menu and reseting data.
        public ICommand MainMenuCommand
        {
            get;
            private set;
        }

        //Command for verifying pronunciation.
        public ICommand SpeakCommand
        {
            get;
            private set;
        }

        public StudyFlashcard()
        {
            //Assigns commands to methods.
            SelectSetCommand = new Gibson_Flashcards.Interface.DelegateCommand(SelectCardSet, IsNotRunning);
            NextCardCommand = new Gibson_Flashcards.Interface.DelegateCommand(NextCard, CanNextCard);
            FlipCardCommand = new Gibson_Flashcards.Interface.DelegateCommand(FlipCard, ReadyShowBackSide);
            MainMenuCommand = new Gibson_Flashcards.Interface.DelegateCommand(ResetData);
            SpeakCommand = new Gibson_Flashcards.Interface.DelegateCommand(CaptureVoice, ReadyPronunciation);
        }

        //Selects the card set text file, creates flashcard objects out of words, and stores them into a list. Displays the first flashcard object, first word.
        private void SelectCardSet(object obj)
        {
            try
            {
                Directory.CreateDirectory(_folderPath); //Create a new directory if not already created.

                var openDialog = new Microsoft.Win32.OpenFileDialog(); //Dialog box for file browsing.
                openDialog.InitialDirectory = @"C:\Flashcards\"; //Sets the default folder in while the openDialog will open in.
                openDialog.Filter = "Text Document(*.txt)|*.txt"; //Specifices which file type that is accepted and shown.
                openDialog.Title = "Please select a Card Set."; //Sets title of dialog box.
                bool? result = openDialog.ShowDialog(); //Shows file-browsing dialog box.
                if (!(bool)result) //Closes the dialog box if cancel was chosen.
                {
                    return;
                }
                ResetData(); //Resets data.
                string _filePath = openDialog.FileName; //Saves the file's filepath.

                using (var reader = new StreamReader(_filePath)) //Creates a StreamReader for reading the strings in the card set text file.
                {
                    while (!reader.EndOfStream) //While not at the end of the file...
                    {
                        var line = reader.ReadLine(); //Store line in variable.
                        var values = line.Split(';'); //Take the strings separated by a semicolon into an array.

                        cardSet.Add(new Flashcard(values[0], values[1])); //Create a Flashcard object using the strings and store the Flashcard in a list.
                    }
                    reader.Close(); //Closes reader.
                }

                FrontOrBack(); //Shows window dialog, asking if user wants to show front side or back side first.
            }
            catch (NullReferenceException e) //Catches for references to null files.
            {
                Debug.WriteLine("Attempted to Open/Read/Write to Unexisting File.");
                return;
            }
        }

        //Determines whether VoiceCapture is running or not.
        private bool IsNotRunning()
        {
            if (NotRunning == true)
                return true;
            else
                return false;
        }

        //Determines whether VoiceCaputre is running or not. Object parameter version.
        private bool IsNotRunning(object obj)
        {
            if (NotRunning == true)
                return true;
            else
                return false;
        }

        //A ShowDialog Window that ask the user if the front or back side will show first, returning the DialogResult.
        private void FrontOrBack()
        {
            Gibson_Flashcards.View.StudyMenu _studyMenu = new View.StudyMenu();
            firstFront = _studyMenu.ShowDialog();
        }

        //Checks if cardSet is populated, used for delegate command.
        private bool CardSetPopulated(object obj)
        {
            if (cardSet.Count() > 0 && IsNotRunning() == true)
            {
                return true;
            }
            else
                return false;
        }

        //Checks if cardSet is populated, general use. For the purposes of fulfilling assignment requirements.
        private bool CardSetPopulated()
        {
            if (cardSet.Count() > 0)
                return true;
            else
                return false;
        }

        //Checks if the cardSet is populated and the iterator i is greater than -1.
        private bool ReadyShowBackSide(object obj)
        {
            if (CardSetPopulated() == true && i > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks if the cardSet is populated and the iterator i is greater than -1. Non object version.
        private bool ReadyShowBackSide()
        {
            if (CardSetPopulated() == true && i > -1 && IsNotRunning() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Shows the next card, clearing the second word.
        private void NextCard(object obj)
        {
            try
            {
                i++; //Increments.

                //If user wanted to show front side first...
                if (firstFront == true)
                {
                    FrontSide = cardSet[i].FirstWord; //Show first word on front side.
                }
                else //If user wanted to show back side first...
                {
                    FrontSide = cardSet[i].SecondWord; //Show second word on front side.
                }

                BackSide = ""; //Make backside blank.
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.WriteLine("Attempted Access to Unexisting Object in List.");
                return;
            }
        }

        //Checks if there are cards in the list not already used.
        private bool CanNextCard(object obj)
        {
            if (CardSetPopulated() == false || cardSet.Count() <= i || IsNotRunning() == false)
                return false;
            else
                return true;
        }

        //Shows the second word.
        private void FlipCard(object obj)
        {
            try
            {
                if (firstFront == true) //If user wanted to show front side first...
                {
                    BackSide = cardSet[i].SecondWord;//Show the second word on backside.
                }
                else //If user wanted to show the back side first...
                {
                    BackSide = cardSet[i].FirstWord; //Show the first word on back side.
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.WriteLine("Attempted Access to Unexisting Object in List.");
                return;
            }
        }

        //Verifies if microphone is available.
        private bool VerifyMicrophone()
        {
            if (NAudio.Wave.WaveIn.DeviceCount < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Checks if there is a microphone available and a card set loaded.
        private bool ReadyPronunciation(object obj)
        {
            if (VerifyMicrophone() == true && ReadyShowBackSide() == true && IsNotRunning() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void StreamingMicConfigureAsync()
        {
            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = "ja-JP",
                        },
                        InterimResults = true,
                    }
                });
        }

        //Captures Speech and Converts to text; displays messages.
        private async void CaptureVoice(object obj)
        {
            NotRunning = false;
            VoiceCapture = "Status: Loading";

            List<string> words = new List<string>();

            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding =
                            RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = "ja-JP",
                        },
                        InterimResults = true,
                    }
                });
            // Print responses as they arrive.
            Task printResponses = Task.Run(async () =>
            {

                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream
                        .Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            Console.WriteLine(alternative.Transcript);
                            words.Add(alternative.Transcript);
                        }
                    }
                }
            });
            // Read from the microphone and stream to API.
            object writeLock = new object();
            bool writeMore = true;
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            waveIn.DataAvailable +=
                (object sender, NAudio.Wave.WaveInEventArgs args) =>
                {
                    lock (writeLock)
                    {
                        if (!writeMore) return;
                        streamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };
            waveIn.StartRecording();
            VoiceCapture = "Status: Speak Now";
            Console.WriteLine("Speak now.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            // Stop recording and shut down.
            waveIn.StopRecording();
            VoiceCapture = "Status: Processing";
            lock (writeLock) writeMore = false;
            await streamingCall.WriteCompleteAsync();
            await printResponses;

            if (words.Contains(BackSide) == true || words.Contains(FrontSide) == true)
            {
                VoiceCapture = "Status: Correct";
                Console.WriteLine("True");
            }
            else
            {
                VoiceCapture = "Status: Incorrect";
                Console.WriteLine("False");
            }

            NotRunning = true;
        }

        //Resets data, for delegate command use.
        private void ResetData(object obj)
        {
            firstFront = true;
            FrontSide = "";
            BackSide = "";
            cardSet.Clear();
            _filePath = "";
            i = -1;
        }

        //Resets data, general method.
        private void ResetData()
        {
            firstFront = true;
            FrontSide = "";
            BackSide = "";
            cardSet.Clear();
            _filePath = "";
            i = -1;
        }
    }
}
