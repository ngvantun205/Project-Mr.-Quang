using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class AuthService : IAuthService {
        private readonly IUserService _userService;
        public AuthService(IUserService userSevice) {
            _userService = userSevice;
        }
        public async Task<User?> Login(string email, string password) {
            // Dummy implementation for example purposes
            await Task.Delay(100); // Simulate async work
            var user = await _userService.GetByEmail(email);
             if (user == null || (user != null && user.PasswordHash != password)) return null;
            else {
                return user;
            }
        }
    }
}
