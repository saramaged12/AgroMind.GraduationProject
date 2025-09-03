using AgroMind.GP.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroMind.GP.Core.Specification
{
	public class OrderSpecification : BaseSpecifications<Order, int>
	{
		
		//Get All Orders For Specific User By BuyerEmail
		public OrderSpecification(string BuyerEmail):base(order=>order.BuyerEmail==BuyerEmail)
		{
			AddInclude(order => order.DeliveryMethod!); 
			AddInclude(order => order.OrderItems);
			AddOrderByDescending(order => order.OrderDate);
		}

		//Get Order By Id And BuyerEmail
		public OrderSpecification(string BuyerEmail, int OrderId) : base(order => order.BuyerEmail == BuyerEmail && order.Id == OrderId)
		{
			AddInclude(order => order.DeliveryMethod!);
			AddInclude(order => order.OrderItems);

		}

	}
}
