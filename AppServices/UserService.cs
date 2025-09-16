using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TDEduEnglish.AppServices {
    internal class UserService : IUserService {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository) {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetAll() => await _userRepository.GetAll();
        public async Task<User?> GetById(int id) => await _userRepository.GetById(id);
        public async Task Add(User user) => await _userRepository.Add(user);
        public async Task Update(User user) => await _userRepository.Update(user);
        public async Task Delete(int id) => await _userRepository.Delete(id);
        public async Task<User?> GetByEmail(string email) {
            var users = await _userRepository.GetAll();
            return users.FirstOrDefault(u => u.Email == email);
        }
    }
}
