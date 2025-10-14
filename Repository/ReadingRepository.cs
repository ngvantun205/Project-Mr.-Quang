using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class ReadingRepository : IReadingRepository {
        private readonly AppDbContext _context;
        public ReadingRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<ReadingLesson>> GetAll() {
            return await _context.ReadingLessons
                .Include(r => r.Questions)
                .ToListAsync();
        }

        public async Task<ReadingLesson?> GetById(int id) {
            return await _context.ReadingLessons
                .Include(r => r.Questions)
                .FirstOrDefaultAsync(r => r.ReadingLessonId == id);
        }

        public async Task Add(ReadingLesson entity) {
            await _context.ReadingLessons.AddAsync(entity);
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task Update(ReadingLesson entity) {
            var lesson = await GetById(entity.ReadingLessonId);
            if (lesson != null) {
                lesson.Title = entity.Title;
                lesson.Content = entity.Content;
                lesson.Questions = entity.Questions;
                lesson.Level = entity.Level;
                lesson.SuggestedTime = entity.SuggestedTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id) {
            var lesson = await GetById(id);
            if (lesson != null) {
                _context.ReadingLessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveResult(UserReadingResult result) {
            await _context.UserReadingResults.AddAsync(result);
            await _context.SaveChangesAsync();
        }
        public async Task AddListAsync(IEnumerable<ReadingLesson> lessons) {
            await _context.ReadingLessons.AddRangeAsync(lessons);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ReadingLesson>> GetByLevel(string level) {
            return await _context.ReadingLessons.Where(r => r.Level == level).ToListAsync();
        }
    }
}
