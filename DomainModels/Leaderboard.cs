using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    internal class Leaderboard : Bindable {
        private int leaderboardid;
        public int LeaderboardId { get => leaderboardid; set => Set(ref leaderboardid, value); }
        private int totalxp;
        public int TotalXP { get => totalxp; set => Set(ref totalxp, value); }
        private int userid;
        public int UserId { get => userid; set => Set(ref userid, value); }
        private int rank;
        public int Rank { get => rank; set => Set(ref rank, value); }
        private DateTime updateddate;
        public DateTime UpdatedDate { get => updateddate; set => Set(ref updateddate, value); }
    }
}
