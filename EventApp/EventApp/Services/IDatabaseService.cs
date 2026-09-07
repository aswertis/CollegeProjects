using EventApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public interface IDatabaseService
    {
        Task<User> Authenticate(string username, string password);
        Task<bool> Register(User user);
        Task<bool> UpdatePassword(int userId, string newPassword);
        Task<List<Event>> GetAllEvents();
        Task<bool> AddOrderToCart(Order order);
        Task<bool> AddToCart(int userId, int eventId, int quantity);
        Task<List<Ticket>> GetUserCart(int userId);
        Task<bool> Checkout(int userId);

        Task<List<Order>> GetOrdersForUser(int userId);
        Task<Event> GetEventById(int eventId);
        Task<bool> DeleteOrderId(int orderId);
    }
}