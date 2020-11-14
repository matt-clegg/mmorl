using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MMORL.Client.Auth
{
    public class AuthenticationManager
    {
        private static readonly HttpClient _client = new HttpClient();

        public string Login(string username, string password)
        {
            LoginData login = new LoginData
            {
                Email = username,
                Password = password
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string loginJson = JsonSerializer.Serialize(login, options);

            var content = new StringContent(loginJson, Encoding.UTF8, "application/json");

            string token = string.Empty;

            try
            {
                var result = _client.PostAsync("/api/user/login", content).Result;
                token = result.Headers.GetValues("auth").FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when trying to login {ex.Message}");
            }

            return token;
        }

        // TODO: Maybe move the HttpClient elsewhere, as this class will be instantiated in a separate thread.
        static AuthenticationManager()
        {
            _client.BaseAddress = new Uri("http://localhost:3000");
        }
    }
}
