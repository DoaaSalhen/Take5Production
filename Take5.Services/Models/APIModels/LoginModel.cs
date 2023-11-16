using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models.APIModels
{
    [Bind(include: "DriverNumber, Password")]
    public class LoginModel
    {
       public long DriverNumber { get; set; }
       public string Password { get; set; }
       public bool RememberMe { get; set; }
    }
}
