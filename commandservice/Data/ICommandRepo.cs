using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        // Platforms
        void CreatePlatform(Platform platform);
        bool PlatformExist(int platformId);
        bool ExternalPlatformExist(int externalPlatformId);
        
        // Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int commandId);
        void CreateCommand(Command command);

        bool SaveChanges();
    }
}
