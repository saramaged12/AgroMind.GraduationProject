using AgroMind.GP.APIs.DTOs;
using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Entities.ProductModule;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgroMind.GP.APIs.Controllers
{

    public class MessageController : APIbaseController
    {
        private readonly IGenericRepositories<Message, string> _messagerepo;

        public MessageController(IGenericRepositories<Message, string> messagerepo)
        {
            _messagerepo = messagerepo;
        }


        //Get All
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var SpecMessage = new MessageSpecification();
            var messages = await _messagerepo.GetAllWithSpecASync(SpecMessage);
            return Ok(messages);
        }

        //Update or Create New Message
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            if (message == null) return BadRequest("Invalid message data.");
            await _messagerepo.AddAsync(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = message.Id }, message);
        }

        //Delete Message
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            var specMessage = new MessageSpecification(id);
            var message = await _messagerepo.GetByIdAWithSpecAsync(specMessage);

            if (message == null)

                return NotFound();

            _messagerepo.Delete(message);
            return Ok($"Message with ID {id} deleted successfully.");
        }

        //Get Message By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessageById(string id) //<ActionResult<Message> : will return Land
        {
            var SpecMessage = new MessageSpecification(id);
            var message = await _messagerepo.GetByIdAWithSpecAsync(SpecMessage);
            return Ok(message);
        }

    }
}