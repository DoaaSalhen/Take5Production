using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Services.Models.APIModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services
{
    public interface IQuestionService
    {
        List<QuestionModel> GetAllQuestions();
        Task<bool> CreateQuestion(QuestionModel model);
        Task<bool> UpdateQuestion(QuestionModel model);
        bool DeleteQuestion(int id);
        QuestionModel GetQuestion(int id);
        QuestionModel GetQuestionByName(string deptName);
        Task<List<QuestionModel>> GetQuestionsByStep(int step);
    }
}
