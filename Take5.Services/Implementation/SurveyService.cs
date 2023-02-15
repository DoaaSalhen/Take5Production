using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;
using Take5.Services.Models.SpecificModels;

namespace Take5.Services.Implementation
{
    public class SurveyService : ISurveyService
    {
        private readonly IQuestionService _questionService;
        private readonly IDangerService _dangerService;
        private readonly IDangerCategoryService _dangerCategoryService;
        private readonly IMeasureControlService _measureControlService;
        private readonly IRepository<TripQuestion, long> _tripQuestionRepository;
        private readonly IRepository<TripDanger, long> _tripDangersRepository;
        private readonly ILogger<SurveyService> _logger;
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;

        public SurveyService(IQuestionService questionService,
            IDangerService dangerService,
            IDangerCategoryService dangerCategoryService,
            IMeasureControlService measureControlService,
            IRepository<TripQuestion, long> tripQuestionRepository,
            IRepository<TripDanger, long> tripDangersRepository,
            ILogger<SurveyService> logger,
            IDriverService driverService,
            IMapper mapper
            )
        {
            _questionService = questionService;
            _dangerService = dangerService;
            _dangerCategoryService = dangerCategoryService;
            _measureControlService = measureControlService;
            _tripQuestionRepository = tripQuestionRepository;
            _tripDangersRepository = tripDangersRepository;
            _driverService = driverService;
            _logger = logger;
            _mapper = mapper;
        }


