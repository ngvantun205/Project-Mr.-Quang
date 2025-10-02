using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class QuizQuestionService : IQuizQuestionService {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        public  QuizQuestionService(IQuizQuestionRepository quizQuestionRepository) {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task Add(QuizQuestion quiz) => await _quizQuestionRepository.Add(quiz);
        public async Task Delete(int id) => await _quizQuestionRepository.Delete(id);
        public async Task<IEnumerable<QuizQuestion>> GetAll() => await _quizQuestionRepository.GetAll();
        public async Task<QuizQuestion?> GetById(int id) => await _quizQuestionRepository.GetById(id);
        public async Task<IEnumerable<QuizQuestion>> GetByQuizId(int quizid) => await _quizQuestionRepository.GetByQuizId(quizid);
        public async Task Update(QuizQuestion quiz) => await _quizQuestionRepository.Update(quiz);
        public async Task AddListAsync(IEnumerable<QuizQuestion> quizQuestions) => await _quizQuestionRepository.AddListAsync(quizQuestions);
    }
}
