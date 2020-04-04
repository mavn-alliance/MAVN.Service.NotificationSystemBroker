using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.NotificationSystemBroker.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace MAVN.Service.NotificationSystemBroker.Client
{
    /// <summary>
    /// EmailMessages client API interface.
    /// </summary>
    [PublicAPI]
    public interface IEmailMessagesApi
    {
        /// <summary>
        /// Retrieves all messages for a given email
        /// </summary>
        /// <param name="email">The email for which to retrieve messages</param>
        /// <returns>Status code 200 - Ok</returns>
        [Post("api/emailMessages")]
        Task<IEnumerable<EmailMessageResponseModel>> RetrieveEmailMessages([FromBody] string email);
    }
}
