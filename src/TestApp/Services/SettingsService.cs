using Newtonsoft.Json;
using System;
using System.IO;
using TestApp.Settings;
using TestApp.Utils;

namespace TestApp.Services
{
    public static class SettingsService
    {
        public static string LoginPage { get; private set; } = "https://accounts.google.com/signin";

        public static TimeSpan ImplicitWaitSpan { get; private set; } = TimeSpan.FromSeconds(10);

        public static string FirstUserEmail { get; private set; } = "ttestname99@gmail.com";

        public static string FirstUserPassword { get; private set; } = "Abcd_123";

        public static string ExpectedAccountUrl { get; private set; } = "https://myaccount.google.com/?utm_source=sign_in_no_continue&pli=1";

        public static string SecondUserEmail { get; private set; } = "testsurname299@mail.ru";

        public static string SecondUserPassword { get; private set; } = "Abcd_3_test";

        public static string GmailUrl { get; private set; } = "https://mail.google.com/mail";

        public static string MessageToSend { get; private set; } = "Hello; brother in test!";

        public static string MailLoginUrl { get; private set; } = "https://mail.ru/";

        public static string MailUrl { get; private set; } = "https://e.mail.ru/inbox/";

        public static string NewPseudonym { get; private set; } = "IAmNew";

        public static string AccountUrl { get; private set; } = "https://myaccount.google.com/";

        public static string OldNickName { get; private set; } = "TestName";

        private static bool isAlreadySet = false;

        public static void SetSettings()
        {
            if (!isAlreadySet)
            {
                var environment = Environment.GetEnvironmentVariable("environment");
                if (!string.IsNullOrEmpty(environment))
                {
                    var dataPath = PathUtil.GetPathToData(environment.Replace("\'", ""));
                    var file = File.ReadAllText(dataPath);
                    var settings = JsonConvert.DeserializeObject<TestSettings>(file);
                    AccountUrl = settings.AccountUrl;
                    ExpectedAccountUrl = settings.ExpectedAccountUrl;
                    FirstUserEmail = settings.FirstUserEmail;
                    FirstUserPassword = settings.FirstUserPassword;
                    GmailUrl = settings.GmailUrl;
                    ImplicitWaitSpan = TimeSpan.FromSeconds(settings.ImplicitWaitSpan);
                    LoginPage = settings.LoginPage;
                    MailLoginUrl = settings.MailLoginUrl;
                    MailUrl = settings.MailUrl;
                    MessageToSend = settings.MessageToSend;
                    NewPseudonym = settings.NewPseudonym;
                    OldNickName = settings.OldNickName;
                    SecondUserEmail = settings.SecondUserEmail;
                    SecondUserPassword = settings.SecondUserPassword;
                }
                
                isAlreadySet = true;
            }
        }
    }
}
