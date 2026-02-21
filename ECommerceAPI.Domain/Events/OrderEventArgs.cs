using System;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Events
{
    public class OrderEventArgs : EventArgs
    {
        public Order Order { get; }

        public OrderEventArgs(Order order)
        {
            Order = order;
        }
    }
}
