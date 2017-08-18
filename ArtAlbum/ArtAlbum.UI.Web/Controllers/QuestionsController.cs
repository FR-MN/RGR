using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtAlbum.UI.Web.Controllers
{
    public class QuestionsController : Controller
    {
        //// GET: Questions
        //public ActionResult Index()
        //{
        //    return View();
        //}
        // GET: Questions
        public ActionResult QuestionsPage()
        {
            return View();
        }
        public ActionResult AskQuestion()
        {
            return View();
        }
        public ActionResult AnswersOnQuestions()
        {
            return View();
        }

    }
}