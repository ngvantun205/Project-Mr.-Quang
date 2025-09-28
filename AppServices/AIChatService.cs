using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mscc.GenerativeAI;


namespace TDEduEnglish.AppServices {
    internal class AIChatService : IAIChatService {
        private readonly IAIChatRepository _aiChatRepository;
        private readonly GenerativeModel _model;
        public AIChatService(IAIChatRepository aiChatRepository, string apiKey = "AIzaSyC4nDtN-OVQ4DuPpMSk-99fsiKU0cf5pIM", string modelName = "gemini-2.5-flash") {
            _aiChatRepository = aiChatRepository;
            var api = new GoogleAI(apiKey);
            _model = api.GenerativeModel(model: modelName);
        }
        public async Task<IEnumerable<AIChat>> GetAll() => await _aiChatRepository.GetAll();
        public async Task<AIChat?> GetById(int id) => await _aiChatRepository.GetById(id);
        public async Task Add(AIChat chat) => await _aiChatRepository.Add(chat);
        public async Task Delete(int id) => await _aiChatRepository.Delete(id);
        public async Task Update(AIChat chat) => await _aiChatRepository.Update(chat);
        public async Task<IEnumerable<AIChat>> GetByUserId(int id) => await _aiChatRepository.GetByUserId(id);
        //public async Task<string?> GenerateTextAsync(string message) {
        //    var prompt = $@"
        //                   You are a teacher who teach English fot student
        //                   You have many years of experiences in teaching and correcting exercises/exams/submitions from student";

        //}
    }
}
