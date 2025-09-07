using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    class Quiz : Bindable {
		private int quizid;

		public int QuizId { get => quizid; set => Set(ref quizid, value); }
		private int lessonid;

		public int LessonId { get => lessonid; set => Set(ref lessonid, value); }
		private string quiztype = "";

		public string QuizType { get => quiztype; set => Set(ref quiztype, value); }
		private string questiontext = "";

		public string QuestionText { get => questiontext; set => Set(ref questiontext, value); }
		private string options = "";

		public string Options { get => options; set => Set(ref options, value); }
		private string correctanswer = "";

		public string CorrectAnswer { get => correctanswer; set => Set(ref correctanswer, value); }
		private string explanation = ""	;

		public string Explanation { get => explanation; set => Set(ref explanation, value); }

	}
}
