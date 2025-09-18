using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Vocabulary {
        [Key]
        public int VocabularyId { get; set; }
        public string Word { get; set; } = "";
        public string WordType { get; set; } = "";
        public string Meaning { get; set; } = "";
        public string IPATranscription { get; set; } = "";
        public string ExampleSentence { get; set; } = "";
        public string Level { get; set; } = "";
        public string Topic { get; set; } = "";

    }
}
