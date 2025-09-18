using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IListeningService : IRepository<ListeningLesson> {
        Task<IEnumerable<ListeningLesson>> GetAll();
        Task<ListeningLesson?> GetById(int id);
        Task Add(ListeningLesson lesson);
        Task Update(ListeningLesson lesson);
        Task Delete(int id);
        Task SaveResult(UserListeningResult result);
        Task AddListAsync(IEnumerable<ListeningLesson> lessons);
        Task<IEnumerable<ListeningLesson>> GetByLevel(string level);
    }
}
