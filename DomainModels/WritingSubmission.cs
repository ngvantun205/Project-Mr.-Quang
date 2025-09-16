using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class WritingSubmission : Bindable {
        [Key]
        public int SubmissionId { get; set; }   
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string Content { get; set; } = "";

        public string AIFeedback { get; set; } = "";
        public DateTime SubmittedDate { get; set; } = DateTime.Now;


    }
}
