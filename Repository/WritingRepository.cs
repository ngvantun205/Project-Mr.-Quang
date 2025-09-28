using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class WritingRepository : IWritingRepository {
        private readonly AppDbContext _context;
        public WritingRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Writing>> GetAll() {
            return await _context.Writings.ToListAsync();
        }
        public async Task<Writing?> GetById(int id) {
            return await _context.Writings.FindAsync(id);
        }
        public async Task Add(Writing writing) {
            await _context.Writings.AddAsync(writing);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var writing = await GetById(id);    
            if(writing != null) {
                _context.Writings.Remove(writing);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(Writing writing) {
            var existingwriting = await GetById(writing.WritingId);   
            if(existingwriting != null) {
                existingwriting.Text = writing.Text;
                existingwriting.Feedback = writing.Feedback;
                existingwriting.SubmitDate = writing.SubmitDate;
                existingwriting.Score = writing.Score;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Writing>> GetByUserId(int userid) {
            return await _context.Writings.Where(writing => writing.UserId == userid).ToListAsync();
        }
    }
}
