using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class AuthService : IAuthService {
        public AuthService() { }
        public async Task<User?> Login(string email, string password) {
            // Dummy implementation for example purposes
            await Task.Delay(100); // Simulate async work
            if (email == "user1@gmail.com" && password == "123456") {
                return new User { Email = email, FullName = "Test User" };
            }
            return null;
        }
    }
}
