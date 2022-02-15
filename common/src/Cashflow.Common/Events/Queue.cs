namespace Cashflow.Common.Events
{
    public static class Queue
    {
        public static class Accounts
        {
            // Moderation updates:
            public static string UserBanned = "accounts-user-banned-queue";
        }
        
        public static class Tasks
        {
            // User updates:
            public static string UserCreated = "tasks-user-created-queue";
            public static string UserUpdated = "tasks-user-updated-queue";
            // Money updates:
            public static string TaskTransactionCreated = "tasks-task-transaction-created-queue";
            public static string TaskJobTransactionCreated = "tasks-taskjob-transaction-created-queue";
            // Moderation updates:
            public static string TaskApproved = "tasks-task-approved-queue";
        }

        public static class Money
        {
            // User updates:
            public static string UserCreated = "money-user-created-queue";
            public static string UserUpdated = "money-user-updated-queue";
            // Task updates:
            public static string TaskCreated = "money-task-created-queue";
            public static string TaskUpdated = "money-task-updated-queue";
            // Task Job updates:
            public static string TaskJobCreated = "money-taskjob-created-queue";
            public static string TaskJobUpdated = "money-taskjob-updated-queue";
        }
        
        public static class Moderation
        {
            // User updates:
            public static string UserCreated = "moderation-user-created-queue";
            public static string UserUpdated = "moderation-user-updated-queue";
            // Task updates:
            public static string TaskCreated = "moderation-task-created-queue";
            public static string TaskUpdated = "moderation-task-updated-queue";
        }
    }
}
