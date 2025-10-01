using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IVocabularyService {
        Task<IEnumerable<Vocabulary>> GetAll();
        Task<Vocabulary?> GetById(int id);
        Task Add(Vocabulary vocabulary);
        Task Update(Vocabulary vocabulary);
        Task Delete(int id);
        Task<IEnumerable<Vocabulary>> GetByLevelTopic(string level, string topic);
        Task AddListAsync(IEnumerable<Vocabulary> vocabularies);
        Task<Vocabulary?> GetByWord(string word);
    }
}
