using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class ListeningService : IListeningService {
        private readonly IListeningRepository _listeningRepository;
        public ListeningService(IListeningRepository listeningRepository) {
            this._listeningRepository = listeningRepository;
        }
        public async Task<IEnumerable<ListeningLesson>> GetAll() => await _listeningRepository.GetAll();
        public async Task<ListeningLesson?> GetById(int id) => await _listeningRepository.GetById(id);
        public async Task Add(ListeningLesson lesson) => await _listeningRepository.Add(lesson);
        public async Task Update(ListeningLesson lesson) => await _listeningRepository.Update(lesson);
        public async Task Delete(int id) => await _listeningRepository.Delete(id);
        public async Task SaveResult(UserListeningResult u) => await _listeningRepository.SaveResult(u);
        public async Task AddListAsync(IEnumerable<ListeningLesson> list) => await _listeningRepository.AddListAsync(list);
        public async Task<IEnumerable<ListeningLesson>> GetByLevel(string level) => await _listeningRepository.GetByLevel(level);
    }
}
