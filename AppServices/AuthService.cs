using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
            await Task.Delay(100); 
            var user = await _userService.GetByEmail(email);
            if (user != null && user.PasswordHash == password) {
                return user;
            }
            else return null;
        }
    }
}
