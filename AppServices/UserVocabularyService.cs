using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices
{
    public class UserVocabularyService : IUserVocabularyService {
        private readonly IUserVocabularyRepository _userVocabularyRepository;
        public UserVocabularyService(IUserVocabularyRepository userVocabularyRepository) {
            _userVocabularyRepository = userVocabularyRepository;
        }
        public async Task Add(UserVocabulary entity) => await _userVocabularyRepository.Add(entity);
        public async Task Delete(int id) => await _userVocabularyRepository.Delete(id);
        public async Task<IEnumerable<UserVocabulary>> GetAll() => await _userVocabularyRepository.GetAll();
        public async Task<UserVocabulary?> GetById(int id) => await _userVocabularyRepository.GetById(id);
        public async Task<IEnumerable<UserVocabulary>> GetByUserId(int userId) => await _userVocabularyRepository.GetByUserId(userId);
        public async Task Update(UserVocabulary entity) => await Update(entity);
    }
}
