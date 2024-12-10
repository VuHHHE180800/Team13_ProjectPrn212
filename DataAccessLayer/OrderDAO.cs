using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer
{
    public class OrderDAO
    {
        private readonly Team13QlbhContext _context;

        public OrderDAO(Team13QlbhContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Set<Order>()
                .Include(o => o.Employee)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Set<Order>()
                .Include(o => o.Employee)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public void AddOrder(Order order)
        {
            _context.Set<Order>().Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Set<Order>().Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                _context.Set<Order>().Remove(order);
                _context.SaveChanges();
            }
        }

        public List<Order> GetOrdersByCustomerName(string customerName)
        {
            return _context.Set<Order>()
                .Where(o => o.CustomerName.Contains(customerName))
                .Include(o => o.Employee)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ToList();
        }

        public List<Order> GetOrdersByDateRange(DateOnly startDate, DateOnly endDate)
        {
            return _context.Set<Order>()
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.Employee)
                .Include(o => o.Status)
                .Include(o => o.OrderDetails)
                .ToList();
        }
    }
}