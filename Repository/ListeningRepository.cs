using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class ListeningRepository : IListeningRepository {
        private readonly AppDbContext _context;
        public ListeningRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<ListeningLesson>> GetAll() {
            return await _context.ListeningLessons
                .Include(r => r.Questions)
                .ToListAsync();
        }

        public async Task<ListeningLesson?> GetById(int id) {
            return await _context.ListeningLessons
                .Include(r => r.Questions)
                .FirstOrDefaultAsync(r => r.ListeningLessonId == id);
        }

        public async Task Add(ListeningLesson entity) {
            await _context.ListeningLessons.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(ListeningLesson entity) {
            var existing = await GetById(entity.ListeningLessonId);
            if (existing != null) {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id) {
            var lesson = await GetById(id);
            if (lesson != null) {
                _context.ListeningLessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveResult(UserListeningResult result) {
            await _context.UserListeningResults.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        public async Task AddListAsync(IEnumerable<ListeningLesson> lessons) {
            await _context.ListeningLessons.AddRangeAsync(lessons);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ListeningLesson>> GetByLevel(string level) {
            return await _context.ListeningLessons.Where(r => r.Level == level).ToListAsync();
        }
    }
}
