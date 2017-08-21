using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using ArtAlbum.Entities;
using ArtAlbum.DAL.DataBase.Exceptions;
using System.Configuration;
using System.Data.SqlClient;

namespace ArtAlbum.DAL.DataBase
{
    public class QImagesDAL : IQImagesDAL
    {
        private static string connectionString;

        public QImagesDAL()
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

        public bool AddImage(QImageDTO image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            foreach (var imageData in GetAllImages())
            {
                if (imageData.Id == image.Id)
                {
                    throw new ArgumentException("image already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO QImages(Id, Data, Type) VALUES(@Id, @Data, @Type)", connection);
                command.Parameters.AddWithValue("@Id", image.Id);
                command.Parameters.AddWithValue("@Data", image.Data);
                command.Parameters.AddWithValue("@Type", image.Type);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddImageToQuestion(Guid qimageId, Guid questionId)
        {
            if (qimageId == null || questionId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO QuestionsQImages(questionId,qimageId) VALUES(@questionId, @qimageId)", connection);
                command.Parameters.AddWithValue("@questionId", questionId);
                command.Parameters.AddWithValue("@qimageId", qimageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public QImageDTO GetImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Data,Type FROM QImages WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new QImageDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Data = (byte[])reader["Data"],
                        Type = (string)reader["Type"]
                    };
                }
                throw new NotFoundDataException("image not found");
            }
        }

        public IEnumerable<Guid> GetImagesIdsByQuestionId(Guid questionId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT questionId,qimageId FROM QuestionsQImages WHERE questionId=@questionId", connection);
                command.Parameters.AddWithValue("@questionId", questionId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["qimageId"];
                }
            }
        }

        public bool RemoveImage(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM QImages WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveImageFromQuestion(Guid imageId, Guid questionId)
        {
            if (imageId == null || questionId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM QuestionsQImages WHERE imageId=@imageId AND questionId=@questionId", connection);
                command.Parameters.AddWithValue("@imageId", imageId);
                command.Parameters.AddWithValue("@questionId", questionId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        private IEnumerable<QImageDTO> GetAllImages()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Data,Type FROM QImages", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new QImageDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Data = (byte[])reader["Data"],
                        Type = (string)reader["Type"]
                    };
                }
            }
        }
    }
}
