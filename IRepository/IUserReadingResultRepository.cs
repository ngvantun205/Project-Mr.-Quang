using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IUserReadingResultRepository : IRepository<UserReadingResult> {
        Task<IEnumerable<UserReadingResult>> GetByUserId(int id);
        Task<IEnumerable<UserReadingResult>> GetByReadingLessonId(int id);
    }
}
