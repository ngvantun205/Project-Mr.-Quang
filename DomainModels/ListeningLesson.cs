using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TDEduEnglish.DomainModels {
    public class ListeningLesson {
        [Key]
        public int ListeningLessonId { get; set; }
        public string Level { get; set; } = "";
        public string Title { get; set; } = "";
        public TimeSpan? SuggestedTime { get; set; }

        public string? ListeningAudioPath { get; set; }
        public ICollection<ListeningQuestion>? Questions { get; set; } = new List<ListeningQuestion>();
        public ICollection<UserListeningResult> UserListeningResults { get; set; } = new List<UserListeningResult>();
    }
}
