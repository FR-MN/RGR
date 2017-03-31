using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.DataBase.Exceptions;
using System.Configuration;
using System.Data.SqlClient;

namespace ArtAlbum.DAL.DataBase
{
    public class ImagesDAL : IImagesDAL
    {
        private string connectionString;

        public ImagesDAL()
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
        public bool AddImage(ImageDTO image)
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
                SqlCommand command = new SqlCommand("INSERT INTO Images(Id, Description, DateOfCreating) VALUES(@Id, @Description, @DateOfCreating)", connection);
                command.Parameters.AddWithValue("@Id", image.Id);
                command.Parameters.AddWithValue("@Description", image.Description);
                command.Parameters.AddWithValue("@DateOfCreating", image.DateOfCreating);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<ImageDTO> GetAllImages()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Description,DateOfCreating FROM Images", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new ImageDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Description = (string)reader["Description"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"]
                    };
                }
            }
        }

        public ImageDTO GetImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Description,DateOfCreating FROM Images WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new ImageDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Description = (string)reader["Description"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"]
                    };
                }
                throw new NotFoundDataException("image not found");
            }
        }

        public bool RemoveImageById(Guid imageId)
        {
            if (imageId == null)
            {
                throw new ArgumentNullException("image id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Images WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool UpdateImage(ImageDTO image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image data is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE Images SET Id=@Id, Description=@Description, DateOfCreating=@DateOfCreating WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", image.Id);
                command.Parameters.AddWithValue("@Description", image.Description);
                command.Parameters.AddWithValue("@DateOfCreating", image.DateOfCreating);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
