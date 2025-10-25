using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserSpeakingRecord {
        [Key]
        public int UserSpeakingRecordId { get; set; }
        public int UserId { get; set; }
        public int SpeakingSentenceId { get; set; } 
        public double Accuracy { get; set; }
        public double Fluency { get; set; }
        public double Completeness { get; set; }
        public double PronScore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
