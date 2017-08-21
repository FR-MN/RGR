using ArtAlbum.DAL.Abstract;
using ArtAlbum.DAL.DataBase;
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
    public class QuestionsDAL : IQuestionsDAL
    {
        private static string connectionString;

        public QuestionsDAL()
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

        public bool AddQuestion(QuestionDTO question)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question data is null");
            }
            foreach (var questionData in GetAllQuestions())
            {
                if (questionData.Id == question.Id)
                {
                    throw new ArgumentException("question already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Questions(Id, DateOfCreating, Caption) VALUES(@Id, @DateOfCreating, @Caption)", connection);
                command.Parameters.AddWithValue("@Id", question.Id);
                command.Parameters.AddWithValue("@DateOfCreating", question.DateOfCreating);
                command.Parameters.AddWithValue("@Caption", question.Caption);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddQuestionToUser(Guid questionId, Guid userId)
        {
            if (questionId == null || userId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersQuestions(questionId,userId) VALUES(@questionId, @userId)", connection);
                command.Parameters.AddWithValue("@questionId", questionId);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<QuestionDTO> GetAllQuestions()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,DateOfCreating,Caption FROM Questions", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new QuestionDTO()
                    {
                        Id = (Guid)reader["Id"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"],
                        Caption = (string)reader["Caption"]
                    };
                }
            }
        }

        public QuestionDTO GetQuestionById(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,DateOfCreating,Caption FROM Questions WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", questionId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return new QuestionDTO()
                    {
                        Id = (Guid)reader["Id"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"],
                        Caption = (string)reader["Caption"]
                    };
                }
                throw new NotFoundDataException("question not found");
            }
        }

        public IEnumerable<Guid> GetUsersIdsByQuestionId(Guid questionId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,questionId FROM UsersQuestions WHERE questionId=@questionId", connection);
                command.Parameters.AddWithValue("@questionId", questionId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["userId"];
                }
            }
        }

        public bool RemoveQuestionById(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Questions WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", questionId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveQuestionFromUser(Guid questionId, Guid userId)
        {
            if (userId == null || questionId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersQuestions WHERE userId=@userId AND questionId=@questionId", connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@questionId", questionId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }
    }
}
