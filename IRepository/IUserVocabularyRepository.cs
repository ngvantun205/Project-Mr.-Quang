using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface IUserVocabularyRepository : IRepository<UserVocabulary> {
        //GetAll, GetBYId, Add, Delete, Update
        Task<IEnumerable<UserVocabulary>> GetByUserId(int id);
    }
}
