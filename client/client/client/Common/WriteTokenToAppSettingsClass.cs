using client.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace client.Common
{
    public class WriteTokenToAppSettingsClass
    {
        private string AppSettingsPath;

        public WriteTokenToAppSettingsClass()
        {
            AppSettingsPath = Directory.GetCurrentDirectory() + "\\..\\..\\appsettings.json";
        }

        public void WriteToken(string token)
        {
            var settings = ReadAppSettings();
            settings.EmployeeToken = token;

            WriteAppSettings(settings);
        }

        public void ClearToken()
        {
            var settings = ReadAppSettings();
            settings.EmployeeToken = "";

            WriteAppSettings(settings);
        }

        public void WriteLoginAndPassword(string login, string password)
        {
            var settings = ReadAppSettings();
            settings.LastEmployeeLogin = login;
            settings.LastEmployeePassword = password;

            WriteAppSettings(settings);
        }

        public void ClearLoginAndPassWord()
        {
            var settings = ReadAppSettings();
            settings.LastEmployeeLogin = "";
            settings.LastEmployeePassword = "";

            WriteAppSettings(settings);
        }

        private AppSettings ReadAppSettings()
        {
            if (File.Exists(AppSettingsPath))
            {
                var json = File.ReadAllText(AppSettingsPath);
                return JsonConvert.DeserializeObject<AppSettings>(json);
            }
            else
            {
                return new AppSettings();
            }
        }

        private void WriteAppSettings(AppSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(AppSettingsPath, json);
        }
    }
}
