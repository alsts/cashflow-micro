using Microsoft.AspNetCore.Http;
using TaskService.Data.Models;
using TaskService.Data.Models.External;
using TaskService.Dtos;
using TaskService.Dtos.Promotion;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Util
{
    public static class ExtensionMethods
    {
        public static void AppendAuthCookie(this HttpResponse response, User user, string token) 
        {
            if (response == null) return;
            response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
        }
        
        public static UserReadDto ToPublicDto(this User user) 
        {
            if (user == null) return null;
            
            return new UserReadDto
            {
                Email = user.Email,
                Username = user.UserName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
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
