using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    public class Quiz : Bindable {
        [Key]
        public int QuizId { get; set; }
        public int LessonId { get; set; }
        public string QuizType { get; set; } = "";
        public string QuestionText { get; set; } = "";
        public string Options { get; set; } = "";

		public string CorrectAnswer { get; set; } = "";
		public string Explanation { get; set; } = "";

	}
}
