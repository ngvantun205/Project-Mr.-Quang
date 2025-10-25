using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface ISpeakingSentenceRepository : IRepository<SpeakingSentence> {
        Task<IEnumerable<SpeakingSentence>> GetByTopicId(int topicId);
    }
}
