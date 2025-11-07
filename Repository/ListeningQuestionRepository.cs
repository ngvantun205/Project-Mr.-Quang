using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class ListeningQuestionRepository : IListeningQuestionRepository {
        private readonly AppDbContext _context;
        public ListeningQuestionRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(ListeningQuestion entity) {
            await _context.ListeningQuestions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddListAsync(IEnumerable<ListeningQuestion> list) {
           await _context.ListeningQuestions.AddRangeAsync(list);
           await _context.SaveChangesAsync();
        }

        public async Task Delete(int id) {
           var lq = await GetById(id);
            if(lq != null) {
                _context.ListeningQuestions.Remove(lq);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ListeningQuestion>> GetAll() {
            return await _context.ListeningQuestions.ToListAsync();
        }

        public async Task<ListeningQuestion?> GetById(int id) {
            return await _context.ListeningQuestions.FindAsync(id);
        }

        public async Task<IEnumerable<ListeningQuestion>> GetByListeningId(int id) {
            return await _context.ListeningQuestions.Where(questions => questions.ListeningLessonId == id).ToListAsync();
        }

        public async Task Update(ListeningQuestion entity) {
           var question = await GetById(entity.ListeningQuestionId);
            if(question != null) {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
