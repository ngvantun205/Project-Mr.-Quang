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
        public string Title { get; set; } = "";   // tiêu đề bài đọc
        public string Content { get; set; } = ""; // nội dung đoạn văn
        public ICollection<ReadingQuestion> Questions { get; set; } = new List<ReadingQuestion>(); // danh sách câu hỏi
        public string Level { get; set; } = "";   // Beginner, Intermediate, Advanced
        public TimeSpan? SuggestedTime { get; set; } // gợi ý thời gian đọc
    }
}
