using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class SpeakingSentence {
        public int SpeakingSentenceId { get; set; }
        public int TopicId { get; set; }
        public string SentenceText { get; set; } = "";
        public string Level { get; set; } = "Beginner";
    }
}
