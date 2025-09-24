using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IListeningQuestionRepository : IRepository<ListeningQuestion> {
        Task AddListAsync(IEnumerable<ListeningQuestion> list);
        Task<IEnumerable<ListeningQuestion>> GetByListeningId(int id);
    }
}
