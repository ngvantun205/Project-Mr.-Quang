using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TDEduEnglish.Data;
using TDEduEnglish.DomainModels;
using TDEduEnglish.IRepository;

namespace TDEduEnglish.Repository {
    internal class UserRepository : IUserRepository{
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAll() {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetById(int id) {
            return await _context.Users.FindAsync(id);
        }
        public async Task Add(User entity) {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(User entity) {
            var user = await GetById(entity.UserId);
            if (user != null) {
                user.FullName = entity.FullName;
                user.Email = entity.Email;
                user.PasswordHash = entity.PasswordHash;
                user.Role = entity.Role;
                user.JoinDate = entity.JoinDate;
                user.Level = entity.Level;
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id) {
            var user = await GetById(id);
            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User?> GetByEmail(string email) {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
