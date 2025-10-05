using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class UserScore {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public int AllTimeScore { get; set; } = 0;
        public int ThisMonthScore { get; set; } = 0;
        public int ThisWeekScore { get; set; } = 0;
        public virtual User User { get; set; } = null!;
    }
}
