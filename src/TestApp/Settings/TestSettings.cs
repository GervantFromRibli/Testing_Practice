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
    }
}
