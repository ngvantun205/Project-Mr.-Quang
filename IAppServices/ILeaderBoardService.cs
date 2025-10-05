using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ILeaderBoardService {
        Task<bool> SubmitAttemptAsync(int userId, int exerciseId, string exerciseType, int newScore, int maxScore);
    }
}