        public SurveyStepOneModel LoadSurveyStepOne()
        {
            try
            {
                SurveyStepOneModel surveyStepOneModel = new SurveyStepOneModel();

                surveyStepOneModel.QuestionModels = _questionService.GetQuestionsByStep((int)CommanData.SurveySteps.StepOne).Result;

                var DangerModels = _dangerService.GetAllDangers();

                if (DangerModels.Count != 0)
                {
                    var DangerCategoryModels = _dangerCategoryService.GetAllDangerCategories();
                    foreach (var danger in DangerCategoryModels)
                    {
                        danger.DangerModels = DangerModels.Where(d => d.DangerCategoryId == danger.Id).ToList();
                    }
                    surveyStepOneModel.DangerCategoryModels = DangerCategoryModels;
                }
                surveyStepOneModel.MeasureControlModels = _measureControlService.GetAllMeasureControls();
                return surveyStepOneModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SurveyStepTwoModel LoadSurveyStepTwo()
        {
            try
            {
                SurveyStepTwoModel surveyStepTwoModel = new SurveyStepTwoModel();

                surveyStepTwoModel.QuestionModels = _questionService.GetQuestionsByStep((int)CommanData.SurveySteps.StepTwo).Result;
                return surveyStepTwoModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<bool> AddDangersToSurveyStepOne(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId)
        {
            try
            {
                foreach (var danger in model.DangerAPIs)
                {
                    foreach (var control in danger.MeasureControlAPIs)
                    {
                        TripDanger tripDanger = new TripDanger();
                        tripDanger.DangerId = danger.DangerId;
                        tripDanger.JobSiteId = JobSiteId;
                        tripDanger.TripId = TripId;
                        tripDanger.MeasureControlId = control.MeasureControlId;
                        var addedTripDanger = _tripDangersRepository.Add(tripDanger);
                        if (addedTripDanger == null)
                        {
                            return Task<bool>.FromResult(false);
                        }
                    }
                }
                return Task<bool>.FromResult(true);
            }
            catch (Exception e)
            {
                return Task<bool>.FromResult(false);
            }

        }

        public TripSurveyModel LoadSurveyQuestionAnswersForTrip(long tripNumber, long jobsiteId)
        {
            try
            {
                TripSurveyModel tripSurveyModel = new TripSurveyModel();
                List<TripQuestion> tripQuestions = _tripQuestionRepository.Find(tq => tq.IsVisible == true && tq.TripId == tripNumber && tq.JobSiteId == jobsiteId, false, tq => tq.Question).ToList();
                if (tripQuestions.Count > 0)
                {
                    List<TripQuestion> tripQuestionsStepOne = tripQuestions.Where(tq => tq.Question.Step == (int)CommanData.SurveySteps.StepOne).ToList();
                    if (tripQuestionsStepOne.Count > 0)
                    {
                        tripSurveyModel.SurveyStepOneAnswersModels = MapFromTripQuestionToSurveyQuestionAnswersModel(tripQuestionsStepOne);
                    }
                    List<TripQuestion> tripQuestionsStepTwo = tripQuestions.Where(tq => tq.Question.Step == (int)CommanData.SurveySteps.StepTwo).ToList();
                    if (tripQuestionsStepTwo.Count > 0)
                    {
                        tripSurveyModel.SurveyStepTwoAnswersModels = MapFromTripQuestionToSurveyQuestionAnswersModel(tripQuestionsStepTwo);
                    }
                    return tripSurveyModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<SurveyStepOneDangersModel> LoadSurveyDangersForTrip(long tripNumber, long jobsiteId)
        {
            try
            {
                List<TripDangerModel> tripDangerModels = new List<TripDangerModel>();
                List<TripDanger> tripDangers = _tripDangersRepository.Find(td => td.IsVisible == true && td.TripId == tripNumber && td.JobSiteId == jobsiteId, false, td => td.MeasureControl).ToList();
                if (tripDangers.Count > 0)
                {
                    List<SurveyStepOneDangersModel> surveyStepOneDangersModels = new List<SurveyStepOneDangersModel>();
                    tripDangerModels = _mapper.Map<List<TripDangerModel>>(tripDangers);
                    if(tripDangerModels.Count > 0)
                    {
                        var measureControlGroups = tripDangerModels.GroupBy(td => td.DangerId);
                        foreach (var key in measureControlGroups)
                        {
                            SurveyStepOneDangersModel surveyStepOneDangersModel = new SurveyStepOneDangersModel();
                           DangerModel dangerModel = _dangerService.GetDanger(key.Key);
                            surveyStepOneDangersModel.DangerModel = dangerModel;
                            List<MeasureControl> measureControls = tripDangerModels.Where(td => td.DangerId == key.Key).Select(td => td.MeasureControl).ToList();
                            List<MeasureControlModel> measureControlModels = _mapper.Map<List<MeasureControlModel>>(measureControls);
                            surveyStepOneDangersModel.measureControlModels = measureControlModels;
                            surveyStepOneDangersModels.Add(surveyStepOneDangersModel);
                        }
                    }
                    return surveyStepOneDangersModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<SurveyQuestionAnswersModel> MapFromTripQuestionToSurveyQuestionAnswersModel(List<TripQuestion> tripQuestions)
        {
            try
            {
                List<SurveyQuestionAnswersModel> surveyQuestionAnswersModels = new List<SurveyQuestionAnswersModel>();
                foreach (var tripQuestion in tripQuestions)
                {
                    SurveyQuestionAnswersModel surveyQuestionAnswersModel = new SurveyQuestionAnswersModel();
                    surveyQuestionAnswersModel.QuestionId = tripQuestion.Question.Id;
                    surveyQuestionAnswersModel.QuestionText = tripQuestion.Question.Text;
                    surveyQuestionAnswersModel.Answer = tripQuestion.Answer;

                    surveyQuestionAnswersModels.Add(surveyQuestionAnswersModel);
                }
                return surveyQuestionAnswersModels;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DeleteTripQuestion(long tripNumber, long jobsiteId, int step)
        {
            bool updateResult = false;
            try
            {
                List<TripQuestion> tripQuestions = _tripQuestionRepository.Find(tq => tq.IsVisible == true && tq.TripId == tripNumber && tq.JobSiteId == jobsiteId, false, tq => tq.Question).ToList();
                if (tripQuestions.Count > 0)
                {
                    foreach (var question in tripQuestions)
                    {
                        QuestionModel questionModel = _questionService.GetQuestion(question.QuestionId);
                        if (questionModel.Step == step)
                        {
                            question.IsVisible = false;
                            question.IsDelted = true;
                            question.UpdatedDate = DateTime.Now;
                            updateResult = _tripQuestionRepository.Update(question);
                        }
                    }
                    return updateResult;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                return updateResult;
            }

        }


        public bool DeleteTripDangers(long tripNumber, long jobsiteId)
        {
            bool updateResult = false;
            try
            {
                var TripDangers = _tripDangersRepository.Find(td => td.TripId == tripNumber && td.JobSiteId == jobsiteId && td.IsVisible == true).ToList();

                    foreach (var danger in TripDangers)
                    {
                        danger.IsVisible = false;
                        danger.IsDelted = true;
                        danger.UpdatedDate = DateTime.Now;
                        updateResult = _tripDangersRepository.Update(danger);
                    }
                    return updateResult;

            }
            catch (Exception e)
            {
                return updateResult;
            }

        }


        public ALLSurveyModel getAllSurvey()
        {
            try
            {
                ALLSurveyModel aLLSurveyModel = new ALLSurveyModel();
                SurveyStepOneModel surveyStepOneModel = LoadSurveyStepOne();
                SurveyStepTwoModel surveyStepTwoModel = LoadSurveyStepTwo();
                if (surveyStepOneModel != null)
                {
                    List<QuestionAnswerModel> StePOneQuestionModels = new List<QuestionAnswerModel>();
                    if (surveyStepOneModel.QuestionModels.Count > 0)
                    {
                        foreach (var question in surveyStepOneModel.QuestionModels)
                        {
                            QuestionAnswerModel questionModel = new QuestionAnswerModel();

                            questionModel.Id = question.Id;
                            questionModel.Question = question.Text;
                            questionModel.Step = question.Step;
                            StePOneQuestionModels.Add(questionModel);
                        }
                        aLLSurveyModel.StepOneQuestions = StePOneQuestionModels;
                    }
                    if (surveyStepOneModel.DangerCategoryModels.Count > 0)
                    {
                        List<DangerControlsWithCategoryAPIModel> dangerWithCategoryAPIModels = new List<DangerControlsWithCategoryAPIModel>();
                        foreach (var category in surveyStepOneModel.DangerCategoryModels)
                        {
                            if (category.DangerModels.Count > 0)
                            {
                                DangerControlsWithCategoryAPIModel dangerWithCategoryAPIModel = new DangerControlsWithCategoryAPIModel();
                                dangerWithCategoryAPIModel.DangerCategory = category.Name;
                                dangerWithCategoryAPIModel.DangerCategoryId = category.Id;
                                List<DangerAPI> dangerModels = new List<DangerAPI>();
                                foreach (var danger in category.DangerModels)
                                {
                                    DangerAPI dangerModel = new DangerAPI();
                                    dangerModel.DangerId = danger.Id;
                                    dangerModel.DangerName = danger.Name;
                                    List<MeasureControlModel> measureControlModels = _measureControlService.GetMeasureControlsByDangerId(danger.Id);
                                    if (measureControlModels != null)
                                    {
                                        List<MeasureControlAPI> measureControlAPIs = _measureControlService.MapMeasureControlModelToMeasureControlAPI(measureControlModels);
                                        if (measureControlAPIs != null)
                                        {
                                            dangerModel.MeasureControlAPIs = measureControlAPIs;
                                        }
                                    }
                                    dangerModels.Add(dangerModel);
                                }
                                dangerWithCategoryAPIModel.DangerModels = dangerModels;
                                dangerWithCategoryAPIModels.Add(dangerWithCategoryAPIModel);
                            }
                            aLLSurveyModel.DangerWithCategoryAPIModels = dangerWithCategoryAPIModels;
                        }

                    }

                    //if(surveyStepOneModel.MeasureControlModels.Count >0)
                    //{
                    //   var MeasureControlModelsGroups = surveyStepOneModel.MeasureControlModels.GroupBy(m => m.DangerId).ToList();
                    //    List<MeasureControlWithDangerModel> measureControlWithDangerModels = new List<MeasureControlWithDangerModel>();
                    //    foreach(var group in MeasureControlModelsGroups)
                    //    {
                    //        MeasureControlWithDangerModel measureControlWithDangerModel = new MeasureControlWithDangerModel();
                    //        measureControlWithDangerModel.DangerId = group.Key;
                    //        List<SurveyStaticDataModel> measureControlModels = new List<SurveyStaticDataModel>();
                    //        foreach (var measureControl in group)
                    //        {
                    //            SurveyStaticDataModel measureControlModel = new SurveyStaticDataModel();
                    //            measureControlModel.Id = measureControl.Id;
                    //            measureControlModel.Text = measureControl.Name;
                    //            measureControlWithDangerModel.DangerName = measureControl.Danger.Name;
                    //            measureControlModels.Add(measureControlModel);
                    //        }
                    //        measureControlWithDangerModel.DangerControlModels = measureControlModels;
                    //        measureControlWithDangerModels.Add(measureControlWithDangerModel);
                    //    }
                    //    aLLSurveyModel.MeasureControlWithDangerModels = measureControlWithDangerModels;
                    //}
                }
                if (surveyStepTwoModel != null)
                {
                    List<QuestionAnswerModel> StepTwoQuestionModels = new List<QuestionAnswerModel>();
                    if (surveyStepTwoModel.QuestionModels.Count > 0)
                    {
                        foreach (var question in surveyStepTwoModel.QuestionModels)
                        {
                            QuestionAnswerModel questionModel = new QuestionAnswerModel();

                            questionModel.Id = question.Id;
                            questionModel.Question = question.Text;
                            StepTwoQuestionModels.Add(questionModel);
                        }
                        aLLSurveyModel.StepTwoQuestions = StepTwoQuestionModels;
                    }
                }
                return aLLSurveyModel;

            }
            catch (Exception e)
            {
                return null;
            }
        }


        public bool DeleteTake5StepOneForTripJobsite(long tripNumber, long JobsiteId)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = DeleteTripQuestion(tripNumber, JobsiteId, (int)CommanData.SurveySteps.StepOne);
                if (deleteResult == true)
                {
                    deleteResult = DeleteTripDangers(tripNumber, JobsiteId);
                    return deleteResult;
                }
                return deleteResult;

            }
            catch (Exception e)
            {
                return deleteResult;
            }
        }

        public bool DeleteTake5StepTwoForTripJobsite(long tripNumber, long JobsiteId)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = DeleteTripQuestion(tripNumber, JobsiteId, (int)CommanData.SurveySteps.StepTwo);
                return deleteResult;

            }
            catch (Exception e)
            {
                return deleteResult;
            }
        }


        public async Task<bool> AddAnswersToStepNQuestions(SurveyStepOneAnswersAPIModel model, long TripId, long JobSiteId, int step)
        {
            try
            {
                var questions = _questionService.GetQuestionsByStep(step).Result;
                if(questions.Count == model.QuestionAnswerModels.Count)
                {
                    foreach (var question in model.QuestionAnswerModels)
                    {
                        var currentQuestion = _questionService.GetQuestion(question.Id);
                        if (currentQuestion.Step == step)
                        {
                            TripQuestion tripQuestion = new TripQuestion();
                            tripQuestion.TripId = TripId;
                            tripQuestion.JobSiteId = JobSiteId;
                            tripQuestion.QuestionId = question.Id;
                            tripQuestion.Answer = (bool)question.Answer;
                            tripQuestion.CreatedDate = model.CreatedDate;
                            tripQuestion.UpdatedDate = model.CreatedDate;
                            tripQuestion.IsVisible = true;
                            tripQuestion.IsDelted = false;
                            var addedTripQuestion = _tripQuestionRepository.Add(tripQuestion);
                        }
                        //else
                        //{
                        //    return false;
                        //}

                    }

                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;

            }
        }
    }
       
}
