using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Services;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        // GET: QuestionController
        public ActionResult Index()
        {
           List<QuestionModel> questionModels = _questionService.GetAllQuestions();
            return View(questionModels);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionModel model)
        {
            try
            {

               List<QuestionModel> questionModels = _questionService.GetAllQuestions();
                if(questionModels.Count != 0)
                {
                    model.Id = questionModels.Max<QuestionModel>().Id + 1;
                }
                else
                {
                    model.Id = 1;
                }
               bool result = _questionService.CreateQuestion(model).Result;
                if(result == true)
                {
                    TempData["Message"] = "Your question are added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Failed process, your question can not be added";
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: QuestionController/Edit/5
        //public ActionResult Edit(int id, int step)
        //{
        //    QuestionModel model = _questionService.GetQuestion(id);
        //    return View(model);
        //}

        // POST: QuestionController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public bool Edit(QuestionModel model)
        {
            try
            {
                var result = _questionService.UpdateQuestion(model).Result;
                if(result == true)
                {
                    TempData["Message"] = "Your question are updated successfully";
                    return true;
                }
                else
                {
                    TempData["Error"] = "Failed process, your question can not be added";
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        // GET: QuestionController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: QuestionController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public bool Delete(int id)
        {
            try
            {
                var result = _questionService.DeleteQuestion(id);
                return result;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult GetQuestionsUsingStep(int step)
        {
            try
            {
                List<QuestionModel> questionModels = _questionService.GetQuestionsByStep(step).Result;

                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }
    }
}
