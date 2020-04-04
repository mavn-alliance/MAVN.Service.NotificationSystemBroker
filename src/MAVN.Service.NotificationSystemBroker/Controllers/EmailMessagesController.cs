using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Service.NotificationSystemBroker.Client;
using MAVN.Service.NotificationSystemBroker.Client.Models;
using MAVN.Service.NotificationSystemBroker.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MAVN.Service.NotificationSystemBroker.Controllers
{
    [Route("/api/")]
    [ApiController]
    public class EmailMessagesController : Controller, IEmailMessagesApi
    {
        private readonly IEmailMessagesService _emailMessagesService;
        private readonly IMapper _mapper;

        public EmailMessagesController(IEmailMessagesService emailMessagesService, IMapper mapper)
        {
            _emailMessagesService = emailMessagesService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        /// <response code="200">Success</response>
        [HttpPost("emailMessages")]
        [SwaggerOperation("RetrieveEmailMessages")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IEnumerable<EmailMessageResponseModel>> RetrieveEmailMessages([FromBody, Required]string email)
        {
            var entities = await _emailMessagesService.RetrieveEmailMessages(email);
            var models = entities.Select(e => _mapper.Map<EmailMessageResponseModel>(e));

            return models;
        }
    }
}
