using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mscc.GenerativeAI;

namespace TDEduEnglish.AppServices {
    public class QuizQuestionService : IQuizQuestionService {
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly GenerativeModel _model;
        public  QuizQuestionService(IQuizQuestionRepository quizQuestionRepository) {
            _quizQuestionRepository = quizQuestionRepository;

            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            string apiKey = config["GeminiSettings:GeminiAPIKey"];
            string modelName = config["GeminiSettings:ModelName"];

            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }

        public async Task Add(QuizQuestion quiz) => await _quizQuestionRepository.Add(quiz);
        public async Task Delete(int id) => await _quizQuestionRepository.Delete(id);
        public async Task<IEnumerable<QuizQuestion>> GetAll() => await _quizQuestionRepository.GetAll();
        public async Task<QuizQuestion?> GetById(int id) => await _quizQuestionRepository.GetById(id);
        public async Task<IEnumerable<QuizQuestion>> GetByQuizId(int quizid) => await _quizQuestionRepository.GetByQuizId(quizid);
        public async Task Update(QuizQuestion quiz) => await _quizQuestionRepository.Update(quiz);
        public async Task AddListAsync(IEnumerable<QuizQuestion> quizQuestions) => await _quizQuestionRepository.AddListAsync(quizQuestions);
        public async Task<IEnumerable<QuizQuestion>> GetUnCompletedOrFlaggedQuestion() => await _quizQuestionRepository.GetUnCompletedOrFlaggedQuestion();
        public async Task<string?> GetSuggestionAsync(string question) {
            string prompt = $@"
                               You are an expert English quiz question advisor.
                               Provide a suggestion for users that they can use to improve their quiz question.
                               Question: '{question}'
                               Respond with only the revised question without any additional explanations.
                               Note: Make sure the suggestion is clear, concise, and relevant to the original question, your response should be not related to the answer of the question, explain that user can understand and can solve the question better.
                                     Less than 50 words.
                              ";
            var result = await _model.GenerateContent(prompt);
            if (result == null)
                return "⚠️ No response from AI model.";
            var content = result.Candidates?.FirstOrDefault()?
                                .Content?.Parts?.FirstOrDefault()?.Text
                           ?? result.Text
                           ?? "⚠️ Empty response from AI.";
            return content.Trim();
        }
    }
}
