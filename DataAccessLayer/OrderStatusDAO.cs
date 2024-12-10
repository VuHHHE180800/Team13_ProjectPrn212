using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer
{
    public class OrderStatusDAO
    {
        private readonly Team13QlbhContext _context;

        public OrderStatusDAO(Team13QlbhContext context)
        {
            _context = context;
        }

        public List<OrderStatus> GetAllOrderStatuses()
        {
            return _context.Set<OrderStatus>()
                .Include(os => os.Orders)
                .ToList();
        }

        public OrderStatus GetOrderStatusById(int statusId)
        {
            return _context.Set<OrderStatus>()
                .Include(os => os.Orders)
                .FirstOrDefault(os => os.StatusId == statusId);
        }

        public void AddOrderStatus(OrderStatus orderStatus)
        {
            _context.Set<OrderStatus>().Add(orderStatus);
            _context.SaveChanges();
        }

        public void UpdateOrderStatus(OrderStatus orderStatus)
        {
            _context.Set<OrderStatus>().Update(orderStatus);
            _context.SaveChanges();
        }

        public void DeleteOrderStatus(int statusId)
        {
            var orderStatus = GetOrderStatusById(statusId);
            if (orderStatus != null)
            {
                _context.Set<OrderStatus>().Remove(orderStatus);
                _context.SaveChanges();
            }
        }

        public OrderStatus GetOrderStatusByName(string statusName)
        {
            return _context.Set<OrderStatus>()
                .Include(os => os.Orders)
                .FirstOrDefault(os => os.StatusName == statusName);
        }
    }
}