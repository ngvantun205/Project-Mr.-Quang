using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface IWritingService {
        Task<IEnumerable<Writing>> GetAll();
        Task<Writing> GetById(int id);
        Task Add(Writing writing);
        Task Delete(int id);
        Task Update(Writing writing);
        Task<IEnumerable<Writing>> GetByUserId(int userId);
        Task<string?> GenerateTextAsync(string message, string userlevel, string writingtask);
    }
}
