using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using ShopWeb.Models;

namespace ShopWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSendercs _emailSender;

        public ContactController(IEmailSendercs emailSendercs)
        {
            this._emailSender = emailSendercs;
        }

        public IActionResult Index(string? status)
        {
            if(status != null)
            {
                ViewBag.Status = "Submit success !";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail(SendMailDTO sendMailDTO)
        {
            await _emailSender.SendMailAsync(sendMailDTO.Name, sendMailDTO.Email, sendMailDTO.Subject, sendMailDTO.Message);

            return RedirectToAction("Index", new {status = "suss"});
            
        }
    }
}
