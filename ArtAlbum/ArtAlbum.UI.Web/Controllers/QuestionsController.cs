using ArtAlbum.Entities;
using ArtAlbum.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class QuestionsController : Controller
    {
        // GET: Questions
        public ActionResult QuestionsPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestion(QuestionVM question)
        {
            if (!string.IsNullOrWhiteSpace(question.Caption))
            {
                QuestionVM.Add(question, UserVM.GetUserIdByNickname(User.Identity.Name));
            }
            return View("QuestionsPage");
        }

        public ActionResult AnswersPage()
        {
            return View();
        }

    }
}