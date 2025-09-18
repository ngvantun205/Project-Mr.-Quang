using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class ReadingService : IReadingService {
        private readonly IReadingRepository _readingRepository;
        public ReadingService(IReadingRepository readingRepository) {
            _readingRepository = readingRepository;
        }
        public async Task<IEnumerable<ReadingLesson>> GetAll() => await _readingRepository.GetAll();
        public async Task<ReadingLesson?> GetById(int id) => await _readingRepository.GetById(id);
        public async Task Add(ReadingLesson vocabulary) => await _readingRepository.Add(vocabulary);
        public async Task Update(ReadingLesson vocabulary) => await _readingRepository.Update(vocabulary);
        public async Task Delete(int id) => await _readingRepository.Delete(id);
        public async Task SaveResult(UserReadingResult result) => await _readingRepository.SaveResult(result);
        public async Task AddListAsync(IEnumerable<ReadingLesson> readinglesson) => await _readingRepository.AddListAsync(readinglesson);
        public async Task<IEnumerable<ReadingLesson>> GetByLevel(string level) => await _readingRepository.GetByLevel(level);
    }
}
