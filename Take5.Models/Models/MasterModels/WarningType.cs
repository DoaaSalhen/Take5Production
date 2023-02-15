using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models.Models.Entity;

namespace Take5.Models.Models.MasterModels
{
    public class WarningType: Entity<int>
    {
        public string Type { get; set; }
    }
}
