namespace TaskService.Tests.Common
{
    public static class ApiRoutes
    {
        public static class Tasks
        {
            public static class Promotion
            {
                private const string TasksPromotionBase =  "/api/promotion/tasks";
                public const string Create = TasksPromotionBase;
                public const string Update = TasksPromotionBase + "/{id}";
                public const string GetUserTasks = TasksPromotionBase +  "";
                public const string GetById = TasksPromotionBase +  "/{id}";
            }
        }
    }
}
