using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentEmail.Core;
using System.Net.Mail;
using System.Net;
using FluentEmail.Smtp;
using System;

namespace EnvioEmailDemo.Controllers
{
    [ApiController]
    [Route("emails")]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(EmailEntity email)
        {
            var sender = new SmtpSender(() => new SmtpClient(email.Host, email.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email.From, email.Password),
                EnableSsl = true
            });

            Email.DefaultSender = sender;

            await Email
                .From(email.From)
                .To(email.ToEmail, email.ToName)
                .Subject(email.Subject)
                .Body(email.Body)
                .SendAsync();

            return Ok(new { success = true });
        }
    }
}
