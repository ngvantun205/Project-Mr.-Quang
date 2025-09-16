using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Progress : Bindable {
        [Key]
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public int IsCompleted { get; set; }
        public int Score { get; set; }
		public DateTime CompletedDate { get; set; }

    }
}
