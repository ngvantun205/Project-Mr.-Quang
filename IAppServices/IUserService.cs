using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.DomainModels;

namespace TDEduEnglish.IAppServices {
    public interface IUserService {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(int id);

    }
}
