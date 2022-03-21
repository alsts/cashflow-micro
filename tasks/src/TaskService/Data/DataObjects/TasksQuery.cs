namespace TaskService.Data.DataObjects
{
    public class TasksQuery
    {
        public TasksQuery(string task, string user)
        {
            Task = task;
            User = user;
        }

        public string Task { get; set; }
        public string User { get; set; }
    }
}
