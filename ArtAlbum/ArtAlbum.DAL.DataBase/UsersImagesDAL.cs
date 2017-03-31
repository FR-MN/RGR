using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using ArtAlbum.DAL.DataBase.Exceptions;
using ArtAlbum.Entities;

namespace ArtAlbum.DAL.DataBase
{
    public class UsersImagesDAL : IUsersImagesDAL
    {
        private class Relation
        {
            public Guid UserId;
            public Guid ImageId;
        }
       

        private string connectionString;

        public UsersImagesDAL()
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
        public bool AddRelation(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }        
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersImages(userId,imageId) VALUES(@userId, @imageId)", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@imageId", imageId);                
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<Guid> GetImagesIdsByUserId(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,imageId FROM UsersImages WHERE userId=@userId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["imageId"];
                }
                throw new NotFoundDataException("images not found");
            }
        }

        public IEnumerable<Guid> GetUsersIdsByImageId(Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,imageId FROM UsersImages WHERE imageId=@imageId", connection);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return(Guid)reader["userId"];
                }
                throw new NotFoundDataException("users not found");
            }
        }

        public bool RemoveRelation(Guid userId, Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersImages WHERE userId=@userId AND imagesId=@imagesId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
