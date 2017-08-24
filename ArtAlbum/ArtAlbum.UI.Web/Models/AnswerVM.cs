using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class AnswerVM
    {
        private static IAnswersBLL answersLogic = Provider.AnswersBLL;

        private string data;
        private DateTime dateOfCreating;

        public Guid Id { get; set; }
        public string Data
        {
            get { return data; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length >= 1000)
                {
                    throw new ArgumentException("incorrect text");
                }
                data = value;
            }
        }
        public DateTime DateOfCreating
        {
            get { return dateOfCreating; }
            set
            {
                if (value > DateTime.Now || value == null)
                {
                    throw new ArgumentException("incorrect date of creating");
                }
                dateOfCreating = value;
            }
        }

        public static bool Add(AnswerVM answer, Guid authorId, Guid questionId)
        {
            return answersLogic.AddAnswer(new AnswerDTO { Id = Guid.NewGuid(), Data = answer.Data, DateOfCreating = DateTime.Now }, authorId, questionId);
        }

        public static IEnumerable<AnswerVM> GetAnswersByQuestionId(Guid questionId)
        {
            List<AnswerVM> list = new List<AnswerVM>();
            foreach (var answer in answersLogic.GetAnswersByQuestionId(questionId))
            {
                list.Add((AnswerVM)answer);
            }
            return list;
        }

        public static explicit operator AnswerVM(AnswerDTO data)
        {
            return new AnswerVM { Id = data.Id, Data = data.Data, DateOfCreating = data.DateOfCreating };
        }

        public static UserVM GetAuthorByAnswerId(Guid answerId)
        {
            return (UserVM)answersLogic.GetUsersByAnswerId(answerId);
        }
    }
}