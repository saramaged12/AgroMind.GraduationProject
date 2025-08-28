using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Entities.Orders
{
	public class Order : BaseEntity<int>
	{
		public required string BuyerEmail { get; set; }

		public  DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow; //Date Of Order Creation

		public  OrderStatus Status { get; set; } = OrderStatus.Pending;

		public required Address ShippingAddress { get; set; }
		public int? DeliveryMethodId { get; set; } //1 to Many
		public virtual DeliveryMethod? DeliveryMethod { get; set; }

		public virtual ICollection<OrderItems> OrderItems { get; set; } =new HashSet<OrderItems>();

		public decimal Subtotal { get; set; }  //Cost Of Order without Delivery

		
		public decimal GetTotal() => Subtotal + DeliveryMethod!.Cost;

		public string PaymentIntentId { get; set; } = " "; //For Stripe Payment

		
	}
	
}
