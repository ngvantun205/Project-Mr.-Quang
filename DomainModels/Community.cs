using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Community: Bindable {
        private int postid;
        public int PostId { get => postid; set => Set(ref postid, value); }
        private int userid;
        public int UserId { get => userid; set => Set(ref userid, value); }
        private string title = "";
        public string Title { get => title; set => Set(ref title, value); }
        private string content = "";
        public string Content { get => content; set => Set(ref content, value); }
        private DateTime createddate;
        public DateTime CreatedDate { get => createddate; set => Set(ref createddate, value); }
        private int commentid;
        public int CommentId { get => commentid; set => Set(ref commentid, value); }
        private string commenttext = "";
        public string CommentText { get => commenttext; set => Set(ref commenttext, value); }
        private int likecount;
        public int LikeCount { get => likecount; set => Set(ref likecount, value); }
    }
}
