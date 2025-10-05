using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Framwork.Bindable;

namespace TDEduEnglish.DomainModels {
    public class Quiz : Bindable {
        [Key]
        public int QuizId { get; set; }
        public string Title { get; set; } = "";
        public string Level { get; set; } = "";
        public string Topic { get; set; } = "";
        public TimeSpan SuggestedTime { get; set; }
        public IEnumerable<QuizQuestion> Options { get; set; }
	}
}
