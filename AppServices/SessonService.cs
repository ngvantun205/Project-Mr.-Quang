using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TDEduEnglish.AppServices {
    public class SessonService : ISessonService {
        public User? CurrentUser { get;  set; }
        public Quiz? CurrentQuiz { get;  set; }
        public string? CurrentTopic { get; set; }
        public Topic? CurrentSpeakingTopic { get; set; }
        public ReadingLesson? CurrentReading { get; set; }
        public ListeningLesson? CurrentListening { get; set; }
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
        public string? GetCurrentTopic() => CurrentTopic;
        public void SetCurrentTopic(string topic) =>  CurrentTopic = topic;
        public void SetCurrentReading(ReadingLesson reading) => CurrentReading = reading;
        public ReadingLesson? GetCurrentReading() => CurrentReading;
        public void SetCurrentListening(ListeningLesson listening) => CurrentListening = listening;
        public ListeningLesson? GetCurrentListening() => CurrentListening;
        public Quiz? GetCurrentQuiz() => CurrentQuiz;
        public void SetCurrentQuiz(Quiz quiz) => CurrentQuiz = quiz;
        public void SetCurrentSpeakingTopic(Topic topic) => CurrentSpeakingTopic = topic;
    }
}
