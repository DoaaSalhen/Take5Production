using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.SpecificModels
{
    public class TripTimeLineModel
    {
        public DateTime AssignedDate { get; set; }

        public DateTime StartingDate { get; set; }
        public DateTime ArrivedDate { get; set; }

        public DateTime StepOneCompletionDate { get; set; }

        public DateTime StepTwoRequestDate { get; set; }

        public DateTime StepTwoApprovalDate { get; set; }

        public DateTime StepTwoCompletionDate { get; set; }



    }
}
