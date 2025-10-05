using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDEduEnglish.Data;
using TDEduEnglish.DomainModels;

namespace TDEduEnglish.Repository {
    public class UserScoreRepository : IUserScoreRepository {
        private readonly AppDbContext _context;
        public UserScoreRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(UserScore entity) {
            await _context.UserScores.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int userId) {
            var userScore = await GetByUserId(userId);
            if (userScore != null) {
                _context.UserScores.Remove(userScore);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserScore>> GetAll() => await _context.UserScores.Include(u => u.User).ToListAsync();
        public Task<UserScore?> GetById(int id) {
            throw new NotImplementedException();
        }
        public async Task<UserScore?> GetByUserId(int userId) =>
            await _context.UserScores
                          .Include(u => u.User)
                          .FirstOrDefaultAsync(s => s.UserId == userId);
        public async Task Update(UserScore entity) {
            var existing = await GetByUserId(entity.UserId);
            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
