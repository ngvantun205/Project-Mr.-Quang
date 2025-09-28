using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class QuizResult : Bindable {
        [Key]
        public int QuizResultId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public string SelectedAnswer { get; set; } = "";
        public int IsCorrect { get; set; }

		public int Score { get; set; }
		public DateTime SubmittedDate { get; set    ; }
	}
}
