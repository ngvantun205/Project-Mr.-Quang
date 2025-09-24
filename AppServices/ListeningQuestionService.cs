using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class ListeningQuestionService : IListeningQuestionService {
        private readonly IListeningQuestionRepository _listeningQuestionRepository;
        public ListeningQuestionService(IListeningQuestionRepository listeningQuestionRepository) {
            _listeningQuestionRepository = listeningQuestionRepository;
        }
        public async Task<IEnumerable<ListeningQuestion>> GetAll() => await _listeningQuestionRepository.GetAll();
        public async Task<ListeningQuestion?> GetById(int id) => await _listeningQuestionRepository.GetById(id);
        public async Task Add(ListeningQuestion question) => await _listeningQuestionRepository.Add(question);
        public async Task Delete(int id) => await _listeningQuestionRepository.Delete(id);
        public async Task Update(ListeningQuestion question) => await _listeningQuestionRepository.Update(question);
        public async Task<IEnumerable<ListeningQuestion>> GetByListeningId(int id) => await _listeningQuestionRepository.GetByListeningId(id);
        public async Task AddListAsync(IEnumerable<ListeningQuestion> list) => await _listeningQuestionRepository.AddListAsync(list);
    }
}
