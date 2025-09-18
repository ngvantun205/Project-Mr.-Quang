using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class AnswerOption {
        [Key]
        public int OptionId { get; set; }
        public string OptionText { get; set; } = "";
        public bool IsSelected { get; set; } = false;
    }
}
