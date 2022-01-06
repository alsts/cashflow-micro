using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext context;

        public CommandRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            context.Platforms.Add(platform);
        }

        public bool PlatformExist(int platformId)
        {
            return context.Platforms.Any(p => p.Id == platformId);
        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return context.Commands.Where(p => p.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public Command GetCommand(int commandId)
        {
            return context.Commands
                .FirstOrDefault(c => c.Id == commandId);
        }

        public void CreateCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            context.Commands.Add(command);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }
    }
}
