using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class ReadingQuestion {
        [Key]
        public int ReadingQuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = "";
        public ICollection<AnswerOption> Options { get; set; }  = new List<AnswerOption>();
        public string CorrectAnswer { get; set; } = "";
        public string Explanation { get; set; } = ""; 
    }
}
