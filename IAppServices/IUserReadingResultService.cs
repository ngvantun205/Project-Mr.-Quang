using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IUserReadingResultService {
        Task Add(UserReadingResult entity);
        Task Delete(int id);
        Task<IEnumerable<UserReadingResult>> GetAll();
        Task<UserReadingResult?> GetById(int id);
        Task Update(UserReadingResult entity);
        Task<IEnumerable<UserReadingResult>> GetByUserId(int id);
        Task<IEnumerable<UserReadingResult>> GetByReadingLessonId(int id);
        Task<IEnumerable<UserReadingResult>> GetByUserIdAndLessonId(int userId, int lessonId);
    }
}
