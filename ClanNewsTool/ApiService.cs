using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClanNewsTool
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "https://wolffiles.eu/api/v1/clan";

        public ApiService(string apiKey)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Clan-Api-Key", apiKey);
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(15);
        }

        private async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{BaseUrl}/{endpoint}", content);
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body);
        }

        private async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{endpoint}");
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body);
        }

        public async Task<ClanInfo?> GetMeAsync()
        {
            return await GetAsync<ClanInfo>("me");
        }

        public async Task<ApiResponse?> PostNewsAsync(NewsPost post)
        {
            return await PostAsync<ApiResponse>("news", post);
        }

        public async Task<ApiResponse?> PostEventAsync(EventPost post)
        {
            return await PostAsync<ApiResponse>("event", post);
        }

        public async Task<ApiResponse?> PostMatchAsync(MatchPost post)
        {
            return await PostAsync<ApiResponse>("match", post);
        }

        public async Task<ApiResponse?> PostRecruitmentAsync(RecruitmentPost post)
        {
            return await PostAsync<ApiResponse>("recruitment", post);
        }

        public static async Task<VersionInfo?> GetVersionAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(
                "https://wolffiles.eu/api/v1/clan/version");
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VersionInfo>(body);
        }
    }
}