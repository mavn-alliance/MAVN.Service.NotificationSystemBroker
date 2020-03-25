using System;
using Lykke.Service.NotificationSystemBroker.DomainServices.Services;
using MailKit;
using Moq;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystemBroker.Contract;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.Domain.Enums;
using Lykke.Service.NotificationSystemBroker.Domain.Repositories;
using Xunit;
using MimeKit;
using Lykke.Service.NotificationSystemBroker.DomainServices;

namespace Lykke.Service.NotificationSystemBroker.Tests
{
    public class SmtpServiceTests
    {
        private const string _host = "host";
        private const int _port = 1;
        private const string _username = "username";
        private const string _password = "password";
        private const string _sender = "sender";
        private const bool _useSsl = true;
        private readonly Mock<IMailTransport> _mailClientMock;
        private readonly MailClientFactory _mailFactory;
        private readonly Mock<IEmailMessageRepository> _emailMessageRepositoryMock = new Mock<IEmailMessageRepository>();
        private readonly Mock<IRabbitPublisher<UpdateAuditMessageEvent>> _auditMessagePublisherMock =
            new Mock<IRabbitPublisher<UpdateAuditMessageEvent>>();

        public SmtpServiceTests()
        {
            _mailClientMock = new Mock<IMailTransport>();
            _mailFactory = new MailClientFactory(() => _mailClientMock.Object);
        }

        [Fact]
        public async Task When_WorkModeInvalid_ExpectNoMethodsCalled()
        {
            //Arrange
            //Check that the Enum is invalid
            Assert.True(!Enum.IsDefined(typeof(EmailSenderWorkMode), Int32.MaxValue));
            var service = CreateServiceWithMocks((EmailSenderWorkMode)Int32.MaxValue);

            _mailClientMock.Setup(x => x.IsConnected).Returns(false);
            _mailClientMock.Setup(x => x.IsAuthenticated).Returns(false);

            _mailClientMock.Setup(x => x.ConnectAsync(_host, _port, _useSsl, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.AuthenticateAsync(_username, _password, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.DisconnectAsync(true, default))
                .Returns(Task.CompletedTask);
            _emailMessageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<IEmailMessage>()))
                .Returns(Task.FromResult(It.IsAny<Guid>()));
            
            //Act
            await service.ProcessMessageAsync("email", "subject", "body", Guid.NewGuid());

            //Assert
            _mailClientMock.Verify(x => x.ConnectAsync(_host, _port, _useSsl, default), Times.Never());
            _mailClientMock.Verify(x => x.AuthenticateAsync(_username, _password, default), Times.Never());
            _mailClientMock.Verify(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null), Times.Never());
            _mailClientMock.Verify(x => x.DisconnectAsync(true, default), Times.Never());
            _emailMessageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<IEmailMessage>()), Times.Never());
        }

        [Fact]
        public async Task When_WorkModeSmtpAndStorage_ExpectAllMethodsCalled()
        {
            //Arrange
            var service = CreateServiceWithMocks(EmailSenderWorkMode.SmtpAndStorage);

            _mailClientMock.Setup(x => x.ConnectAsync(_host, _port, _useSsl, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.AuthenticateAsync(_username, _password, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.DisconnectAsync(true, default))
                .Returns(Task.CompletedTask);
            _emailMessageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<IEmailMessage>()))
                .Returns(Task.FromResult(It.IsAny<Guid>()));
            
            //Act
            await service.ProcessMessageAsync("email", "subject", "body", Guid.NewGuid());

            //Assert
            _mailClientMock.Verify(x => x.ConnectAsync(_host, _port, _useSsl, default), Times.Once());
            _mailClientMock.Verify(x => x.AuthenticateAsync(_username, _password, default), Times.Once());
            _mailClientMock.Verify(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null), Times.Once());
            _mailClientMock.Verify(x => x.DisconnectAsync(true, default), Times.Once());
            _emailMessageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<IEmailMessage>()), Times.Once());
            _auditMessagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once());
        }

        [Fact]
        public async Task When_WorkModeStorage_ExpectStorageMethodsForStorageCalled()
        {
            //Arrange
            var service = CreateServiceWithMocks(EmailSenderWorkMode.Storage);

            _emailMessageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<IEmailMessage>()))
                .Returns(Task.FromResult(It.IsAny<Guid>()));
            
            //Act
            await service.ProcessMessageAsync("email", "subject", "body", Guid.NewGuid());

            //Assert
            _emailMessageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<IEmailMessage>()), Times.Once);
            _auditMessagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once());
        }

        [Fact]
        public async Task When_WorkModeSmtp_ExpectSmtpMethodsForMailSendingCalled()
        {
            //Arrange
            var service = CreateServiceWithMocks(EmailSenderWorkMode.Smtp);

            _mailClientMock.Setup(x => x.ConnectAsync(_host, _port, _useSsl, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.AuthenticateAsync(_username, _password, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.DisconnectAsync(true, default))
                .Returns(Task.CompletedTask);
            _auditMessagePublisherMock.Setup(x => x.PublishAsync(Mock.Of<UpdateAuditMessageEvent>()));

            //Act
            await service.ProcessMessageAsync("email", "subject", "body", Guid.NewGuid());

            //Assert
            _mailClientMock.Verify(x => x.ConnectAsync(_host, _port, _useSsl, default), Times.Once());
            _mailClientMock.Verify(x => x.AuthenticateAsync(_username, _password, default), Times.Once());
            _mailClientMock.Verify(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null), Times.Once());
            _mailClientMock.Verify(x => x.DisconnectAsync(true, default), Times.Once());
            _auditMessagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once());
        }

        [Fact]
        public async Task When_SmtpThrowsException_ExpectExceptionLoggedAndWorkStopped()
        {
            //Arrange
            var service = CreateServiceWithMocks(EmailSenderWorkMode.Smtp);
            var exception = new TestException();
            var email = "email";
            var subject = "subject";

            _mailClientMock.Setup(x => x.ConnectAsync(_host, _port, _useSsl, default))
                .Throws(exception);
            _mailClientMock.Setup(x => x.AuthenticateAsync(_username, _password, default))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null))
                .Returns(Task.CompletedTask);
            _mailClientMock.Setup(x => x.DisconnectAsync(true, default))
                .Returns(Task.CompletedTask);
            _auditMessagePublisherMock.Setup(x => x.PublishAsync(Mock.Of<UpdateAuditMessageEvent>()));

            //Act
            await service.ProcessMessageAsync(email, subject, "body", Guid.NewGuid());

            //Assert
            _mailClientMock.Verify(x => x.ConnectAsync(_host, _port, _useSsl, default), Times.Once());
            _mailClientMock.Verify(x => x.AuthenticateAsync(_username, _password, default), Times.Never());
            _mailClientMock.Verify(x => x.SendAsync(It.IsAny<MimeMessage>(), default, null), Times.Never());
            _mailClientMock.Verify(x => x.DisconnectAsync(true, default), Times.Never());
            _auditMessagePublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once());
        }

        private SmtpService CreateServiceWithMocks(EmailSenderWorkMode workMode)
        {
            var service = new SmtpService(_mailFactory, EmptyLogFactory.Instance, 
                _host, _port, _username, _password, _sender, null,
                _useSsl, workMode, _emailMessageRepositoryMock.Object, 
                _auditMessagePublisherMock.Object);
            
            return service;
        }
    }
}
