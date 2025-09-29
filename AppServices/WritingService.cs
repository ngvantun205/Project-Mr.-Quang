using GenerativeAI.Types;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    public class WritingService : IWritingService {
        private readonly IWritingRepository _writingRepository;
        private readonly GenerativeModel _model;
        public WritingService(IWritingRepository writingRepository) {
            _writingRepository = writingRepository;
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string apiKey = config["GeminiSettings:GeminiAPIKey"];
            string modelName = config["GeminiSettings:ModelName"];

            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }
        public async Task<IEnumerable<Writing>> GetAll() => await _writingRepository.GetAll();
        public async Task<Writing?> GetById(int id) => await _writingRepository.GetById(id);
        public async Task Add(Writing chat) => await _writingRepository.Add(chat);
        public async Task Delete(int id) => await _writingRepository.Delete(id);
        public async Task Update(Writing chat) => await _writingRepository.Update(chat);
        public async Task<IEnumerable<Writing>> GetByUserId(int id) => await _writingRepository.GetByUserId(id);
        public async Task<string?> GenerateTextAsync(string message, string userlevel, string writingtask) {
            string prompt = $@"
                             You are an experienced IELTS Writing teacher and English writing coach.
                             Your job is to review and correct the following English writing submitted by a student.

                             Student Level: {userlevel}
                             Writing Task: {writingtask}
                             Student's Writing:
                             {message}

                             Please follow these steps clearly and in detail:

                             1. **Grammar and Spelling**: Identify and correct all grammar, punctuation, and spelling mistakes.
                             2. **Vocabulary and Word Choice**: Suggest better or more natural words and phrases where appropriate.
                             3. **Sentence Structure and Cohesion**: Analyze how sentences connect and flow. Suggest improvements for clarity and coherence.
                             4. **Content and Task Response**: Evaluate if the writing fully answers the prompt and maintains logical ideas. Point out any missing or off-topic parts.
                             5. **Tone and Style**: Comment on whether the tone fits the purpose (formal/informal, academic, etc.). Suggest adjustments if needed.
                             6. **Improved Version**: Rewrite the entire passage in a more polished, natural, and advanced way, while keeping the original meaning.
                             7. **Score and Advice**: Give a band score (IELTS-style or CEFR-level) and provide specific advice for improvement.

                             Return your response in this clear structure:

                             ---
                             **Original Text (Student’s writing):**
                             {message}

                              **Corrections and Comments:**
                              - Grammar: ...
                              - Vocabulary: ...
                              - Structure: ...
                              - Content: ...
                              - Tone: ...

                              **Improved Version:**
                              <your_revised_text>

                              **Estimated Level:** B2 / Band 6.5  
                              **Advice:** <personalized feedback on how to improve>
                              ---

                              Tone Guidelines:
                              - Be friendly, encouraging, and educational.
                              - Do not just rewrite the text — explain *why* changes are made.
                              - Avoid overly formal or robotic language; sound like a helpful coach.

                              End your feedback with a short motivational line, e.g.:
                              “Keep practicing — you’re getting better every time!”
                              Response in both English and VietNamese
                              ";
            var result = await _model.GenerateContent(prompt);

            if (result == null || result.Candidates == null || !result.Candidates.Any())
                return "⚠️ No response from AI model.";

            var content = result.Candidates[0].Content?.Parts?.FirstOrDefault()?.Text;
            return content ?? "⚠️ Empty response from Gemini.";
        }
    }
}
