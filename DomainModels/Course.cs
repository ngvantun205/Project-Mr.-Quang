using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels
{
    public class Course : Bindable {
        [Key]
        public int CourseId { get; set; }	
        public string Title { get; set; } = "";
		public string Description { get; set; } = "";
		public string Level { get; set; } = "";
		public string Category { get; set; } = "";

		public int CreateBy { get; set; }
		public DateTime CreateDate { get; set; }



	}
}
