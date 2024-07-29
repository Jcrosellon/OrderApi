using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderApi.Models;

namespace OrderApi.Services
{
    public class MessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public MessageService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MessageRequest>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<MessageRequest> GetMessageByIdAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            return message ?? throw new Exception("No message found with the specified id");
        }

        public async Task<MessageRequest> CreateMessageAsync(MessageRequest messageRequest)
        {
            _context.Messages.Add(messageRequest);
            await _context.SaveChangesAsync();
            return messageRequest;
        }

        public async Task UpdateMessageAsync(MessageRequest messageRequest)
        {
            _context.Entry(messageRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var messageRequest = await _context.Messages.FindAsync(id);
            if (messageRequest != null)
            {
                _context.Messages.Remove(messageRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendMessageAsync(string phoneNumber, string message)
        {
            var requestPayload = new
            {
                phone = phoneNumber,
                body = message
            };

            var jsonContent = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.whatsapp.com/send", content);

            if (!response.IsSuccessStatusCode)
            {
                // Manejo de errores en caso de que la solicitud falle
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send message: {response.StatusCode} - {errorMessage}");
            }
        }
    }
}
