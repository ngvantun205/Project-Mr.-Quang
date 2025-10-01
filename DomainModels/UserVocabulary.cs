using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserVocabulary {
        [Key]
        public int UserVocabularyId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Word { get; set; } = "";
        public string Meaning { get; set; } = "";
        public string WordType { get; set; } = "";
        public string IPATranscription { get; set; } = "";
    }
}
