using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ForeignKey(nameof(ReadingLessonId))]
        public ReadingLesson ReadingLesson { get; set; }
    }
}
