using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class SpeakingSentenceService : ISpeakingSentenceService {
        private readonly ISpeakingSentenceRepository _speakingSentenceRepository;
        public SpeakingSentenceService(ISpeakingSentenceRepository speakingSentenceRepository) {
            _speakingSentenceRepository = speakingSentenceRepository;
        }
        public async Task Add(SpeakingSentence sentence) => await _speakingSentenceRepository.Add(sentence);
        public async Task Delete(int id) => await _speakingSentenceRepository.Delete(id);
        public async Task<IEnumerable<SpeakingSentence>> GetAll() => await _speakingSentenceRepository.GetAll();
        public async Task<SpeakingSentence?> GetById(int id) => await _speakingSentenceRepository.GetById(id);
        public async Task<IEnumerable<SpeakingSentence>> GetByTopicId(int topicId) => await _speakingSentenceRepository.GetByTopicId(topicId);
        public async Task Update(SpeakingSentence sentence) => await _speakingSentenceRepository.Update(sentence);
        public async Task AddListAsync(IEnumerable<SpeakingSentence> sentences) => await _speakingSentenceRepository.AddListAsync(sentences);
    }
}
