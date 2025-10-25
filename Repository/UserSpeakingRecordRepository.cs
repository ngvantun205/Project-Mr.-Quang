using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class UserSpeakingRecordRepository : IUserSpeakingRecordRepository {
        private readonly AppDbContext _context;
        public UserSpeakingRecordRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(UserSpeakingRecord entity) {
            await _context.UserSpeakingRecords.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
           var record = await GetById(id);
              if (record != null) {
                 _context.UserSpeakingRecords.Remove(record);
                 await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserSpeakingRecord>> GetAll() {
            return await _context.UserSpeakingRecords.ToListAsync();
        }
        public async Task<UserSpeakingRecord?> GetById(int id) {
            return await _context.UserSpeakingRecords.FindAsync(id);
        }
        public async Task<IEnumerable<UserSpeakingRecord>> GetAllByUserId(int userId) {
            return await _context.UserSpeakingRecords
                .Where(record => record.UserId == userId)
                .ToListAsync();
        }
        public async Task Update(UserSpeakingRecord entity) {
            var existingRecord = await GetById(entity.UserSpeakingRecordId);
            if (existingRecord != null) {
                _context.Entry(existingRecord).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
