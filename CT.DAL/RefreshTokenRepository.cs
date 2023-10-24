using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace CT.Services
{
    public class RefreshTokenRepository
    {
        private readonly string _connectionString;

        public RefreshTokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveRefreshToken(int userId, string token, DateTime expiryDateTime)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO RefreshTokens (UserId, Token, ExpiryDateTime) VALUES (@UserId, @Token, @ExpiryDateTime)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@ExpiryDateTime", expiryDateTime);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RevokeRefreshToken(string token)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM RefreshTokens WHERE Token = @Token";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool IsRefreshTokenValid(int userId, string token)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM RefreshTokens WHERE UserId = @UserId AND Token = @Token AND ExpiryDateTime > GETDATE()";
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Token", token);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
