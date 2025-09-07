using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels
{
    class Course : Bindable {
		private int courseid;

		public int	CourseId { get => courseid; set => Set(ref courseid, value); }
        private string title;

        public string Title { get => title; set => Set(ref title, value); }
		private string description;

		public string Description { get => description; set => Set(ref description, value); }
		private string level;

		public string Level { get => level; set => Set(ref level, value); }
		private string category;

		public string Category { get => category; set => Set(ref category, value); }
		private int createby;

		public int CreateBy { get => createby; set => Set(ref createby, value); }
		private DateTime createdate;

		public DateTime CreateDate { get => createdate; set => Set(ref createdate, value); }



	}
}
