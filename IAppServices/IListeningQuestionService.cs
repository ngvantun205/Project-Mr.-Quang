using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IListeningQuestionService {
        Task<IEnumerable<ListeningQuestion>> GetAll();
        Task<ListeningQuestion?> GetById(int id);
        Task Add(ListeningQuestion question);
        Task Delete(int id);
        Task Update(ListeningQuestion question);
        Task<IEnumerable<ListeningQuestion>> GetByListeningId(int id);
        Task AddListAsync(IEnumerable<ListeningQuestion> listeningQuestions);
    }
}
