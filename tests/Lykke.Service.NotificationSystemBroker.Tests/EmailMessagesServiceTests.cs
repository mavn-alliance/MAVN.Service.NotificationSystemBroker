using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.Domain.Repositories;
using Lykke.Service.NotificationSystemBroker.DomainServices.Services;
using Moq;
using Xunit;

namespace Lykke.Service.NotificationSystemBroker.Tests
{
    public class EmailMessagesServiceTests
    {
        private readonly Mock<IEmailMessageRepository> _emailMessageRepositoryMock = new Mock<IEmailMessageRepository>();
        private readonly EmailMessagesService _service;

        public EmailMessagesServiceTests()
        {
            _service = new EmailMessagesService(_emailMessageRepositoryMock.Object);
        }

        [Fact]
        public async Task When_WorkModeInvalid_ExpectNoMethodsCalled()
        {
            //Arrange
            _emailMessageRepositoryMock.Setup(x => x.GetEmailMessagesForEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<IEnumerable<IEmailMessage>>()));

            //Act
            await _service.RetrieveEmailMessages(It.IsAny<string>());

            //Assert
            _emailMessageRepositoryMock.Verify(x => x.GetEmailMessagesForEmailAsync(It.IsAny<string>()), Times.Once);}

    }
}
