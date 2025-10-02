using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface IQuizRepository : IRepository<Quiz> {
        Task<IEnumerable<Quiz>> GetByLevel(string level);
        Task<IEnumerable<Quiz>> GetByTopic(string topic);
        Task AddListAsync(IEnumerable<Quiz> quizzes);
    }
}
