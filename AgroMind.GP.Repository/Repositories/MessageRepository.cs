using AgroMind.GP.Core.Entities;
using AgroMind.GP.Core.Repositories.Contract;
using AgroMind.GP.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroMind.GP.Repository.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AgroMindContext _context; // Use your actual DbContext

        public MessageRepository(AgroMindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.FindAsync<Message>(messageId);
        }

        public async Task AddMessageAsync(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Message can not null or empty");

            var existingMessage = await _context.FindAsync<Message>(message.Id);

            _context.Entry(existingMessage).CurrentValues.SetValues(message);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var message = await _context.FindAsync<Message>(messageId);
            if (message == null)
                return false;

            _context.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
