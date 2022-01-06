using System;
using System.Collections.Generic;
using System.Linq;
using PlatformServices.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext context;

        public PlatformRepo(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Platform> GetAll()
        {
            return context.Platforms.ToList();
        }

        public Platform GetById(int id)
        {
            return context.Platforms.FirstOrDefault(p => p.Id ==  id);
        }

        public void Create(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            context.Platforms.Add(platform);
        }
        
        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }
    }
}
