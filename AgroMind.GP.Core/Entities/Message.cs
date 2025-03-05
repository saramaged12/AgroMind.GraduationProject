using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
    public class Message : BaseEntity<int>
    {

        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public DateTime TimeStamp { get; set; }

        public Message(int senderId, int receiverId, string content)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Type = "text";      // Default type
            TimeStamp = DateTime.UtcNow;
        }


    }
}
