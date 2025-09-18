using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class ListeningQuestion {
        [Key]
        public int ListeningQuestionId { get; set; }
        public string CorrectAnswer { get; set; } = "";
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = "";
        public string Explanation { get; set; } = "";
        public IEnumerable<AnswerOption>? Options { get; set; }
    }
}
