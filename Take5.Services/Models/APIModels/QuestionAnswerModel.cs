using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class QuestionAnswerModel
    {
        public int Id { get; set; }

        public string Question { get; set; }
        public int Step { get; set; }

        public bool? Answer { get; set; }
    }
}
