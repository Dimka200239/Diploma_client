using System;
using System.Text.Json.Serialization;

namespace client.Results
{
    public class AuthResult : BaseResult
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
