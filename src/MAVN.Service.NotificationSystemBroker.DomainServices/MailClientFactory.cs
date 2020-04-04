using MailKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAVN.Service.NotificationSystemBroker.DomainServices
{
    public class MailClientFactory
    {
        private readonly Func<IMailTransport> _mailClientCreator;

        public MailClientFactory(Func<IMailTransport> mailClientCreator)
        {
            _mailClientCreator = mailClientCreator;
        }

        public IMailTransport CreateMailClient()
        {
            return _mailClientCreator();
        }
    }
}
