namespace TaskService.Tests.Common
{
    public static class ApiRoutes
    {
        public static class Tasks
        {
            private const string TaskBase = "/api/tasks";
            public const string GetCurrent = TaskBase;
            public const string Create = TaskBase;
            public const string Update = TaskBase + "/{id}";
            public const string GetAll = TaskBase +  "";
            public const string SignUp = TaskBase +  "/signup";
            public const string Refresh = TaskBase +  "/refresh";
            public const string GetById = TaskBase +  "/{id}";
        }
    }
}
