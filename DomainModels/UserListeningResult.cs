using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserListeningResult {
        [Key]
        public int UserListeningResultId { get; set; }
        public int UserId { get; set; }
        public int ListeningLessonId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        [ForeignKey(nameof(ListeningLessonId))]
        public ListeningLesson ListeningLesson { get; set; }
    }
}
