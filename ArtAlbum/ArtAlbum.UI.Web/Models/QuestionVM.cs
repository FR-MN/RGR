using ArtAlbum.BLL.Abstract;
using ArtAlbum.DI.Providers;
using ArtAlbum.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtAlbum.UI.Web.Models
{
    public class QuestionVM
    {
        private static IQuestionsBLL questionsLogic = Provider.QuestionsBLL;

        private string caption;
        private DateTime dateOfCreating;

        public Guid Id { get; set; }
        public string Caption
        {
            get { return caption; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length >= 1000)
                {
                    throw new ArgumentException("incorrect caption");
                }
                caption = value;
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

        public static bool Add(QuestionVM question, Guid authorId)
        {
            return questionsLogic.AddQuestion(new QuestionDTO { Id = Guid.NewGuid(), Caption = question.Caption, DateOfCreating = DateTime.Now }, authorId);
        }

        public static IEnumerable<QuestionVM> GetAllQuestions()
        {
            List<QuestionVM> list = new List<QuestionVM>();
            foreach (var question in questionsLogic.GetAllQuestions())
            {
                list.Add((QuestionVM)question);
            }
            return list;
        }

        public static explicit operator QuestionVM(QuestionDTO data)
        {
            return new QuestionVM { Id = data.Id, Caption = data.Caption, DateOfCreating = data.DateOfCreating };
        }

        public static UserVM GetAuthorByQuestionId(Guid questionId)
        {
            return (UserVM)questionsLogic.GetUserByQuestionId(questionId);
        }

        public static QuestionVM GetQuestionById(Guid questionId)
        {
            return (QuestionVM)questionsLogic.GetQuestionById(questionId);
        }
    }
}