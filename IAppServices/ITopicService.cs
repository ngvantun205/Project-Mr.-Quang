using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ITopicService {
        Task Add(Topic topic);
        Task Update(Topic topic);
        Task Delete(int id);
        Task<Topic?> GetById(int id);
        Task<IEnumerable<Topic>> GetAll(); 
        Task<IEnumerable<Topic>> GetByLevel(string level);
        Task AddListAsync(IEnumerable<Topic> topicList);
    }
}
