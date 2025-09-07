using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class WritingSubmission : Bindable {
		private int submissionid;

		public int SubmissionId { get => submissionid; set => Set(ref submissionid, value); }
        private int lessonid;

        public int LessonId { get => lessonid; set => Set(ref lessonid, value); }
        private int userid;

        public int UserId { get => userid; set => Set(ref userid, value); }
        private int score;

        public int Score { get => score; set => Set(ref score, value); }
        private string content ="";

        public string Content { get => content; set => Set(ref content, value); }
        private string aifeedback = "";

        public string AIFeedback { get => aifeedback; set => Set(ref aifeedback, value); }
        private DateTime submitteddate;

        public DateTime SubmittedDate { get => submitteddate; set => Set(ref submitteddate, value); }



    }
}
