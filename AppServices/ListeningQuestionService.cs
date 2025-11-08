using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.AppServices {
    internal class ListeningQuestionService : IListeningQuestionService {
        private readonly IListeningQuestionRepository _listeningQuestionRepository;
        private readonly GenerativeModel _model;
        public ListeningQuestionService(IListeningQuestionRepository listeningQuestionRepository) {
            _listeningQuestionRepository = listeningQuestionRepository;

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string apiKey = config["GeminiSettings:GeminiAPIKey"];
            string modelName = config["GeminiSettings:ModelName"];

            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }
        public async Task<IEnumerable<ListeningQuestion>> GetAll() => await _listeningQuestionRepository.GetAll();
        public async Task<ListeningQuestion?> GetById(int id) => await _listeningQuestionRepository.GetById(id);
        public async Task Add(ListeningQuestion question) => await _listeningQuestionRepository.Add(question);
        public async Task Delete(int id) => await _listeningQuestionRepository.Delete(id);
        public async Task Update(ListeningQuestion question) => await _listeningQuestionRepository.Update(question);
        public async Task<IEnumerable<ListeningQuestion>> GetByListeningId(int id) => await _listeningQuestionRepository.GetByListeningId(id);
        public async Task AddListAsync(IEnumerable<ListeningQuestion> list) => await _listeningQuestionRepository.AddListAsync(list);

        public async Task<string?> GetMeaningAsync(string word) {
            string prompt = $@"
                               You are an expert bilingual dictionary.
                               Define the English word '{word}' 
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
