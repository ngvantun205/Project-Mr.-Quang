using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class SessonService : ISessonService {
        public User? CurrentUser { get;  set; }
        public Course? CurrentCourse { get;  set; }
        public Quiz? CurrentQuiz { get;  set; }
        public void SetCurrentUser(User user) {
            CurrentUser = user;
        }
        public void Logout() {
            CurrentUser = null;
        }

        public bool IsUserLoggedIn() {
            return CurrentUser != null;
        }
    }
}
