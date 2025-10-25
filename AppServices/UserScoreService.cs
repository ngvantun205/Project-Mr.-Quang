using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class UserScoreService : IUserScoreService {
        private readonly IUserScoreRepository _scoreRepo;
        public UserScoreService(IUserScoreRepository scoreRepo) {
            _scoreRepo = scoreRepo;
        }
        public async Task<UserScore?> GetByUserId(int userId) => await _scoreRepo.GetByUserId(userId);
        public async Task<IEnumerable<UserScore>> GetAll() => await _scoreRepo.GetAll();
    }
}
