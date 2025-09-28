using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class AIChat {
        [Key]
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public DateTime SendDate { get; set; }
    }
}
