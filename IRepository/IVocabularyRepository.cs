using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IVocabularyRepository : IRepository<Vocabulary> {
        Task<IEnumerable<Vocabulary>> GetByLevelTopic(string level, string topic);
        Task AddListAsync(IEnumerable<Vocabulary> vocabularies);   
        Task<Vocabulary?> GetByWord(string word);
    }
}
