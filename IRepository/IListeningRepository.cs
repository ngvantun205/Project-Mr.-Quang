using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IListeningRepository : IRepository<ListeningLesson> {
        Task SaveResult(UserListeningResult result);
        Task AddListAsync(IEnumerable<ListeningLesson> lessons);
        Task<IEnumerable<ListeningLesson>> GetByLevel(string level);
    }
}
