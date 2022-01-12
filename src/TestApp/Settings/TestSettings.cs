using System;

namespace TestApp.Settings
{
    public static class TestSettings
    {
        public static readonly string LoginPage = "https://accounts.google.com/signin";

        public static readonly TimeSpan ImplicitWaitSpan = TimeSpan.FromSeconds(10);

        public static readonly string FirstUserEmail = "ttestname99@gmail.com";

        public static readonly string FirstUserPassword = "Abcd_123";

        public static readonly string ExpectedAccountUrl = "https://myaccount.google.com/?utm_source=sign_in_no_continue&pli=1";

        public static readonly string SecondUserEmail = "testsurname299@mail.ru";

        public static readonly string SecondUserPassword = "Abcd_3_test";

        public static readonly string GmailUrl = "https://mail.google.com/mail";

        public static readonly string MessageToSend = "Hello, brother in test!";

        public static readonly string MailLoginUrl = "https://mail.ru/";

        public static readonly string MailUrl = "https://e.mail.ru/inbox/";

        public static readonly string NewPseudonym = "IAmNew";

        public static readonly string AccountUrl = "https://myaccount.google.com/";

        public static readonly string OldNickName = "TestName";
    }
}
