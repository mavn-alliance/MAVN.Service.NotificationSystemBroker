using System;
using System.Linq;
using System.Threading.Tasks;
using MAVN.Service.NotificationSystemBroker.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.NotificationSystemBroker.Controllers
{

    [Route("/emails")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmailVisualizationController : Controller
    {
        private readonly IEmailMessageRepository _emailMessageRepository;

        public EmailVisualizationController(IEmailMessageRepository emailMessageRepository)
        {
            _emailMessageRepository = emailMessageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var mails = await _emailMessageRepository.GetLastFiftyTodayEmailsAsync();

            return View(mails.ToList());
        }

        [HttpGet("/details")]
        public async Task<IActionResult> Details(Guid messageId)
        {
            var mail = await _emailMessageRepository.GetMessageByMessageIdAsync(messageId);

            return View(mail);
        }
    }
}
