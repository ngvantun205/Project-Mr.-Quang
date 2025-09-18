using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IReadingRepository : IRepository<ReadingLesson> {
        Task SaveResult(UserReadingResult result);
        Task AddListAsync(IEnumerable<ReadingLesson> lessons);
        Task<IEnumerable<ReadingLesson>> GetByLevel(string level);
    }
}
