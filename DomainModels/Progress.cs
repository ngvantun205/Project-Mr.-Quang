using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Progress : Bindable {
		private int progressid;

		public int ProgressId { get => progressid; set => Set(ref progressid, value); }
		private int userid;

		public int UserId { get => userid; set => Set(ref userid, value); }
		private int courseid;
		public int CourseId { get => courseid; set => Set(ref courseid, value); }
		private int lessonid;
		public int LessonId { get => lessonid; set => Set(ref lessonid, value); }
		private int iscompleted;
		public int IsCompleted { get => iscompleted; set => Set(ref iscompleted, value); }
		private int score;
		public int Score { get => score; set => Set(ref score, value); }
		private DateTime completeddate;
		public DateTime CompletedDate { get => completeddate; set => Set(ref completeddate, value); }

    }
}
