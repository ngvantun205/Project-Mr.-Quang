using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TDEduEnglish.DomainModels;
using TDEduEnglish.IAppServices;

namespace TDEduEnglish.AppServices {
    internal class ReadingService : IReadingService {
        private readonly IReadingRepository _readingRepository;
        private readonly GenerativeModel _model;
        public ReadingService(IReadingRepository readingRepository) {
            _readingRepository = readingRepository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string apiKey = config["GeminiSettings:GeminiAPIKey"];
            string modelName = config["GeminiSettings:ModelName"];

            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }
        public async Task<IEnumerable<ReadingLesson>> GetAll() => await _readingRepository.GetAll();
        public async Task<ReadingLesson?> GetById(int id) => await _readingRepository.GetById(id);
        public async Task Add(ReadingLesson vocabulary) => await _readingRepository.Add(vocabulary);
        public async Task Update(ReadingLesson vocabulary) => await _readingRepository.Update(vocabulary);
        public async Task Delete(int id) => await _readingRepository.Delete(id);
        public async Task SaveResult(UserReadingResult result) => await _readingRepository.SaveResult(result);
        public async Task AddListAsync(IEnumerable<ReadingLesson> readinglesson) => await _readingRepository.AddListAsync(readinglesson);
        public async Task<IEnumerable<ReadingLesson>> GetByLevel(string level) => await _readingRepository.GetByLevel(level);

        public async Task GenerateReadingAsync(string topic, string level, TimeSpan suggestedtime) {
            string prompt = @" I want to create a reading lesson for English learners.
                               Please generate a reading lessons in JSON format based on the following criteria:
                               1. Topic: " + topic + @"
                               2. Level: " + level + @"
                               3. Suggested Time: " + suggestedtime.TotalMinutes + @" minutes
                               Each reading lesson should include:
                               - Title: {topic}
                               - Content:number of words depends on the level, for example:
                                 - Beginner: 100-200 words
                                 - Intermediate: 200-300 words
                                 - Advanced: 300-500 words
                               - Level: Beginner, Intermediate, Advanced.
                               - SuggestedTime: The estimated time to read and understand the passage.
                               Format the output as a valid JSON array of objects inside and all of them must be in '[' and ']', dont add anymore words or other that dont relate to Json, return only one object with properties I gave.
                               For example:
                               Json is in format:
                               [
                                   {
                                       ""Title"": ""{topic}"",
                                       ""Content"": ""This is a sample content for the reading lesson."",
                                       ""Level"": ""{level}"",
                                       ""SuggestedTime"": ""00:05:00""
                                   }
                               ]
                              ";

            var result = await _model.GenerateContent(prompt);

            if (result == null)
                MessageBox.Show("⚠️ No response from AI model.");

            var content = result.Candidates?.FirstOrDefault()?
                                .Content?.Parts?.FirstOrDefault()?.Text
                           ?? result.Text
                           ?? "⚠️ Empty response from AI.";

            content.Trim();

            if (content.StartsWith("```")) {
                int start = content.IndexOf("{");
                int end = content.LastIndexOf("}");
                if (start >= 0 && end > start)
                    content = content.Substring(start, end - start + 1);
            }
            var content1 = $"[{content}]";
            MessageBox.Show($"📜 Generated content:\n{content1}", "Generated Reading Lesson", MessageBoxButton.OK, MessageBoxImage.Information);
            try {
                var readinglessons = JsonSerializer.Deserialize<List<ReadingLesson>>(content1);
                foreach (var lesson in readinglessons) {
                    if (double.TryParse(lesson.SuggestedTime.ToString(), out double minutes))
                        lesson.SuggestedTime = TimeSpan.FromMinutes(minutes);
                }
                if (readinglessons != null && readinglessons.Count > 0) {
                    await AddListAsync(readinglessons);

                    MessageBox.Show($"✅ Đã thêm {readinglessons.Count} bài đọc vào cơ sở dữ liệu.",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else {
                    MessageBox.Show("⚠️ File JSON rỗng hoặc không đúng định dạng.",
                                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"❌ Lỗi khi đọc file JSON: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
