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
        public async Task<string?> GetSuggestionAsync(QuizQuestion question) {
            string prompt = $@"
                              You are an experienced English teacher helping students with quiz questions.
                              Your task: give a helpful hint that makes the question clearer or easier to understand — 
                              but never reveal or imply the correct answer.

                              Question: '{question.QuestionText}'
                              Options:
                              A. {question.Option1}
                              B. {question.Option2}
                              C. {question.Option3}
                              D. {question.Option4}

                              Rules:
                              - Do NOT mention or hint which option is correct.
                              - Focus on explaining context, grammar, or vocabulary related to the question.
                              - The hint should guide understanding, not give the answer.
                              - Keep it concise (under 50 words).
                              - Write naturally as if speaking to a learner.
                              - Answer in Vietnamese.   

                              Return only the hint text, no explanation or intro words.
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
