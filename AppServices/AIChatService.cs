using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mscc.GenerativeAI;


namespace TDEduEnglish.AppServices {
    internal class AIChatService : IAIChatService {
        private readonly IAIChatRepository _aiChatRepository;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        public AIChatService(IAIChatRepository aiChatRepository, string apiKey) {
            _aiChatRepository = aiChatRepository;
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient();
        }
        public async Task<IEnumerable<AIChat>> GetAll() => await _aiChatRepository.GetAll();
        public async Task<AIChat?> GetById(int id) => await _aiChatRepository.GetById(id);
        public async Task Add(AIChat chat) => await _aiChatRepository.Add(chat);
        public async Task Delete(int id) => await _aiChatRepository.Delete(id);
        public async Task Update(AIChat chat) => await _aiChatRepository.Update(chat);
        public async Task<IEnumerable<AIChat>> GetByUserId(int id) => await _aiChatRepository.GetByUserId(id);

        public async Task<string> SendMessageAsync(IEnumerable<AIChat> history, string userMessage) {
            if (string.IsNullOrWhiteSpace(userMessage))
                throw new ArgumentNullException("Message cannot be empty");

            var url = "https://api.openai.com/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var payload = new {
                model = "gpt-4o-mini",
                messages = history.Select(m => new { userid = m.UserId, content = m.Response })
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8, "application/json"
                );

            using var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) {
                throw new Exception($"API Error {response.StatusCode} : {body}");
            }

            try {
                using var doc = JsonDocument.Parse(body);
                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return content ?? "";
            }
            catch (Exception ex) {
                throw new Exception("Parse error: " + ex.Message + "\nResponse body: " + body);
            }
        }
    }
}
