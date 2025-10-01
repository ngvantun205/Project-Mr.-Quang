using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;
using TDEduEnglish.IRepository;

namespace TDEduEnglish.Repository {
    internal class VocabularyRepository : IVocabularyRepository {
        private readonly AppDbContext _context;
        public VocabularyRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Vocabulary>> GetAll() => await _context.Vocabularies.ToListAsync();
        public async Task<Vocabulary?> GetById(int id) => await _context.Vocabularies.FindAsync(id);
        public async Task Add(Vocabulary entity) => await _context.Vocabularies.AddAsync(entity);
        public async Task Update(Vocabulary entity) {
            var vocabulary = await GetById(entity.VocabularyId);
            if (vocabulary != null) {
                vocabulary.Word = entity.Word;
                vocabulary.Meaning = entity.Meaning;
                vocabulary.ExampleSentence = entity.ExampleSentence;
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id) {
            var vocabulary = await GetById(id);
            if (vocabulary != null) {
                _context.Vocabularies.Remove(vocabulary);
                await _context.SaveChangesAsync();
            }
        }
        public Task<IEnumerable<Vocabulary>> GetByLevelTopic(string level, string topic) {
            var vocabularies = _context.Vocabularies.Where(v => v.Level == level && v.Topic == topic);
            return Task.FromResult(vocabularies.AsEnumerable());
        }
        public async Task AddListAsync(IEnumerable<Vocabulary> vocabularies) {
            await _context.Vocabularies.AddRangeAsync(vocabularies);
            await _context.SaveChangesAsync();
        }
        public async Task<Vocabulary?> GetByWord(string word) {
            return await _context.Vocabularies.FirstOrDefaultAsync(v =>  v.Word.ToLower() == word.ToLower());
        }
    }
}
