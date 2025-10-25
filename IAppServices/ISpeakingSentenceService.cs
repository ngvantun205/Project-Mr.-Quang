using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ISpeakingSentenceService {
        Task Add(SpeakingSentence sentence);
        Task Update(SpeakingSentence sentence);
        Task Delete(int id);
        Task<SpeakingSentence?> GetById(int id);
        Task<IEnumerable<SpeakingSentence>> GetAll();
        Task<IEnumerable<SpeakingSentence>> GetByTopicId(int topicId);
    }
}
