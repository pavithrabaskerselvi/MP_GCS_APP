using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissionPlanner
{
    public static class AuthService
    {
        const string ApiKey = "AIzaSyCY2q-nC8jtKRQnOre_z-gR_LlRK32x_rI";
        static readonly HttpClient http = new HttpClient();

        public static string UserEmail;
        public static string IdToken;

        public static Task<string> SignIn(string email, string pass)
        {
            return Call("signInWithPassword", email, pass);
        }

        public static Task<string> SignUp(string email, string pass)
        {
            return Call("signUp", email, pass);
        }

        // returns null on success, or an error message
        static async Task<string> Call(string action, string email, string pass)
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                var url = "https://identitytoolkit.googleapis.com/v1/accounts:" + action + "?key=" + ApiKey;
                var body = JsonConvert.SerializeObject(new { email = email, password = pass, returnSecureToken = true });
                var res = await http.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                var json = JObject.Parse(await res.Content.ReadAsStringAsync());

                if (res.IsSuccessStatusCode)
                {
                    UserEmail = (string)json["email"];
                    IdToken = (string)json["idToken"];
                    return null;
                }

                return Friendly((string)json["error"]?["message"]);
            }
            catch (Exception ex)
            {
                return "Network error: " + ex.Message;
            }
        }

        static string Friendly(string m)
        {
            if (m == null) return "Unknown error";
            if (m.StartsWith("EMAIL_NOT_FOUND") || m.StartsWith("INVALID_PASSWORD") || m.StartsWith("INVALID_LOGIN_CREDENTIALS"))
                return "Email or password is wrong";
            if (m.StartsWith("EMAIL_EXISTS")) return "This email is already registered";
            if (m.StartsWith("WEAK_PASSWORD")) return "Password must be at least 6 characters";
            if (m.StartsWith("INVALID_EMAIL")) return "Invalid email";
            if (m.StartsWith("TOO_MANY_ATTEMPTS")) return "Too many attempts, try later";
            return m;
        }
    }
}