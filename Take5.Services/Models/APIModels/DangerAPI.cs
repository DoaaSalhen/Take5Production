using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
   public class DangerAPI
    {
        public int DangerId { get; set; }

        public string DangerName { get; set; }

        public List<MeasureControlAPI> MeasureControlAPIs { get; set; }
    }
}
