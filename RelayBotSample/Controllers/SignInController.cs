// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Collections.Generic;

namespace SampleBot.Controllers
{
    [Authorize]
    [Route("signin")]
    public class SignInController : Controller
    {
        public IActionResult PostAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                var properties = new AuthenticationProperties();

                //var scopes = new List<string>() { 
                //    "openid", 
                //    "profile",
                //};

                //properties.SetParameter<ICollection<string>>(OpenIdConnectParameterNames.Scope, scopes);

                return Challenge(properties);
            }
        }
    }
}
