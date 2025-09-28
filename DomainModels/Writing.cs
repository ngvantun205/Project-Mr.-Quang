using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class Writing {
        public int WritingId { get; set; }
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public string Feedback { get; set; } = "";
        public int UserId { get; set; }
        public DateTime SubmitDate { get; set; } = DateTime.Now;
    }
}
