using System;
using System.Threading.Tasks;
using AccountService.Data.Models;
using AccountService.Data.Repos.Interfaces;
using AccountService.Events.Consumers;
using AccountService.Events.Publishers.Interfaces;
using AutoMapper;
using Cashflow.Common.Events.Moderation;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AccountService.Tests.Events
{
    public class UserBannedConsumerTests
    {
        private Mock<IMapper> mapper;
        private Mock<ILogger<UserBannedConsumer>> logger;
        private Mock<IUserRepo> userRepo;
        private Mock<IMessageBusPublisher> messageBusPublisher;

        public UserBannedConsumerTests()
        {
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<UserBannedConsumer>>();
            userRepo = new Mock<IUserRepo>();
            messageBusPublisher = new Mock<IMessageBusPublisher>();
        }

        private UserBannedConsumer CreateUserBannedConsumer()
        {
            return new(
                mapper.Object,
                logger.Object,
                userRepo.Object,
                messageBusPublisher.Object);
        }

        [Fact]
        public async Task Consume_NoUserReturned_ThrowsException()
        {
            // Arrange
            var userBannedEvent = new UserBannedEvent();
            var context = Mock.Of<ConsumeContext<UserBannedEvent>>(cc =>
                cc.Message == userBannedEvent);
            
            // Act
            var sut = CreateUserBannedConsumer();

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => { await sut.Consume(context); });
        }
        
        [Fact]
        public async Task Consume_EmptyEvent_Skipped()
        {
            // Arrange
            var context = Mock.Of<ConsumeContext<UserBannedEvent>>(cc =>
                cc.Message == null);
            
            // Act
            var sut = CreateUserBannedConsumer();
            await sut.Consume(context);
            
            // Assert
            userRepo.Verify(x => x.GetByPublicId(It.IsAny<string>()), Times.Never);
            messageBusPublisher.Verify(x => x.PublishUpdatedUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Consume_UserReturned_Ok()
        {
            // Arrange
            var userBannedEvent = new UserBannedEvent();
            var context = Mock.Of<ConsumeContext<UserBannedEvent>>(cc =>
                cc.Message == userBannedEvent);

            var user = new User();
            userRepo.Setup(x => x.GetByPublicId(It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            // Act
            var sut = CreateUserBannedConsumer();
            await sut.Consume(context);

            // Assert
            userRepo.Verify(x => x.GetByPublicId(It.IsAny<string>()), Times.Once);
            messageBusPublisher.Verify(x => x.PublishUpdatedUser(It.IsAny<User>()), Times.Once);
        }
    }
}
