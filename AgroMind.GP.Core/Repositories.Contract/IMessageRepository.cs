using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Repositories.Contract
{
    interface IMessageRepository
    {
        //IEnumerable ---> returns a collection of Message objects (a list, array, or any enumerable type)
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message> GetMessageByIdAsync(int messageId);
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(int messageId);

    }
}
