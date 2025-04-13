using AgroMind.GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class MessageSpecification : BaseSpecifications<Messages, string>
	{
        // Get all messages with related Sender and Receiver
        public MessageSpecification() : base()
        {
            Includes.Add(m => m.SenderId);
            Includes.Add(m => m.ReceiverId);
        }

        // Get a specific message by ID
        public MessageSpecification(string id) : base(m => m.Id.Equals(id))
        {
            Includes.Add(m => m.SenderId);
            Includes.Add(m => m.ReceiverId);
        }
    }
}
