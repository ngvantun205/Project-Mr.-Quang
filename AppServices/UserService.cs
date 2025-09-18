using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TDEduEnglish.AppServices {
    internal class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetAll() => await _userRepository.GetAll();
        public async Task<User?> GetById(int id) => await _userRepository.GetById(id);
        public async Task Add(User user) => await _userRepository.Add(user);
        public async Task Update(User user) => await _userRepository.Update(user);
        public async Task Delete(int id) => await _userRepository.Delete(id);
        public async Task<User?> GetByEmail(string email) => await _userRepository.GetByEmail(email);
    }
}
