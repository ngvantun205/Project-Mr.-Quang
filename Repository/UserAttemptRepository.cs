using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDEduEnglish.Data;
using TDEduEnglish.DomainModels;

namespace TDEduEnglish.Repository {
    public class UserAttemptRepository : IUserAttemptRepository {
        private readonly AppDbContext _context;
        public UserAttemptRepository(AppDbContext context) {
            _context = context;
        }

        public async Task Add(UserAttempt entity) {
            await _context.UserAttempts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<UserAttempt>> GetAttemptsToday(int userId, int exerciseId, string exerciseType) {
            var today = System.DateTime.Today;
            return await _context.UserAttempts
                .Where(a => a.UserId == userId &&
                            a.ExerciseId == exerciseId &&
                            a.ExerciseType == exerciseType &&
                            a.AttemptDate.Date == today)
                .ToListAsync();
        }
        public async Task Delete(int attemptId) {
            var attempt = await _context.UserAttempts.FindAsync(attemptId);
            if (attempt != null) {
                _context.UserAttempts.Remove(attempt);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(UserAttempt entity) {
            var existing = await GetById(entity.UserAttemptId);
            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserAttempt>> GetAll() => await _context.UserAttempts.Include(a => a.User).ToListAsync();
        public async Task<IEnumerable<UserAttempt>> GetAttemptsByUser(int userId) => await _context.UserAttempts.Where(a => a.UserId == userId).ToListAsync();
        public async Task<UserAttempt?> GetById(int id) => await _context.UserAttempts.Include(a => a.User).FirstOrDefaultAsync(a => a.UserAttemptId == id);
    }
}



