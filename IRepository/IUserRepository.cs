using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    internal interface IUserRepository : IRepository<User> {
        Task<User?> GetByEmail(string email);
    }
}
