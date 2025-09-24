using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class UserListeningResultRepository : IUserListeningResultRepository {
        private readonly AppDbContext _context;
        public UserListeningResultRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<UserListeningResult>> GetAll() {
            return await _context.UserListeningResults.ToListAsync();
        }
        public async Task<UserListeningResult?> GetById(int id) {
            return await _context.UserListeningResults.FindAsync(id);
        }
        public async Task Add(UserListeningResult entity) {
            await _context.UserListeningResults.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var result = await GetById(id);
            if(result != null) {
                _context.UserListeningResults.Remove(result);
                await _context.SaveChangesAsync();  
            }
        }
        public async Task Update(UserListeningResult entity) {
            var result = await GetById(entity.UserListeningResultId);
            if(result != null) {
                result.UserId = entity.UserId;
                result.ListeningLessonId = entity.ListeningLessonId;
                result.Score = entity.Score;
                result.CompletedAt = entity.CompletedAt;
                await _context.SaveChangesAsync();
            }
        }
       

        public async Task<IEnumerable<UserListeningResult>> GetByUserIdAndListeningId(int userid, int listeningid) {
            return await _context.UserListeningResults.Where(result => result.UserId == userid & result.ListeningLessonId == listeningid).ToListAsync();
        }
    }
}
