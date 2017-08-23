using ArtAlbum.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAlbum.Entities;
using ArtAlbum.DAL.Abstract;
using ArtAlbum.BLL.DefaultLogic.Exceptions;

namespace ArtAlbum.BLL.DefaultLogic
{
    public class AnswersBLL : IAnswersBLL
    {
        private IUsersDAL usersDAL;
        private IAnswersDAL answersDAL;
        private IQuestionsDAL questionsDAL;

        public AnswersBLL(IUsersDAL usersDAL, IAnswersDAL answersDAL, IQuestionsDAL questionsDAL)
        {
            if (usersDAL == null || answersDAL == null || questionsDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.usersDAL = usersDAL;
            this.answersDAL = answersDAL;
            this.questionsDAL = questionsDAL;
        }

        private bool IsAnswerCorrect(AnswerDTO answer)
        {
            return !string.IsNullOrWhiteSpace(answer.Data) && answer.Data.Length < 500 && answer.DateOfCreating < DateTime.Now && answer.Id != null;
        }

        public bool AddAnswer(AnswerDTO answer, Guid userId, Guid questionId)
        {
            if (answer == null || userId == null || questionId == null)
            {
                throw new ArgumentNullException("answer data is null");
            }
            else if (!IsAnswerCorrect(answer))
            {
                throw new IncorrectDataException();
            }
            try
            {
                usersDAL.GetUserById(userId);
                questionsDAL.GetQuestionById(questionId);
            }
            catch
            {
                throw new ArgumentNullException("user or question doesn't exist");
            }
            return answersDAL.AddAnswer(answer) && answersDAL.AddAnswerToUser(answer.Id, userId) && answersDAL.AddAnswerToQuestion(answer.Id, questionId);
        }

        public IEnumerable<AnswerDTO> GetAnswersByQuestionId(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            foreach (var answerId in answersDAL.GetAnswersIdsByQuestionId(questionId))
            {
                yield return answersDAL.GetAnswerById(answerId);
            }
        }

        public UserDTO GetUsersByAnswerId(Guid answerId)
        {
            if (answerId == null)
            {
                throw new ArgumentNullException("answer id is null");
            }
            try
            {
                answersDAL.GetAnswerById(answerId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("answer id is incorrect, answer doesn't exist", e);
            }
            var users = usersDAL.GetAllUsers().Join(answersDAL.GetUsersIdsByAnswerId(answerId),
                user => user.Id, userId => userId, (user, userId) => new UserDTO
                { Id = userId, FirstName = user.FirstName, DateOfBirth = user.DateOfBirth, Email = user.Email, HashOfPassword = user.HashOfPassword, LastName = user.LastName, Nickname = user.Nickname });
            if (users != null)
            {
                return users.First();
            }
            throw new Exception("answer doesn't have author");
        }

        public bool RemoveAnswer(Guid answerId)
        {
            if (answerId == null)
            {
                throw new ArgumentNullException("answer id is null");
            }
            try
            {
                answersDAL.GetAnswerById(answerId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("answer id is incorrect, answer doesn't exist", e);
            }
            return answersDAL.RemoveAnswerFromUser(answerId, answersDAL.GetUsersIdsByAnswerId(answerId).First()) && answersDAL.RemoveAnswerFromQuestion(answerId, answersDAL.GetQuestionsIdsByAnswerId(answerId).First()) && answersDAL.RemoveAnswer(answerId);
        }
    }
}
