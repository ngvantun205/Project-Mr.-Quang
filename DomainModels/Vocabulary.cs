using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Vocabulary {
        private int id;
        public int Id { get => id; set => id = value; }
        private string word = "";
        public string Word { get => word; set => word = value; }
        private string meaning = "";
        public string Meaning { get => meaning; set => meaning = value; }
        private string exampleSentence = "";
        public string ExampleSentence { get => exampleSentence; set => exampleSentence = value; }

    }
}
