using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class UserReadingResultRepository : IUserReadingResultRepository {
        private readonly AppDbContext _context;
        public UserReadingResultRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(UserReadingResult entity) {
            _context.UserReadingResults.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var entity = _context.UserReadingResults.Find(id);
            if (entity != null) {
                _context.UserReadingResults.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserReadingResult>> GetAll() {
            return await Task.FromResult(_context.UserReadingResults.ToList());
        }
        public async Task<UserReadingResult?> GetById(int id) {
            return await Task.FromResult(_context.UserReadingResults.Find(id));
        }
        public async Task Update(UserReadingResult entity) {
            var existing = _context.UserReadingResults.Find(entity.UserReadingResultId);
            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserReadingResult>> GetByUserId(int userid) {
            return await _context.UserReadingResults.Where(result => result.UserId == userid).ToListAsync();
        }
        public async Task<IEnumerable<UserReadingResult>> GetByReadingLessonId(int lessonid) {
            return await _context.UserReadingResults.Where(result => result.ReadingLessonId == lessonid).ToListAsync();
        }
        public async Task<IEnumerable<UserReadingResult>> GetByUserIdAndLessonId(int userId, int lessonId) {
            return await _context.UserReadingResults
                .Where(result => result.UserId == userId && result.ReadingLessonId == lessonId)
                .ToListAsync();
        }

    }
}
