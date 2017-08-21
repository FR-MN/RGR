using ArtAlbum.DAL.Abstract;
using System;
using System.Collections.Generic;
using ArtAlbum.Entities;
using System.Configuration;
using ArtAlbum.DAL.DataBase.Exceptions;
using System.Data.SqlClient;

namespace ArtAlbum.DAL.DataBase
{
    public class AnswersDAL : IAnswersDAL
    {
        private static string connectionString;

        public AnswersDAL()
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

        public bool AddAnswer(AnswerDTO answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException("answer data is null");
            }
            foreach (var answerData in GetAllAnswers())
            {
                if (answerData.Id == answer.Id)
                {
                    throw new ArgumentException("answer already exist");
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Answers(Id, DateOfCreating, Data) VALUES(@Id, @DateOfCreating, @Data)", connection);
                command.Parameters.AddWithValue("@Id", answer.Id);
                command.Parameters.AddWithValue("@DateOfCreating", answer.DateOfCreating);
                command.Parameters.AddWithValue("@Data", answer.Data);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddAnswerToQuestion(Guid answerId, Guid questionId)
        {
            if (answerId == null || questionId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO QuestionsAnswers(questionId,answerId) VALUES(@questionId, @answerId)", connection);
                command.Parameters.AddWithValue("@questionId", questionId);
                command.Parameters.AddWithValue("@answerId", answerId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool AddAnswerToUser(Guid answerId, Guid userId)
        {
            if (answerId == null || userId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO UsersAnswers(answerId,userId) VALUES(@answerId, @userId)", connection);
                command.Parameters.AddWithValue("@answerId", answerId);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public IEnumerable<AnswerDTO> GetAnswersByQuestionId(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> GetUsersIdsByAnswerId(Guid answerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT userId,answerId FROM UsersAnswers WHERE answerId=@answerId", connection);
                command.Parameters.AddWithValue("@answerId", answerId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return (Guid)reader["userId"];
                }
            }
        }

        public bool RemoveAnswer(Guid answerId)
        {
            if (answerId == null)
            {
                throw new ArgumentNullException("answer id is null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Answers WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", answerId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveAnswerFromQuestion(Guid answerId, Guid questionId)
        {
            if (answerId == null || questionId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM QuestionsAnswers WHERE answerId=@answerId AND questionId=@questionId", connection);
                command.Parameters.AddWithValue("@answerId", answerId);
                command.Parameters.AddWithValue("@questionId", questionId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        public bool RemoveAnswerFromUser(Guid answerId, Guid userId)
        {
            if (answerId == null || userId == null)
            {
                throw new ArgumentNullException("one of the relation ids are null");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM UsersAnswers WHERE answerId=@answerId AND userId=@userId", connection);
                command.Parameters.AddWithValue("@answerId", answerId);
                command.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                int countRow = command.ExecuteNonQuery();
                return countRow == 1;
            }
        }

        private IEnumerable<AnswerDTO> GetAllAnswers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT Id,DateOfCreating,Data FROM Answers", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new AnswerDTO()
                    {
                        Id = (Guid)reader["Id"],
                        DateOfCreating = (DateTime)reader["DateOfCreating"],
                        Data = (string)reader["Data"]
                    };
                }
            }
        }
    }
}
