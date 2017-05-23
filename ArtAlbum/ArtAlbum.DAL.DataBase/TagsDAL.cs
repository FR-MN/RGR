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
    public class TagsDAL : ITagsDAL
    {
        private static string connectionString;

        public TagsDAL()
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

        public bool AddTag(TagDTO tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag data is null");
            }
            foreach (var tagData in GetAllTags())
            {
                if (tagData.Id == tag.Id)
                {
                    throw new ArgumentException("tag already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Tags(Id, Name) VALUES(@Id, @Name)", connection);
                command.Parameters.AddWithValue("@Id", tag.Id);
                command.Parameters.AddWithValue("@Name", tag.Name);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddTagToImage(Guid tagId, Guid imageId)
        {
            if (tagId == null || imageId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO ImagesTags(tagId,imageId) VALUES(@tagId, @imageId)", connection);
                command.Parameters.AddWithValue("@tagId", tagId);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<TagDTO> GetAllTags()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Name FROM Tags", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new TagDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
            }
        }

        public TagDTO GetTagById(Guid tagId)
        {
            if (tagId == null)
            {
                throw new ArgumentNullException("tag id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,Name FROM Comments WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", tagId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new TagDTO()
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"]
                    };
                }
                throw new NotFoundDataException("comment not found");
            }
        }

        public IEnumerable<Guid> GetTagsByImageId(Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT tagId,imageId FROM ImagesTags WHERE imageId=@imageId", connection);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["tagId"];
                }
            }
        }

        public IEnumerable<Guid> GetImagesByTagId(Guid tagId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT tagId,imageId FROM ImagesTags WHERE tagId=@tagId", connection);
                command.Parameters.AddWithValue("@tagId", tagId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["imageId"];
                }
            }
        }

        public bool RemoveTagFromImage(Guid tagId, Guid imageId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM ImagesTags WHERE tagId=@tagId AND imageId=@imageId", connection);
                command.Parameters.AddWithValue("@tagId", tagId);
                command.Parameters.AddWithValue("@imageId", imageId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveTag(Guid tagId)
        {
            if (tagId == null)
            {
                throw new ArgumentNullException("tag id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Tags WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", tagId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
