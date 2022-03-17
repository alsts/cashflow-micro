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
