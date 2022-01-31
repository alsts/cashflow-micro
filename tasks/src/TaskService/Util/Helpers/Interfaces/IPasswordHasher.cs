namespace TaskService.Util.Helpers.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
