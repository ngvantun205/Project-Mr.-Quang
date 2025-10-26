using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class SpeakingSentence {
        [Key]
        public int SpeakingSentenceId { get; set; }
        public int TopicId { get; set; }
        public string SentenceText { get; set; } = "";
        public string Level { get; set; } = "Beginner";
        public Topic? Topic { get; set; }


    }
}
