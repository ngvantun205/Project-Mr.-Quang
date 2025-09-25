using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class ListeningQuestion {
        [Key]
        public int ListeningQuestionId { get; set; }
        public int ListeningLessonId { get; set; }

        [ForeignKey(nameof(ListeningLessonId))]
        public ListeningLesson ListeningLesson { get; set; }

        public string CorrectAnswer { get; set; } = "";
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; } = "";
        public string Option1 { get; set; } = "#";
        public string Option2 { get; set; } = "#";
        public string Option3 { get; set; } = "#";
        public string Option4 { get; set; } = "#";
        public string Explanation { get; set; } = "";
        public bool IsOption1Selected { get; set; } = false;
        public bool IsOption2Selected { get; set; } = false;
        public bool IsOption3Selected { get; set; } = false;
        public bool IsOption4Selected { get; set; } = false;
    }
}
