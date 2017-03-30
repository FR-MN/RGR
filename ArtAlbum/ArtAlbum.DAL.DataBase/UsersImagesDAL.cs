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
            public Guid AwardId;
        }
       

        private string connectionString;

        public UsersImagesDAL()
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
        public bool AddRelation(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)// добавить проверку на повторяющиеся Id
            {
                throw new ArgumentNullException("user data is null");
            }          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersImages(userId,imgeId) VALUES(@userId, @imgeId)", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@imgeId", imageId);                
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<Guid> GetImagesIdsByUserId(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,imgeId FROM UsersImages WHERE userId=@userId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var reader = command.ExecuteReader();

              
                Queue<Guid> result;
                while (reader.Read())
                {
                    
                    Guid temp = (Guid)reader["userId"];
                    
                       
                  
                }
                throw new NotFoundDataException("Image not found");
            }
        }

        public IEnumerable<Guid> GetUsersIdsByImageId(Guid imageId)
        {
            throw new Exception();
        }

        public bool RemoveRelation(Guid userId, Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersImages WHERE userId=@userId ", connection);//как ставить двойные условие у WHERE
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@imageId", imageId);

                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
