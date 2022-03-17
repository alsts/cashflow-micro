
using ModerationService.Data.Models;
using ModerationService.Dtos;
using TaskEntity = ModerationService.Data.Models.Task;

namespace ModerationService.Util
{
    namespace AccountService.Util.Helpers
    {
        public static class ExtensionMethods
        {
            public static UserReadDto ToPublicDto(this User user) 
            {
                if (user == null) return null;
            
                return new UserReadDto
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Id = user.PublicId
                };
            }
        
            public static TaskReadDto ToPublicDto(this TaskEntity task) 
            {
                if (task == null) return null;
            
                return new TaskReadDto
                {
                    Id = task.PublicId,
                    Title = task.Title,
                    Description = task.Description,
                    CreatedAt = task.CreatedAt
                };
            }
        }
    }
}
