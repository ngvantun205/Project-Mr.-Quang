using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IUserScoreService {
        Task<UserScore?> GetByUserId(int userId);
        Task<IEnumerable<UserScore>> GetAll();
    }
}
