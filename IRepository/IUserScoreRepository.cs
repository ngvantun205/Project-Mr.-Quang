using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface IUserScoreRepository : IRepository<UserScore> {
        Task<UserScore?> GetByUserId(int userId);
    }
}
