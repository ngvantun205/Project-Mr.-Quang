using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    public interface ISpeechService {
        Task<UserSpeakingRecord> AssessAndSaveAsync(int userId, int sentenceId,  string referenceText);
        Task SpeakAsync(string text, string voice = "en-US-JennyNeural");
    }
}
