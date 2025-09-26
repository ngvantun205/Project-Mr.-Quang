using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class ReadingLesson {
        [Key]
        public int ReadingLessonId { get; set; }
        public string Title { get; set; } = "";   
        public string Content { get; set; } = "";  
        public string Level { get; set; } = ""; 
        public TimeSpan? SuggestedTime { get; set; } 
        public ICollection<ReadingQuestion> Questions { get; set; } = new List<ReadingQuestion>();
        public ICollection<UserReadingResult> UserReadingResults { get; set; } = new List<UserReadingResult>();
    }
}
