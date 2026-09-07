using EventApp.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public class AuthService
    {
        private readonly IDatabaseService _databaseService;
        private readonly string _connectionString = "Data Source=Huawei_J\\SQLEspress;Initial Catalog=EventAppDB;User ID=pro;Password=1234;Trust Server Certificate=True";
        private User _currentUser;

        public AuthService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public User CurrentUser => _currentUser;

        public async Task<bool> Login(string username, string password)
        {
            _currentUser = await _databaseService.Authenticate(username, password);
            return _currentUser != null;
        }

        public async Task<bool> Register(User user)
        {
            return await _databaseService.Register(user);
        }

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            return await _databaseService.UpdatePassword(userId, newPassword);
        }

        public async Task<bool> ResetPassword(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE [User] SET password = 'temp_password' WHERE mail = @email",
                    connection);
                command.Parameters.AddWithValue("@email", email);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}