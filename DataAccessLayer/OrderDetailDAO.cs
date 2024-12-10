using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer
{
    public class OrderDetailDAO
    {
        private readonly Team13QlbhContext _context;

        public OrderDetailDAO(Team13QlbhContext context)
        {
            _context = context;
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return _context.Set<OrderDetail>()
                .Include(od => od.Order)
                .Include(od => od.Product)
                .ToList();
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.Set<OrderDetail>()
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Order)
                .Include(od => od.Product)
                .ToList();
        }

        public OrderDetail GetOrderDetailByOrderIdAndProductId(int orderId, int productId)
        {
            return _context.Set<OrderDetail>()
                .FirstOrDefault(od => od.OrderId == orderId && od.ProductId == productId);
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            _context.Set<OrderDetail>().Add(orderDetail);
            _context.SaveChanges();
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.Set<OrderDetail>().Update(orderDetail);
            _context.SaveChanges();
        }

        public void DeleteOrderDetail(int orderId, int productId)
        {
            var orderDetail = GetOrderDetailByOrderIdAndProductId(orderId, productId);
            if (orderDetail != null)
            {
                _context.Set<OrderDetail>().Remove(orderDetail);
                _context.SaveChanges();
            }
        }

        public List<OrderDetail> GetOrderDetailsByProductId(int productId)
        {
            return _context.Set<OrderDetail>()
                .Where(od => od.ProductId == productId)
                .Include(od => od.Order)
                .Include(od => od.Product)
                .ToList();
        }
    }
}