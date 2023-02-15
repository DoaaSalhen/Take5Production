using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class SurveyController : BaseController
    {
        private readonly ISurveyService _surveyService;
        private readonly ITripJobsiteService _tripJobsiteService;
        private readonly ITripTake5TogetherService _tripTake5TogetherService;

        public SurveyController(ISurveyService surveyService,
            ITripJobsiteService tripJobsiteService,
             ITripTake5TogetherService tripTake5TogetherService)
        {
            _surveyService = surveyService;
            _tripJobsiteService = tripJobsiteService;
            _tripTake5TogetherService = tripTake5TogetherService;
        }
        // GET: SurveyController
        public ActionResult ShowSurvey()
        {
            try
            {
                SurveyModel surveyModel = new SurveyModel();
                surveyModel.SurveyStepOneModel = _surveyService.LoadSurveyStepOne();
                surveyModel.SurveyStepTwoModel = _surveyService.LoadSurveyStepTwo();
                return View(surveyModel);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        public ActionResult ShowSurveyForTrip(long tripNumber, long jobsiteId)
        {
            try
            {
                TripSurveyModel tripSurveyModel = new TripSurveyModel();
                var questions = _surveyService.LoadSurveyQuestionAnswersForTrip(tripNumber, jobsiteId);
                if (questions != null)
                {
                    tripSurveyModel.SurveyStepOneAnswersModels = questions.SurveyStepOneAnswersModels;
                    tripSurveyModel.SurveyStepTwoAnswersModels = questions.SurveyStepTwoAnswersModels;
                    var result = _surveyService.LoadSurveyDangersForTrip(tripNumber, jobsiteId);
                    tripSurveyModel.SurveyStepOneDangersModels = result;
                    tripSurveyModel.TripJobsiteModel = _tripJobsiteService.GetTripJobsiteModelByTripNumberAndJobsiteId(tripNumber, jobsiteId);
                    List<TripTake5TogetherModel> tripTake5TogetherModels = _tripTake5TogetherService.GetTripTake5TogetherForTrip(tripNumber, jobsiteId);
                    if (tripTake5TogetherModels.Count > 0)
                    {
                        tripSurveyModel.tripTake5TogetherModels = tripTake5TogetherModels;
                    }
                    return View(tripSurveyModel);
                }
                else
                {
                    return View();
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }

        }

        // GET: SurveyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SurveyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SurveyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: SurveyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SurveyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: SurveyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SurveyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }
    }
}
