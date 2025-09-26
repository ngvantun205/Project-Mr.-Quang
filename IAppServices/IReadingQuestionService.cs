using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IReadingQuestionService {
        Task AddListAsync(IEnumerable<ReadingQuestion> questions);
        Task<IEnumerable<ReadingQuestion>> GetByLessonId(int lessonId);
        Task<IEnumerable<ReadingQuestion>> GetAll();
        Task<ReadingQuestion?> GetById(int id); 
        Task Add(ReadingQuestion entity);
        Task Update(ReadingQuestion entity);
        Task Delete(int id);
    }
}
