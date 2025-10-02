using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface IQuizQuestionRepository : IRepository<QuizQuestion> {
        Task<IEnumerable<QuizQuestion>> GetByQuizId(int quizid);
        Task AddListAsync(IEnumerable<QuizQuestion> quizQuestions);
    }
}
