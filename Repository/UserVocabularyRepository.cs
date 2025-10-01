using Json.Schema.Generation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class UserVocabularyRepository : IUserVocabularyRepository {
        private readonly AppDbContext _context;
        public UserVocabularyRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<UserVocabulary>> GetAll() {
            return await _context.UserVocabularies.ToListAsync();
        }
        public async Task<UserVocabulary?> GetById(int id) {
            return await _context.UserVocabularies.FindAsync(id);
        }
        public async Task Add(UserVocabulary entity) {
            await _context.UserVocabularies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var uservocab = await GetById(id);
            if (uservocab != null) {
                _context.UserVocabularies.Remove(uservocab);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(UserVocabulary entity) {
            var existing = await GetById(entity.UserVocabularyId);
            if (existing != null) {
                existing.Word = entity.Word;
                existing.IPATranscription = entity.IPATranscription;
                existing.WordType = entity.WordType;
                existing.Meaning = entity.Meaning;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<UserVocabulary>> GetByUserId(int id) => await _context.UserVocabularies.Where(x => x.UserId == id).ToListAsync();
    }
}
