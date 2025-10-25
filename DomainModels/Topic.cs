using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class Topic {
        public int TopicId { get; set; }
        public string TopicName { get; set; } = "";
        public string Level { get; set; } = "Beginner";
        public string Description { get; set; } = string.Empty;
    }
}
