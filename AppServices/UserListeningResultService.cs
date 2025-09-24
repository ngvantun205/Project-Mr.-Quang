using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class UserListeningResultService : IUserListeningResultService {
        private readonly IUserListeningResultRepository _userListeningResultRepository;
        public UserListeningResultService(IUserListeningResultRepository userListeningResultRepository) {
            _userListeningResultRepository = userListeningResultRepository;
        }
        public async Task<IEnumerable<UserListeningResult>> GetAll() => await _userListeningResultRepository.GetAll();
        public async Task<UserListeningResult?> GetById(int id) => await _userListeningResultRepository.GetById(id);
        public async Task Add(UserListeningResult result ) => await _userListeningResultRepository.Add(result);
        public async Task Delete(int id) => await _userListeningResultRepository.Delete(id);
        public async Task Update(UserListeningResult result) => await _userListeningResultRepository.Update(result);
        public async Task<IEnumerable<UserListeningResult>> GetByUserIdAndListeningId(int userid, int listeningid) => await _userListeningResultRepository.GetByUserIdAndListeningId(userid, listeningid);
    }
}
