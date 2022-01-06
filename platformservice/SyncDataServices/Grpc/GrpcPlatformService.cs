using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService: GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo platformRepo;
        private readonly IMapper mapper;

        public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper)
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = platformRepo.GetAll();

            foreach (var platform in platforms)
            {
                response.Platform.Add(mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}
