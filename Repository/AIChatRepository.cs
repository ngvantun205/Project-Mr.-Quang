using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class AIChatRepository : IAIChatRepository {
        private readonly AppDbContext _context;
        public AIChatRepository(AppDbContext context) {
            _context = context;
        }
        public async Task Add(AIChat chat) {
            await _context.AIChats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var chat = await GetById(id);
            if(chat != null) _context.AIChats.Remove(chat);
            await _context.SaveChangesAsync();
        }
        public async Task<AIChat?> GetById(int id) {
            return await _context.AIChats.FindAsync(id);
        }
        public async Task<IEnumerable<AIChat>> GetAll() {
            return await _context.AIChats.ToListAsync();
        }
        public async Task Update(AIChat chat) {
            var existingchat = await GetById(chat.ChatId);
            if(existingchat != null) {
                existingchat.Message = chat.Message;
                existingchat.Response = chat.Response;
                existingchat.SendDate = chat.SendDate;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<AIChat>> GetByUserId(int id) {
            return await _context.AIChats.Where(chat => chat.UserId == id).ToListAsync();
        }
    }
}
