using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IAIChatRepository : IRepository<AIChat> {
        Task<IEnumerable<AIChat>> GetByUserId(int userId);
    }
}
