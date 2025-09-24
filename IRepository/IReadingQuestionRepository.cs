using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IReadingQuestionRepository : IRepository<ReadingQuestion> {
        Task AddListAsync(IEnumerable<ReadingQuestion> questions);    
        Task<IEnumerable<ReadingQuestion>> GetByLessonId(int lessonId);
    }
}
