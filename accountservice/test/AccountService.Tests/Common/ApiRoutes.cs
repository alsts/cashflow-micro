namespace AccountService.Tests.Common
{
    public static class ApiRoutes
    {
        public static class Account
        {
            private const string AccountBase = "/account";
            public const string GetCurrent = AccountBase;
            public const string Update = AccountBase;
            public const string SignIn = AccountBase +  "/signin";
            public const string SignUp = AccountBase +  "/signup";
            public const string Refresh = AccountBase +  "/refresh";
            public const string GetById = AccountBase +  "/{id}";
            public const string GetAll = AccountBase +  "/all";
        }
    }
}
