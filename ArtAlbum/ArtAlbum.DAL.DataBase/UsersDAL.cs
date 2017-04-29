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
                connectionString = ConfigurationManager.ConnectionStrings["ArtAlbumDB"].ConnectionString;
            }
            catch (Exception e)
            {
                throw new ConfigurationFileException("error in configuration file", e);
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
                if (userData.Id == user.Id)
                {
                    throw new ArgumentException("user already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Users(Id, FirstName, LastName, Nickname, DateOfBirth, Email, HashOfPassword) VALUES(@Id, @FirstName, @LastName, @Nickname, @DateOfBirth, @Email, @HashOfPassword)", connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Nickname", user.Nickname);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Email", user.Email);
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
                SqlCommand command = new SqlCommand("SELECT Id,FirstName,LastName,Nickname,DateOfBirth,Email,HashOfPassword FROM Users", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new UserDTO()
                    {
                        Id = (Guid)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Nickname = (string)reader["Nickname"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Email = (string)reader["Email"],
                        HashOfPassword = (byte[])reader["HashOfPassword"]
                    };
                }
            }
        }

        public UserDTO GetUserById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,FirstName,LastName,Nickname,DateOfBirth,Email,HashOfPassword FROM Users WHERE Id=@Id", connection);
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
                        Nickname = (string)reader["Nickname"],
                        DateOfBirth = (DateTime)reader["DateOfBirth"],
                        Email = (string)reader["Email"],
                        HashOfPassword = (byte[])reader["HashOfPassword"]
                    };
                }
                throw new NotFoundDataException("user not found");
            }
        }

        public bool RemoveUserById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
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
            if (user == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Users SET Id=@Id, FirstName=@FirstName, LastName=@LastName, Nickname=@Nickname, DateOfBirth=@DateOfBirth, Email=@Email, HashOfPassword=@HashOfPassword WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Nickname", user.Nickname);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@HashOfPassword", user.HashOfPassword);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
