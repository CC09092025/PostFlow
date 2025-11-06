using System.Text.Json;
using PostFlow.Models;

namespace PostFlow.Services
{
    public class JsonPlaceholderService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";

        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/posts");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return posts ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching posts: {ex.Message}");
                return new List<Post>();
            }
        }
    }
}