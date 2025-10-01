using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IUserVocabularyService {
        Task<IEnumerable<UserVocabulary>> GetAll();
        Task<UserVocabulary> GetById(int id);
        Task Add(UserVocabulary entity);
        Task Delete(int id);
        Task Update(UserVocabulary entity);
        Task<IEnumerable<UserVocabulary>> GetByUserId(int userId);
    }
}
