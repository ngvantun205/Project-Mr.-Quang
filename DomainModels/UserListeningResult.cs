using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class UserListeningResult {
        [Key]
        public int UserListeningResultId { get; set; }
        public int UserId { get; set; }
        public int ListeningLessonId { get; set; }
        public int Score { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;
    }
}
