using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
    } 
}
