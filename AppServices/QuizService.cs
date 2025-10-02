using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class QuizService : IQuizService {
        private readonly IQuizRepository _quizRepository;
        public QuizService(IQuizRepository quizRepository) {
            _quizRepository = quizRepository;
        }

        public async Task Add(Quiz quiz) => await _quizRepository.Add(quiz);
        public async Task Delete(int id) => await _quizRepository.Delete(id);
        public async Task<IEnumerable<Quiz>> GetAll() => await _quizRepository.GetAll();
        public async Task<Quiz> GetById(int id) => await GetById(id);
        public async Task<IEnumerable<Quiz>> GetByLevel(string level) => await GetByLevel(level);
        public async Task<IEnumerable<Quiz>> GetByTopic(string topic) => await GetByTopic(topic);
        public async Task Update(Quiz quiz) => await _quizRepository.Update(quiz);
    }
}
