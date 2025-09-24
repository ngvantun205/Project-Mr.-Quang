using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface IUserReadingResultService {
        Task Add(UserReadingResult entity);
        Task Delete(int id);
        Task<IEnumerable<UserReadingResult>> GetAll();
        Task<UserReadingResult?> GetById(int id);
        Task Update(UserReadingResult entity);
    }
}
