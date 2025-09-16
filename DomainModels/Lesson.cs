using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    class Lesson : Bindable {
        [Key]
        public int LessonId { get; set; } 
        public int CourseId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string MediaUrl { get; set; } = "";
		public string LessonType { get; set; } = "";
		public int OrderIndex { get; set; } = 0;

	}
}
