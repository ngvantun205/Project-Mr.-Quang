using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    class Lesson : Bindable {
		private int lessonid;

		public int LessonId { get => lessonid; set => Set(ref lessonid, value); }
		private int courseid;

		public int CourseId { get => courseid; set => Set(ref courseid, value); }
		private string title = "";

		public string Title { get => title; set => Set(ref title, value); }
		private string content = "";

		public string Content { get => content; set => Set(ref content, value); }
		private string mediaurl	= "";

		public string MediaUrl { get => mediaurl; set => Set(ref mediaurl, value); }
		private string lessontype = "";

		public string LessonType { get => lessontype; set => Set(ref lessontype, value); }
		private int orderindex;

		public int OrderIndex { get => orderindex; set => Set(ref orderindex, value); }


	}
}
