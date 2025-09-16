using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Community: Bindable {
        [Key]
        public int PostId { get; set; }

        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public int CommentId { get; set; }
        public string CommentText { get; set; } = "";
        public int LikeCount { get; set; }
    }
}
