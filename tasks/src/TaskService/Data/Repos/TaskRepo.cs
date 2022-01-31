using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskService.Data.Repos.Interfaces;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Data.Repos
{
    public class TaskRepo : ITaskRepo
    {
        private readonly AppDbContext context;

        public TaskRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<TaskEntity> GetById(int id)
        {
            return await context.Tasks.FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<TaskEntity> GetByPublicId(string publicId)
        {
            return await context.Tasks.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }
        
        public async Task Save(TaskEntity task)
        {
            if (task.Id != 0)
            {
                context.Tasks.Update(task);
            }
            else
            {
                await context.Tasks.AddAsync(task);
            }
            
            await SaveChanges();
        }

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await context.Tasks.ToListAsync();
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
