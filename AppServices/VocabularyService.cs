using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TDEduEnglish.AppServices {
    internal class VocabularyService : IVocabularyService {
        private readonly IVocabularyRepository _vocabularyRepository;
        public VocabularyService(IVocabularyRepository vocabularyRepository) {
            _vocabularyRepository = vocabularyRepository;
        }
        public async Task<IEnumerable<Vocabulary>> GetAll() => await _vocabularyRepository.GetAll();
        public async Task<Vocabulary?> GetById(int id) => await _vocabularyRepository.GetById(id);
        public async Task Add(Vocabulary vocabulary) => await _vocabularyRepository.Add(vocabulary);
        public async Task Update(Vocabulary vocabulary) => await _vocabularyRepository.Update(vocabulary);
        public async Task Delete(int id) => await _vocabularyRepository.Delete(id);
        public async Task<IEnumerable<Vocabulary>> GetByLevelTopic(string level, string topic) => await _vocabularyRepository.GetByLevelTopic(level, topic);
        public async Task AddListAsync(IEnumerable<Vocabulary> vocabularies) => await _vocabularyRepository.AddListAsync(vocabularies);
    }
}
