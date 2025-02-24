using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
        private IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
		{
            _messageRepository = messageRepository;
		}

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageRepository.GetAllMessagesAsync();
            return Ok(messages);
        }

        //Update or Create New Message
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            await _messageRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, message);
        }

        //Delete Message
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _messageRepository.DeleteMessageAsync(id);
            return NoContent();
        }

        //Get or ReCreate Message
        [HttpGet ("{id}")]
		public async Task<ActionResult<Message>> GetMessageById(int id) //<ActionResult<Message> : will return Land
		{
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

	}
}
