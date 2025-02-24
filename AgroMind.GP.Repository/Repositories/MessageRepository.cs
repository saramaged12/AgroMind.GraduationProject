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

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

    }
}
