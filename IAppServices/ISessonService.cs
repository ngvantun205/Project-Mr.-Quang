using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ISessonService {
        User? CurrentUser { get; set; }
        Course? CurrentCourse { get; set; }
        Quiz? CurrentQuiz { get; set; }

        void SetCurrentUser(User user);
        User? GetCurrentUser();
        void Logout();  
        bool IsUserLoggedIn();
    }
}
