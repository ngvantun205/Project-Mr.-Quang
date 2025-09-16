using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Leaderboard : Bindable {
        [Key]
        public int LeaderboardId { get; set; }
        public int TotalXP { get; set; }
        public int UserId { get; set; }
        public int Rank { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
