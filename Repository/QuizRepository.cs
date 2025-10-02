using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class QuizRepository : IQuizRepository {
        private readonly AppDbContext _context;
        public QuizRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Quiz>> GetAll() => await _context.Quizzes.ToListAsync();
        public async Task<Quiz?> GetById(int id) => await _context.Quizzes.FindAsync(id);
        public async Task Add(Quiz entity) {
            await _context.Quizzes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var q = await GetById(id);
            if(q != null) {
                _context.Quizzes.Remove(q);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(Quiz entity) {
            var q = await GetById(entity.QuizId);
            if(q != null) {
                q.Title = entity.Title;
                q.Level = entity.Level;
                q.Topic = entity.Topic;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Quiz>> GetByLevel(string level) => await _context.Quizzes.Where(x => x.Level == level).ToListAsync();
        public async Task<IEnumerable<Quiz>> GetByTopic(string topic) => await _context.Quizzes.Where(q => q.Topic == topic).ToListAsync(); 
        public async Task AddListAsync(IEnumerable<Quiz> quizzes) {
            await _context.Quizzes.AddRangeAsync(quizzes);
            await _context.SaveChangesAsync();
        }
    }
}
