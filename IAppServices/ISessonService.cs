using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ISessonService {
        User? CurrentUser { get; set; }
        Quiz? CurrentQuiz { get; set; }
        ReadingLesson? CurrentReading { get; set; }
        ListeningLesson? CurrentListening { get; set; }
        string? CurrentTopic { get; set; }
        public Topic? CurrentSpeakingTopic { get; set; }
        void SetCurrentUser(User user);
        User? GetCurrentUser();
        void Logout();
        bool IsUserLoggedIn();
        string GetCurrentTopic();
        void SetCurrentTopic(string topic);
        void SetCurrentReading(ReadingLesson reading);
        void SetCurrentSpeakingTopic(Topic speakingTopic);
        ReadingLesson? GetCurrentReading();
        void SetCurrentListening(ListeningLesson listening);
        ListeningLesson? GetCurrentListening();
        Quiz? GetCurrentQuiz();
        void SetCurrentQuiz(Quiz quiz);

    }
}
