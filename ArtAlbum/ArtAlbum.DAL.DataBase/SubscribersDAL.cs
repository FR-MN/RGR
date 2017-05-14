using ArtAlbum.DAL.Abstract;
using ArtAlbum.DAL.DataBase.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlbum.DAL.DataBase
{
    public class SubscribersDAL : ISubscribersDAL
    {
        private string connectionString;

        public SubscribersDAL()
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
        public bool AddSubscriberToUser(Guid subscriberId, Guid userId)
        {
            if (userId == null || subscriberId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersSubscribers(userId,subscriberId) VALUES(@userId, @subscriberId)", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@subscriberId", subscriberId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<Guid> GetSubscribersOfUser(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,subscriberId FROM UsersSubscribers WHERE userId=@userId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["subscriberId"];
                }
            }
        }

        public IEnumerable<Guid> GetSubscriptionsOfUser(Guid subscriberId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,subscriberId FROM UsersSubscribers WHERE subscriberId=@subscriberId", connection);
                command.Parameters.AddWithValue("@subscriberId", subscriberId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["userId"];
                }
            }
        }

        public bool RemoveSubscriberFromUser(Guid subscriberId, Guid userId)
        {
            if (userId == null || subscriberId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersSubscribers WHERE userId=@userId AND subscriberId=@subscriberId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@subscriberId", subscriberId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
