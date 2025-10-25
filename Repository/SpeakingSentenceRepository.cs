using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class SpeakingSentenceRepository : ISpeakingSentenceRepository {
        private readonly AppDbContext _context;
        public SpeakingSentenceRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(SpeakingSentence entity) {
            await _context.SpeakingSentences.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var sentence = await GetById(id);
            if (sentence != null) {
                _context.SpeakingSentences.Remove(sentence);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<SpeakingSentence>> GetAll() {
            return await _context.SpeakingSentences.ToListAsync();
        }
        public async Task<SpeakingSentence?> GetById(int id) {
            return await _context.SpeakingSentences.FirstOrDefaultAsync(s => s.SpeakingSentenceId == id);
        }
        public async Task<IEnumerable<SpeakingSentence>> GetByTopicId(int topicId) {
            return await _context.SpeakingSentences.Where(s => s.TopicId == topicId).ToListAsync();
        }
        public async Task Update(SpeakingSentence entity) {
            var existingSentence = await GetById(entity.SpeakingSentenceId);
            if (existingSentence != null) {
                _context.Entry(existingSentence).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
