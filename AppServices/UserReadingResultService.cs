using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class UserReadingResultService : IUserReadingResultService {
        private readonly IRepository<UserReadingResult> _repository;
        public UserReadingResultService(IRepository<UserReadingResult> repository) {
            _repository = repository;
        }
        public async Task Add(UserReadingResult entity) =>  await _repository.Add(entity);
        public async Task Delete(int id) =>  await _repository.Delete(id);
        
        public async Task<IEnumerable<UserReadingResult>> GetAll() =>  await _repository.GetAll();
        public async Task<UserReadingResult?> GetById(int id) => await _repository.GetById(id);
        public async Task Update(UserReadingResult entity) =>  await _repository.Update(entity);
    }
}
