using System.Net.Http.Json;
using System.Text.Json.Nodes;

public static class AuthTestHelper
{
    public static async Task<string> LoginAndGetTokenAsync(HttpClient client, string username, string password)
    {
        var payload = new { username, password };
        var resp = await client.PostAsJsonAsync("/api/v1/auth/login", payload);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<JsonObject>();
        return json!["accessToken"]!.GetValue<string>();
    }

    public static void UseBearer(this HttpClient client, string token)
        => client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
}
