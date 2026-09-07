using Microsoft.Data.SqlClient;
using System.Data;
using EventApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString = "Data Source=Huawei_J\\SQLExpress;Initial Catalog=EventAppDB;User ID=pro;Password=1234;Trust Server Certificate=True";

        public async Task<User> Authenticate(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM [User] WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            Mail = reader.GetString(3),
                            IsAdmin = reader.GetBoolean(4)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<bool> Register(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO [User] (username, password, mail, IsAdmin) VALUES (@username, @password, @mail, @isAdmin)", connection);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@mail", user.Mail);
                command.Parameters.AddWithValue("@isAdmin", user.IsAdmin);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE [User] SET password = @password WHERE userId = @userId", connection);
                command.Parameters.AddWithValue("@password", newPassword);
                command.Parameters.AddWithValue("@userId", userId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var events = new List<Event>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM [Event]", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        events.Add(new Event
                        {
                            EventId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            Location = reader.GetString(3),
                            Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                            AvailableTickets = reader.GetInt32(5),
                            Price = reader.GetDecimal(6)
                        });
                    }
                }
            }
            return events;
        }

        public async Task<bool> AddToCart(int userId, int eventId, int quantity)
        {
            return true;
        }

        public async Task<bool> AddOrderToCart(Order order)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "INSERT INTO [Order] (userId, eventId, totalAmount, orderDate) VALUES (@userId, @eventId, @totalAmount, @orderDate)";
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@userId", order.UserId);
                command.Parameters.AddWithValue("@eventId", order.EventId);
                command.Parameters.AddWithValue("@totalAmount", order.TotalAmount);
                command.Parameters.AddWithValue("@orderDate", order.OrderDate);

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order to cart: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Ticket>> GetUserCart(int userId)
        {
            return new List<Ticket>();
        }

        public async Task<bool> Checkout(int userId)
        {
            return true;
        }

        public async Task<List<Order>> GetOrdersForUser(int userId)
        {
            var orders = new List<Order>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT OrderId, UserId, EventId, TotalAmount, OrderDate FROM [Order] WHERE UserId = @userId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    EventId = reader.GetInt32(2),
                    TotalAmount = reader.GetDecimal(3),
                    OrderDate = reader.GetDateTime(4)
                });
            }

            return orders;
        }

        public async Task<Event> GetEventById(int eventId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT EventId, Title, Date, Location, Description, AvailableTickets, Price FROM [Event] WHERE EventId = @eventId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@eventId", eventId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Event
                {
                    EventId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    Location = reader.GetString(3),
                    Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                    AvailableTickets = reader.GetInt32(5),
                    Price = reader.GetDecimal(6)
                };
            }

            return null;
        }

        public async Task<bool> DeleteOrderId(int orderId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("DELETE FROM [Order] WHERE orderId = @orderId", connection);
                    command.Parameters.AddWithValue("@orderId", orderId);

                    await connection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();

                    // Проверим сколько строк было удалено
                    if (result > 0)
                    {
                        Console.WriteLine($"Deleted {result} rows from Order table.");
                        return true;
                    }

                    Console.WriteLine("No rows were deleted.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order: {ex.Message}");
                return false;
            }
        }
    }
}