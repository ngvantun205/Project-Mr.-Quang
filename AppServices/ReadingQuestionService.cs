using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace TDEduEnglish.AppServices {
    internal class ReadingQuestionService : IReadingQuestionService {
        private readonly IReadingQuestionRepository _readingQuestionRepository;
        private readonly GenerativeModel _model;

        public ReadingQuestionService(IReadingQuestionRepository readingQuestionRepository) {
            _readingQuestionRepository = readingQuestionRepository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string apiKey = config["GeminiSettings:GeminiAPIKey"];
            string modelName = config["GeminiSettings:ModelName"];

            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }
        public async Task AddListAsync(IEnumerable<ReadingQuestion> questions) => await _readingQuestionRepository.AddListAsync(questions);
        public async Task<IEnumerable<ReadingQuestion>> GetByLessonId(int lessonId) => await _readingQuestionRepository.GetByLessonId(lessonId);
        public async Task<IEnumerable<ReadingQuestion>> GetAll() => await _readingQuestionRepository.GetAll();
        public async Task<ReadingQuestion?> GetById(int id) => await _readingQuestionRepository.GetById(id);
        public async Task Add(ReadingQuestion entity) => await _readingQuestionRepository.Add(entity);
        public async Task Update(ReadingQuestion entity) => await _readingQuestionRepository.Update(entity);
        public async Task Delete(int id) => await _readingQuestionRepository.Delete(id);

        public async Task<string?> GetMeaningAsync(string word, ReadingLesson readinglesson) {
            string prompt = $@"
                               You are an expert bilingual dictionary.
                               Define the English word '{word}' based on the reading context below:

                              Context: {readinglesson.Content}

                              Respond in exactly one line, following this format and dont write again the origin word:
                              [word type]/ [meaning in Vietnamese]

                              Example: run → verb/ chạy, hoạt động, điều hành
                              Do not include English explanations or extra text.
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
        public async Task GenerateQuestionsAsync(ReadingLesson readingLesson, int numberOfQuestions) {
            try {
                if (readingLesson == null || string.IsNullOrWhiteSpace(readingLesson.Content)) {
                    MessageBox.Show("⚠️ Reading lesson is invalid or has no content.", "Invalid Lesson", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string prompt = @$"
        You are an expert English exam creator.
        Please generate {numberOfQuestions} multiple-choice reading comprehension questions (in JSON format)
        based on the reading passage below.

        Reading Passage:
        ------------------
        {readingLesson.Content}

        Requirements:
        - Each question should have:
            1. QuestionText: clear and grammatically correct.
            2. Option1, Option2, Option3, Option4: four distinct and logical choices.
            3. CorrectAnswer: one of the options (exact text match).
            4. Explanation: short English explanation why the correct answer is correct.
        - The questions should vary in difficulty and test understanding, vocabulary, inference, etc.
        - Do NOT include additional text or explanations outside JSON.
        - Format output strictly as JSON array of objects in this form:
        [
            {{
                ""QuestionNumber"": 1,
                ""QuestionText"": ""What is the main idea of the passage?"",
                ""Option1"": ""..."",
                ""Option2"": ""..."",
                ""Option3"": ""..."",
                ""Option4"": ""..."",
                ""CorrectAnswer"": ""..."",
                ""Explanation"": ""...""
            }}
        ]
        Return ONLY valid JSON array. No markdown, code blocks, or other text.
        ";

                // 🧠 Gọi Gemini model
                var result = await _model.GenerateContent(prompt);

                if (result == null) {
                    MessageBox.Show("⚠️ No response from AI model.");
                    return;
                }

                var content = result.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text
                              ?? result.Text
                              ?? "";

                content = content.Trim();

                // 🩹 Xử lý trường hợp AI trả về dạng có dấu ```json
                if (content.StartsWith("```")) {
                    int start = content.IndexOf("[");
                    int end = content.LastIndexOf("]");
                    if (start >= 0 && end > start)
                        content = content.Substring(start, end - start + 1);
                }

                MessageBox.Show($"📜 Generated Questions:\n{content}", "AI Output Preview", MessageBoxButton.OK, MessageBoxImage.Information);

                // 🧩 Giải mã JSON thành danh sách câu hỏi
                var questions = JsonSerializer.Deserialize<List<ReadingQuestion>>(content);

                if (questions != null && questions.Count > 0) {
                    // Gán LessonId cho từng câu hỏi
                    foreach (var q in questions) {
                        q.ReadingLessonId = readingLesson.ReadingLessonId;
                    }

                    await AddListAsync(questions);

                    MessageBox.Show($"✅ Successfully generated and saved {questions.Count} questions for lesson '{readingLesson.Title}'.",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else {
                    MessageBox.Show("⚠️ Generated data is empty or invalid JSON format.",
                                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"❌ Error while generating reading questions: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
