using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserAttempt {

        [Key]
        public int UserAttemptId { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseType { get; set; } = ""; 
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
