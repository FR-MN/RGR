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
    public class CommentsDAL : ICommentsDAL
    {
        private static string connectionString;

        public CommentsDAL()
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

        public bool AddComment(CommentDTO comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("user data is null");
            }
            foreach (var commentData in GetAllComments())
            {
                if (commentData.Id == comment.Id)
                {
                    throw new ArgumentException("user already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Comments(Id, Data, DateOfCreating, AuthorId) VALUES(@Id, @Data, @DateOfCreating, @AuthorId)", connection);
                command.Parameters.AddWithValue("@Id", comment.Id);
                command.Parameters.AddWithValue("@Data", comment.Data);
                command.Parameters.AddWithValue("@DateOfCreating", comment.DateOfCreating);
                command.Parameters.AddWithValue("@AuthorId", comment.AuthorId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddCommentToImage(Guid commentId, Guid imageId)
        {
            if (commentId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO ImagesComments(commentId,imageId) VALUES(@commentId, @imageId)", connection);
                command.Parameters.AddWithValue("@commentId", commentId);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<CommentDTO> GetAllComments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Data,DateOfCreating,AuthorId FROM Comments", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new CommentDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Data = (string)reader["Data"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"],
                        AuthorId = (Guid)reader["AuthorId"]
                    };
                }
            }
        }

        public CommentDTO GetCommentById(Guid commentId)
        {
            if (commentId == null)
            {
                throw new ArgumentNullException("user id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Data,DateOfCreating,AuthorId FROM Comments WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", commentId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new CommentDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Data = (string)reader["Data"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"],
                        AuthorId = (Guid)reader["AuthorId"]
                    };
                }
                throw new NotFoundDataException("comment not found");
            }
        }

        public IEnumerable<Guid> GetCommentsByImageId(Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT commentId,imageId FROM ImagesComments WHERE imageId=@imageId", connection);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["commentId"];
                }
            }
        }

        public Guid GetImageByCommentId(Guid commentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT commentId,imageId FROM ImagesComments WHERE commentId=@commentId", connection);
                command.Parameters.AddWithValue("@commentId", commentId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return (Guid)reader["imageId"];
                }
                throw new NotFoundDataException("image not found");
            }
        }

        public bool RemoveCommentFromImage(Guid commentId, Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM ImagesComments WHERE commentId=@commentId AND imageId=@imageId", connection);
                command.Parameters.AddWithValue("@commentId", commentId);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveCommment(Guid commentId)
        {
            if (commentId == null)
            {
                throw new ArgumentNullException("comment id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Comments WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", commentId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
