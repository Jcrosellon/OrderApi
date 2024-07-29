using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessagesController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            if (messageRequest == null || string.IsNullOrWhiteSpace(messageRequest.PhoneNumber) || string.IsNullOrWhiteSpace(messageRequest.Message))
            {
                return BadRequest("Invalid message request.");
            }

            try
            {
                await _messageService.SendMessageAsync(messageRequest.PhoneNumber, messageRequest.Message);
                return Ok("Message sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/messages
        [HttpGet]
        public ActionResult<IEnumerable<MessageRequest>> GetMessages()
        {
            // Simulación de datos
            var messages = new List<MessageRequest>
            {
                new MessageRequest { Id = 1, PhoneNumber = "3183192913", Message = "Hello John Doe, your order with ID 1 is confirmed." }
                // Agrega más mensajes simulados si es necesario
            };

            return Ok(messages);
        }

        // GET api/messages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageRequest>> GetMessageById(int id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
    }
}
