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
    public class QuestionsBLL : IQuestionsBLL
    {
        private IQuestionsDAL questionsDAL;
        private IUsersDAL usersDAL;
        private IAnswersDAL answersDAL;
        private IQImagesDAL qimagesDAL;

        public QuestionsBLL(IQuestionsDAL questionsDAL, IUsersDAL usersDAL, IAnswersDAL answersDAL, IQImagesDAL qimagesDAL)
        {
            if (questionsDAL == null || usersDAL == null || answersDAL == null || qimagesDAL == null)
            {
                throw new ArgumentNullException("one of the dals is null");
            }
            this.questionsDAL = questionsDAL;
            this.usersDAL = usersDAL;
            this.answersDAL = answersDAL;
            this.qimagesDAL = qimagesDAL;
        }

        private bool IsQuestionCorrect(QuestionDTO question)
        {
            return !string.IsNullOrWhiteSpace(question.Caption) && question.Caption.Length < 1000 && question.DateOfCreating < DateTime.Now && question.Id != null;
        }

        public bool AddQuestion(QuestionDTO question, Guid authorId)
        {
            if (question == null || authorId == null)
            {
                throw new ArgumentNullException("question data is null");
            }
            else if (!IsQuestionCorrect(question))
            {
                throw new IncorrectDataException();
            }
            try
            {
                usersDAL.GetUserById(authorId);
            }
            catch
            {
                throw new ArgumentNullException("user doesn't exist");
            }
            if (questionsDAL.GetQuestionById(question.Id) != null)
            {
                return false;
            }
            return questionsDAL.AddQuestion(question) && questionsDAL.AddQuestionToUser(question.Id, authorId);
        }

        public IEnumerable<QuestionDTO> GetAllQuestions()
        {
            return questionsDAL.GetAllQuestions();
        }

        public QuestionDTO GetQuestionById(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            return questionsDAL.GetQuestionById(questionId);
        }

        public UserDTO GetUserByQuestionId(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            try
            {
                questionsDAL.GetQuestionById(questionId);
            }
            catch (Exception e)
            {
                throw new ArgumentException("question id is incorrect, question doesn't exist", e);
            }
            var users = usersDAL.GetAllUsers().Join(questionsDAL.GetUsersIdsByQuestionId(questionId),
                user => user.Id, userId => userId, (user, userId) => new UserDTO
                { Id = userId, FirstName = user.FirstName, DateOfBirth = user.DateOfBirth, Email = user.Email, HashOfPassword = user.HashOfPassword, LastName = user.LastName, Nickname = user.Nickname });
            if (users != null)
            {
                return users.First();
            }
            throw new Exception("question doesn't have author");
        }

        public bool RemoveQuestionById(Guid questionId)
        {
            if (questionId == null)
            {
                throw new ArgumentNullException("question id is null");
            }
            foreach (var answerId in answersDAL.GetAnswersIdsByQuestionId(questionId))
            {
                answersDAL.RemoveAnswerFromUser(answerId, answersDAL.GetUsersIdsByAnswerId(answerId).First());
                answersDAL.RemoveAnswerFromQuestion(answerId, questionId);
                answersDAL.RemoveAnswer(answerId);
            }
            foreach (var qimageId in qimagesDAL.GetImagesIdsByQuestionId(questionId))
            {
                qimagesDAL.RemoveImageFromQuestion(qimageId, questionId);
                qimagesDAL.RemoveImage(qimageId);
            }
            return questionsDAL.RemoveQuestionFromUser(questionId, questionsDAL.GetUsersIdsByQuestionId(questionId).First()) && questionsDAL.RemoveQuestionById(questionId);
        }
    }
}
