using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class UserReadingResultService : IUserReadingResultService {
        private readonly IUserReadingResultService _repository;
        public UserReadingResultService(IUserReadingResultService repository) {
            _repository = repository;
        }
        public async Task Add(UserReadingResult entity) =>  await _repository.Add(entity);
        public async Task Delete(int id) =>  await _repository.Delete(id);
        
        public async Task<IEnumerable<UserReadingResult>> GetAll() =>  await _repository.GetAll();
        public async Task<UserReadingResult?> GetById(int id) => await _repository.GetById(id);
        public async Task Update(UserReadingResult entity) =>  await _repository.Update(entity);
        public async Task<IEnumerable<UserReadingResult>> GetByUserId(int id) => await _repository.GetByUserId(id);

        public async Task<IEnumerable<UserReadingResult>> GetByReadingLessonId(int id) => await _repository.GetByReadingLessonId(id);

        public async Task<IEnumerable<UserReadingResult>> GetByUserIdAndLessonId(int userId, int lessonId) => await _repository.GetByUserIdAndLessonId(userId, lessonId);
    }
}
