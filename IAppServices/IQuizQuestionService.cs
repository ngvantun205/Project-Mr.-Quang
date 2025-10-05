using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IQuizQuestionService {
        Task<IEnumerable<QuizQuestion>> GetAll();
        Task<QuizQuestion> GetById(int id);
        Task Add(QuizQuestion quiz);
        Task Delete(int id);
        Task Update(QuizQuestion quiz);
        Task<IEnumerable<QuizQuestion>> GetByQuizId(int quizid);
        Task AddListAsync(IEnumerable<QuizQuestion> quizQuestions);
        Task<IEnumerable<QuizQuestion>> GetUnCompletedOrFlaggedQuestion();
    }
}
