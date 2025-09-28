using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IAIChatService {
        Task<IEnumerable<AIChat>> GetAll();
        Task<AIChat> GetById(int id);
        Task Add(AIChat chat);
        Task Delete(int id);
        Task Update(AIChat chat);
        Task<IEnumerable<AIChat>> GetByUserId(int userid);
    }
}
