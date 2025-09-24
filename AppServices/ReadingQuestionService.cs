using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TDEduEnglish.AppServices {
    internal class ReadingQuestionService : IReadingQuestionService {
        private readonly IReadingQuestionRepository _readingQuestionRepository;

        public ReadingQuestionService(IReadingQuestionRepository readingQuestionRepository) {
            _readingQuestionRepository = readingQuestionRepository;
        }
        public async Task AddListAsync(IEnumerable<ReadingQuestion> questions) => await _readingQuestionRepository.AddListAsync(questions);
        public async Task<IEnumerable<ReadingQuestion>> GetByLessonId(int lessonId) => await _readingQuestionRepository.GetByLessonId(lessonId);
        public async Task<IEnumerable<ReadingQuestion>> GetAll() => await _readingQuestionRepository.GetAll();
        public async Task<ReadingQuestion?> GetById(int id) => await _readingQuestionRepository.GetById(id);
        public async Task Add(ReadingQuestion entity) => await _readingQuestionRepository.Add(entity);
        public async Task Update(ReadingQuestion entity) => await _readingQuestionRepository.Update(entity);
        public async Task Delete(int id) => await _readingQuestionRepository.Delete(id);
    } 
}
