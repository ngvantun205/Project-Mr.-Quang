using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IReadingService {
        Task<IEnumerable<ReadingLesson>> GetAll();
        Task<ReadingLesson?> GetById(int id);
        Task Add(ReadingLesson vocabulary);
        Task Update(ReadingLesson vocabulary);
        Task Delete(int id);
        Task SaveResult(UserReadingResult result);
        Task AddListAsync(IEnumerable<ReadingLesson> readinglesson);
        Task<IEnumerable<ReadingLesson>> GetByLevel(string level);
    }
}
