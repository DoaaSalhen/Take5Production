﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Take5.Services.Implementation
{
    public class RestrictDomainAttribute : Attribute, IAuthorizationFilter
    {
        public IEnumerable<string> AllowedHosts { get; }

        public RestrictDomainAttribute(params string[] allowedHosts) => AllowedHosts = allowedHosts;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //  Get host from the request and check if it's in the enumeration of allowed hosts
            string host = context.HttpContext.Request.Host.Host;
            if (!AllowedHosts.Contains(host, StringComparer.OrdinalIgnoreCase))
            {
                //  Request came from an authorized host, return bad request
                context.Result = new BadRequestObjectResult("Host is not allowed");
            }
        }
    }
}
