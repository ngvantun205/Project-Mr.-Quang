using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IUserListeningResultService {
        Task<IEnumerable<UserListeningResult>> GetAll();
        Task<UserListeningResult> GetById(int id);
        Task Add(UserListeningResult userListeningResult);
        Task Update(UserListeningResult userListeningResult);
        Task Delete(int id);
        Task<IEnumerable<UserListeningResult>> GetByUserIdAndListeningId(int userid, int listeningid);
    }
}
