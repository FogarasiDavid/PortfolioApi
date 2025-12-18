using Portfolio.Application.DTOs;
using System.Text.Json; 
using System.Net.Http.Json;

namespace Portfolio.Infrastructure.External.Github
{
    public class GitHubClient
    {
        //httpclient
        private readonly HttpClient _httpClient;

        public GitHubClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //githubclient elkezdi a lekerest, es megkapja az adatokat
        public async Task<IEnumerable<GitHubRepoDto>> GetReposAsync(string username)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower 
            };

            Console.WriteLine($"[GitHubClient] Lekérdezés indítása: users/{username}/repos");

            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<GitHubRepoDto>>(
                    $"users/{username}/repos",
                    options
                );
                var count = response?.Count() ?? 0;
                Console.WriteLine($"[GitHubClient] Sikeres letöltés. Elemek száma: {count}");

                return response ?? new List<GitHubRepoDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GitHubClient] hiba történt {ex.Message}");
                throw; 
            }
        }
    }
}