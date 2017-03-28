using ArtAlbum.DAL.Abstract;
using ArtAlbum.DAL.DataBase.Exceptions;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.DataBase
{
    public class UsersDAL : IUsersDAL
    {
        private static string connectionString;

        public UsersDAL()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            }
            catch (Exception e)
            {
                throw new ConfigurationFileException("Error in configuration file", e);
            }
        }

        public bool AddUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            foreach (var userData in GetAllUsers())
            {
                if (userData.Email == user.Email)
                {
                    throw new ArgumentException("User already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Users(Id, FirstName, LastName, DateOfBirth, Email, HashOfPassword) VALUES(@Id, @FirstName, @LastName, @DateOfBirth, @Email, @HashOfPassword)", connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@HashOfPassword", user.HashOfPassword);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,FirstName,LastName,DateOfBirth,Email,HashOfPassword FROM Users", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new UserDTO()
                    {
                        Id = (Guid)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Email = (string)reader["Email"],
                        HashOfPassword = (int)reader["HashOfPassword"]
                    };
                }
            }
        }

        public UserDTO GetUserById(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,FirstName,LastName,DateOfBirth,Email,HashOfPassword FROM Users WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new UserDTO()
                    {
                        Id = (Guid)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Email = (string)reader["Email"],
                        HashOfPassword = (int)reader["HashOfPassword"]
                    };
                }
                throw new NotFoundDataException("User not found");
            }
        }

        public bool RemoveUserById(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool UpdateUser(UserDTO user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Users SET Id=@Id, FirstName=@FirstName, LastName=@LastName, DateOfBirth=@DateOfBirth, Email=@Email, HashOfPassword=@HashOfPassword WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@HashOfPassword", user.HashOfPassword);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
