using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    public class DangerControlsWithCategoryAPIModel
    {

        public string DangerCategory { get; set; }
        public int DangerCategoryId { get; set; }

        public List<DangerAPI> DangerModels { get; set; }

    }
}
