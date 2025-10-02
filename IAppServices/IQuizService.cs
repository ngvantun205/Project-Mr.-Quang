using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IQuizService {
        Task<IEnumerable<Quiz>> GetAll();
        Task<Quiz> GetById(int id);
        Task Add(Quiz quiz);
        Task Delete(int id);
        Task Update(Quiz quiz);
        Task<IEnumerable<Quiz>> GetByLevel(string  level);  
        Task<IEnumerable<Quiz>> GetByTopic(string  topic);
    }
}
