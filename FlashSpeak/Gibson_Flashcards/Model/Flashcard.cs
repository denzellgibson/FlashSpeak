using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibson_Flashcards.Model
{
    class Flashcard
    {
        public string FirstWord {get; set;}
        public string SecondWord { get; set; }

        public Flashcard(string fw, string sw)
        {
            FirstWord = fw;
            SecondWord = sw;
        }
    }
}
