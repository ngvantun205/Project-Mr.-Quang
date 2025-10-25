using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class TopicRepository : ITopicRepository {
        private readonly AppDbContext _context;
        public TopicRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(Topic entity) {
            await _context.Topics.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var topic = await GetById(id);
            if (topic != null) {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Topic>> GetAll() {
            return await Task.FromResult(_context.Topics.ToList());
        }
        public async Task<Topic?> GetById(int id) {
            return await Task.FromResult(_context.Topics.FirstOrDefault(t => t.TopicId == id));
        }
        public async Task Update(Topic entity) {
            var exsistingtopic = await GetById(entity.TopicId);
            if (exsistingtopic != null) {
                _context.Entry(exsistingtopic).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
