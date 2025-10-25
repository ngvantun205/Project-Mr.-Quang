using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class TopicService : ITopicService {
        private readonly ITopicRepository _topicRepository;
        public TopicService(ITopicRepository topicRepository) {
            _topicRepository = topicRepository;
        }
        public async Task Add(Topic topic) =>  await _topicRepository.Add(topic);
        public async Task Delete(int id) =>  await _topicRepository.Delete(id);
        public async Task<IEnumerable<Topic>> GetAll() => await _topicRepository.GetAll();
        public async Task<Topic?> GetById(int id) => await _topicRepository.GetById(id);
        public async Task Update(Topic topic) => await _topicRepository.Update(topic);  
    }
}
