using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.MasterModels
{
    public class CancellationModel
    {
        public List<TripJobsiteModel> TripJobsiteModels { get; set; }

        public string ConvertedMessage { get; set; }

        public long  TripNumber { get; set; }

        public bool CanCancel { get; set; }

    }
}
