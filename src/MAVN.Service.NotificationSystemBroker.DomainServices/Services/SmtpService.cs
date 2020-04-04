using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystemBroker.Contract;
using MAVN.Service.NotificationSystemBroker.Contract.Enums;
using MAVN.Service.NotificationSystemBroker.Domain.Enums;
using MAVN.Service.NotificationSystemBroker.Domain.Repositories;
using MAVN.Service.NotificationSystemBroker.Domain.Services;
using MAVN.Service.NotificationSystemBroker.DomainServices.Entities;
using MimeKit;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly MailClientFactory _mailClientFactory;
        private readonly ILog _log;
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _useSsl;
        private readonly bool _useAuth;
        private readonly EmailSenderWorkMode _emailSenderWorkMode;
        private readonly IEmailMessageRepository _emailMessageRepository;
        private readonly IRabbitPublisher<UpdateAuditMessageEvent> _auditMessagePublisher;
        private readonly MailboxAddress _senderAddress;

        public SmtpService(
            MailClientFactory mailClientFactory,
            ILogFactory logFactory,
            string host,
            int port,
            string username,
            string password,
            string emailSender,
            string emailSenderName,
            bool useSsl,
            EmailSenderWorkMode emailSenderWorkMode,
            IEmailMessageRepository emailMessageRepository,
            IRabbitPublisher<UpdateAuditMessageEvent> auditMessagePublisher)
        {
            _mailClientFactory = mailClientFactory ?? throw new ArgumentNullException(nameof(mailClientFactory));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _port = port == default ? throw new ArgumentNullException(nameof(port)) : port;
            _username = username;
            _password = password;
            _useSsl = useSsl;
            _emailSenderWorkMode = emailSenderWorkMode;
            _emailMessageRepository = emailMessageRepository;
            _auditMessagePublisher = auditMessagePublisher;
            _log = logFactory.CreateLog(this);

            _useAuth = username != null && password != null;

            if (string.IsNullOrWhiteSpace(emailSender))
                throw new ArgumentNullException(nameof(emailSender));
            _senderAddress = new MailboxAddress(emailSender);
            if (!string.IsNullOrEmpty(emailSenderName))
            {
                _senderAddress.Name = emailSenderName;
            }
        }

        public async Task ProcessMessageAsync(string email, string subject, string body, Guid messageId)
        {
            switch (_emailSenderWorkMode)
            {
                case EmailSenderWorkMode.Smtp:
                    await SendMessageAsync(email, subject, body, messageId);
                    break;
                case EmailSenderWorkMode.Storage:
                    await SaveMessageAsync(email, subject, body, messageId, true);
                    break;
                case EmailSenderWorkMode.SmtpAndStorage:
                    await SaveMessageAsync(email, subject, body, messageId, false);
                    await SendMessageAsync(email, subject, body, messageId);
                    break;
                default:
                    _log.Warning(
                        $"EmailSenderWorkMode not implemented: {_emailSenderWorkMode.ToString()}, Message {{MessageId}} could not be processed.");
                    break;
            }
        }

        private async Task SaveMessageAsync(string email, string subject, string body, Guid messageId, bool shouldAudit)
        {
            var emailMessage = new EmailMessage {MessageId = messageId, Email = email, Subject = subject, Body = body};

            await _emailMessageRepository.CreateAsync(emailMessage);

            if (shouldAudit)
            {
                await _auditMessagePublisher.PublishAsync(new UpdateAuditMessageEvent
                {
                    MessageId = messageId.ToString(),
                    DeliveryStatus = DeliveryStatus.Ok,
                    SentTimestamp = DateTime.UtcNow
                });
            }
        }

        private async Task SendMessageAsync(string email, string subject, string body, Guid messageId)
        {
            var auditMessage = new UpdateAuditMessageEvent {MessageId = messageId.ToString()};

            try
            {
                var message = new MimeMessage();

                message.From.Add(_senderAddress);
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;

                message.Body = new TextPart("html") {Text = body};

                using (var mailClient = _mailClientFactory.CreateMailClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    mailClient.ServerCertificateValidationCallback =
                        (sender, certificate, chain, sslPolicyErrors) => true;

                    await mailClient.ConnectAsync(_host, _port, _useSsl);

                    if (_useAuth)
                    {
                        await mailClient.AuthenticateAsync(_username, _password);
                    }

                    await mailClient.SendAsync(message);

                    auditMessage.DeliveryStatus = DeliveryStatus.Ok;
                    auditMessage.SentTimestamp = DateTime.UtcNow;
                    await _auditMessagePublisher.PublishAsync(auditMessage);

                    await mailClient.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                var errorMessage =
                    $"Mail with id \"{messageId}\" could not be sent for email: \"{email.SanitizeEmail()}\" with subject: \"{subject}\"";
                _log.Error(e, errorMessage);
                auditMessage.DeliveryStatus = DeliveryStatus.Error;
                auditMessage.SentTimestamp = DateTime.UtcNow;
                auditMessage.DeliveryComment = $"{errorMessage}{Environment.NewLine}{e.Message}";
                await _auditMessagePublisher.PublishAsync(auditMessage);
            }
        }
    }
}
