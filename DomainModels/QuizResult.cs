using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    class QuizResult : Bindable {
		private int quizresultid;

		public int QuizResultId { get => quizresultid; set => Set(ref quizresultid, value); }
		private int userid;

		public int UserId { get => userid; set => Set(ref userid, value); }
		private int quizid;

		public int QuizId { get => quizid; set => Set(ref quizid, value); }
		private string selectedanswer = "";

		public string SelectedAnswer { get => selectedanswer; set => Set(ref selectedanswer, value); }
		private int iscorrect;

		public int IsCorrect { get => iscorrect; set => Set(ref iscorrect, value); }
		private int score;

		public int Score { get => score; set => Set(ref score, value); }
		private DateTime submitteddate;

		public DateTime SubmittedDate { get => submitteddate; set => Set(ref submitteddate, value); }
	}
}
