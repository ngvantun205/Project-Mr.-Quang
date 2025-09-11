using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class VocabularyService : IVocabularyService {
        private readonly IRepository<Vocabulary> _vocabularyRepository;
        public VocabularyService(IRepository<Vocabulary> vocabularyRepository) {
            _vocabularyRepository = vocabularyRepository;
        }
        public async Task<IEnumerable<Vocabulary>> GetAll() => await _vocabularyRepository.GetAll();
        public async Task<Vocabulary?> GetById(int id) => await _vocabularyRepository.GetById(id);
        public async Task Add(Vocabulary vocabulary) => await _vocabularyRepository.Add(vocabulary);
        public async Task Update(Vocabulary vocabulary) => await _vocabularyRepository.Update(vocabulary);
        public async Task Delete(int id) => await _vocabularyRepository.Delete(id);
    }
}
