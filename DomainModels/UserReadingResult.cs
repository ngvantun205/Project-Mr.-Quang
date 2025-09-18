using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserReadingResult {
        [Key]
        public int UserReadingResultId { get; set; }
        public int UserId { get; set; }
        public int ReadingLessonId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;
    }
}
