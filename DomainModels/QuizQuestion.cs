using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class QuizQuestion {
        [Key]
        public int QuizQuestionId { get; set; }
        public int QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
        public string QuestionText { get; set; } = "";
        public string CorrectAnswer { get; set; } = "";
        public string Explananation { get; set; } = "";
        public TimeSpan AnswerTime { get; set; }    
        public string Option1 { get; set; } = "";
        public string Option2 { get; set; } = "";
        public string Option3 { get; set; } = "";
        public string Option4 { get; set; } = "";
        public bool IsOption1Selected { get; set; } = false;
        public bool IsOption2Selected { get; set; } = false;
        public bool IsOption3Selected { get; set; } = false;
        public bool IsOption4Selected { get; set; } = false;
        
    }
}
