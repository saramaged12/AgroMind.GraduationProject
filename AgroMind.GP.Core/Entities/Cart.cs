using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities
{
    public class Cart
    {


        public string id { get; set; } //Guid

        public List<CartItem> items { get; set; }

        public Cart(string id)
        {
            this.id = id;
        }

    }
}