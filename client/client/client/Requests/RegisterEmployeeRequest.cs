namespace client.Requests
{
    public class RegisterEmployeeRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; }

        public string Token { get; set; } //Токен из ссылки для регистрации
    }
}
