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
    public class LikesDAL : ILikesDAL
    {
        private string connectionString;

        public LikesDAL()
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
        public bool AddLikeToImage(LikeDTO like, Guid imageId)
        {
            if (like == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO ImagesLikes(ImageId,LikerId,DateOfLike) VALUES(@ImageId, @LikerId, @DateOfLike)", connection);
                command.Parameters.AddWithValue("@ImageId", imageId);
                command.Parameters.AddWithValue("@LikerId", like.LikerId);
                command.Parameters.AddWithValue("@DateOfLike", like.DateOfLike);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<LikeDTO> GetLikesByImageId(Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT ImageId,LikerId,DateOfLike FROM ImagesLikes WHERE ImageId=@ImageId", connection);
                command.Parameters.AddWithValue("@ImageId", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new LikeDTO()
                    {
                        LikerId = (Guid)reader["LikerId"],
                        DateOfLike = (DateTime)reader["DateOfLike"]
                    };
                }
            }
        }

        public IEnumerable<Guid> GetIdsOfLikedImagesByUserId(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT ImageId,LikerId,DateOfLike FROM ImagesLikes WHERE LikerId=@LikerId", connection);
                command.Parameters.AddWithValue("@LikerId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["ImageId"];
                }
            }
        }

        public bool RemoveLikeFromImage(Guid userId, Guid imageId)
        {
            if (userId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM ImagesLikes WHERE ImageId=@ImageId AND LikerId=@LikerId", connection);
                command.Parameters.AddWithValue("@ImageId", imageId);
                command.Parameters.AddWithValue("@LikerId", userId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
