using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }
        
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"---> Calling GRPC service {configuration["GrpcPlatform"]}");
            var chanel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(chanel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception e)
            {
                Console.WriteLine($"---> Could not call GRPC Server {e}");
                return null;
            }
        }
    }
}
