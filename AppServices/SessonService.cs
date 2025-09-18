using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class SessonService : ISessonService {
        public User? CurrentUser { get;  set; }
        public Course? CurrentCourse { get;  set; }
        public Quiz? CurrentQuiz { get;  set; }
        public string? CurrentTopic { get; set; }
        public void SetCurrentUser(User user) {
            CurrentUser = user;
        }
        public void Logout() {
            CurrentUser = null;
        }
        public User? GetCurrentUser() {
            return CurrentUser;
        }

        public bool IsUserLoggedIn() {
            return CurrentUser != null;
        }
        public string GetCurrentTopic() {
            return CurrentTopic ?? "";
        }
        public void SetCurrentTopic(string topic) {
            CurrentTopic = topic;
        }
    }
}
