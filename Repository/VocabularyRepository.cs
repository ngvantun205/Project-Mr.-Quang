using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class VocabularyRepository : IRepository<Vocabulary> {
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
    }
}
