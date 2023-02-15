using AutoMapper;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question, long> _repository;
        private readonly ILogger<QuestionService> _logger;
        private readonly IMapper _mapper;

        public QuestionService(IRepository<Question, long> questionRepository,
          ILogger<QuestionService> logger, IMapper mapper)
        {
            _repository = questionRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<bool> CreateQuestion(QuestionModel model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.IsDelted = false;
                model.IsVisible = true;
                var question = _mapper.Map<Question>(model);
                var result = _repository.Add(question);

                if (result != null)
                {
                    return Task<bool>.FromResult<bool>(true);
                }
                else
                {
                    return Task<bool>.FromResult<bool>(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return Task<bool>.FromResult<bool>(false);
        }

        public bool DeleteQuestion(int id)
        {
            try
            {
                QuestionModel questionModel = GetQuestion(id);
                questionModel.IsVisible = false;
                questionModel.IsDelted = true;
                questionModel.UpdatedDate = DateTime.Now;
                Question question = _mapper.Map<Question>(questionModel);
                bool result = _repository.Update(question);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return false;

            }
        }
        public List<QuestionModel> GetAllQuestions()
        {
            try
            {
                var questions = _repository.Findlist().Result;
                var models = new List<QuestionModel>();
                models = _mapper.Map<List<QuestionModel>>(questions);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }


        public List<QuestionModel> GetAllActiveQuestions()
        {
            try
            {
                var questions = _repository.Find(q=>q.IsVisible == true).ToList();
                var models = new List<QuestionModel>();
                models = _mapper.Map<List<QuestionModel>>(questions);
                return models;
            }
            catch (Exception e)

            {
                _logger.LogError(e.ToString());
            }
            return null;
        }


        public QuestionModel GetQuestion(int id)
        {
            try
            {
                Question question = _repository.Find(d => d.IsVisible == true && d.Id == id).First();
                QuestionModel questionModel = _mapper.Map<QuestionModel>(question);
                return questionModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public QuestionModel GetQuestionByName(string text)
        {
            try
            {
                Question question = _repository.Find(d => d.IsVisible == true && d.Text == text).First();
                QuestionModel questionModel = _mapper.Map<QuestionModel>(question);
                return questionModel;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }
        public async Task<List<QuestionModel>> GetQuestionsByStep(int step)
        {
            try
            {
                var Questions = _repository.Find(q => q.Step == step && q.IsVisible == true);
                List<QuestionModel> questionModels = _mapper.Map<List<QuestionModel>>(Questions);
                return questionModels;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        public Task<bool> UpdateQuestion(QuestionModel model)
        {
            try
            {
                QuestionModel questionModel = GetQuestion(model.Id);
                questionModel.Step = model.Step;
                questionModel.Text = model.Text;
                questionModel.IsVisible = model.IsVisible;
                questionModel.UpdatedDate = DateTime.Now;
                Question question  = _mapper.Map<Question>(questionModel);
               bool result =  _repository.Update(question);
               return Task<bool>.FromResult<bool>(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return Task<bool>.FromResult<bool>(false);

            }
        }

    }
}
