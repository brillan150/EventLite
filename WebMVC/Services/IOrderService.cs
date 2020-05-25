using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        // Idea for future expansion:
        //Task<List<Order>> GetOrdersByUser(ApplicationUser user);
        Task<Order> GetOrder(string orderId);
        Task<int> CreateOrder(Order order);
        // Ideas for future expansion:
        //  Order MapUserInfoIntoOrder(ApplicationUser user, Order order);
        //  void OverrideUserInfoIntoOrder(Order original, Order destination);
    }
}
