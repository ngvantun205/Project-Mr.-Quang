using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IRepository {
    public interface IUserAttemptRepository : IRepository<UserAttempt> {
        Task<IEnumerable<UserAttempt>> GetAttemptsToday(int userId, int exerciseId, string exerciseType);
        Task<IEnumerable<UserAttempt>> GetAttemptsByUser(int userId); 
    }
}
