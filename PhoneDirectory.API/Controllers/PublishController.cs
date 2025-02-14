
using Microsoft.AspNetCore.Mvc;
using PhoneDirectory.API.Services;

namespace PhoneDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly MessagePublisher _messagePublisher;

        public PublishController(MessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string message)
        {
            _messagePublisher.Publish(message);
            return Ok(new { status = "Message published", message });
        }
    }
}

