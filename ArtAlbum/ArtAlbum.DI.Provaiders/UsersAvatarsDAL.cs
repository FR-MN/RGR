using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using System.Configuration;
using ArtAlbum.DAL.DataBase.Exceptions;
using System.Data.SqlClient;

namespace ArtAlbum.DAL.DataBase
{
    public class UsersAvatarsDAL : IUsersAvatarsDAL
    {
        private static string connectionString;

        public UsersAvatarsDAL()
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

        public bool AddUserAvatar(UserAvatarDTO userAvatar)
        {
            if (userAvatar == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Users SET Id=@Id, AvatarData=@AvatarData, AvatarType=@AvatarType WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userAvatar.UserId);
                command.Parameters.AddWithValue("@AvatarData", userAvatar.Data);
                command.Parameters.AddWithValue("@AvatarType", userAvatar.Type);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public UserAvatarDTO GetUserAvatarById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,AvatarData,AvatarType FROM Users WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    byte[] data;
                    string type;
                    if (reader.IsDBNull(reader.GetOrdinal("AvatarData")))
                    {
                        data = null;
                    }
                    else
                    {
                        data = (byte[])reader["AvatarData"];
                    }
                    if (reader.IsDBNull(reader.GetOrdinal("AvatarType")))
                    {
                        type = null;
                    }
                    else
                    {
                        type = (string)reader["AvatarType"];
                    }

                    return new UserAvatarDTO()
                    {
                        UserId = (Guid)reader["Id"],
                        Data = data,
                        Type = type
                    };
                }
                throw new NotFoundDataException("user not found");
            }
        }

        public bool RemoveUserAvatarById(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Users SET Id=@Id, AvatarData=@AvatarData, AvatarType=@AvatarType WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", userId);
                command.Parameters.AddWithValue("@AvatarData", null);
                command.Parameters.AddWithValue("@AvatarType", null);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
