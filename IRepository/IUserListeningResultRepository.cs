using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IUserListeningResultRepository : IRepository<UserListeningResult> {
        Task<IEnumerable<UserListeningResult>> GetByUserIdAndListeningId(int userid, int listeningId);
    }
}
