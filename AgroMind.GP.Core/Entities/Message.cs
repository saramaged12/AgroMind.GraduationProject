using AgroMind.GP.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
    public class Messages : BaseEntity<string>
    {

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public DateTime TimeStamp { get; set; }

        public Messages(string senderId, string receiverId, string content)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Type = "text";      // Default type
            TimeStamp = DateTime.UtcNow;
        }


    }
}
