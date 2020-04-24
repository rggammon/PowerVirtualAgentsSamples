// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SampleBot.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleBot.Controllers
{
    [Authorize]
    [Route("signin")]
    public class SignInController : Controller
    {
        private ITokenAcquisition _tokenAcquisition;

        public SignInController(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        public IActionResult Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                var properties = new AuthenticationProperties();
                return Challenge(properties);
            }
        }

        [Route("arm")]
        public async Task<IActionResult> Arm()
        {
            bool challenge = true;

            if (User.Identity.IsAuthenticated)
            {
                var token = await _tokenAcquisition.GetArmTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    challenge = false;
                }
            }
            
            if (challenge)
            {
                var properties = new AuthenticationProperties();

                properties.SetParameter<ICollection<string>>(OpenIdConnectParameterNames.Scope, _tokenAcquisition.GetArmScopes());

                return Challenge(properties);
            }

            return Redirect("/");
        }

        [Route("graph")]
        public async Task<IActionResult> Graph()
        {
            bool challenge = true;

            if (User.Identity.IsAuthenticated)
            {
                var token = await _tokenAcquisition.GetGraphTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    challenge = false;
                }
            }

            if (challenge)
            {
                var properties = new AuthenticationProperties();

                properties.SetParameter<ICollection<string>>(OpenIdConnectParameterNames.Scope, _tokenAcquisition.GetGraphScopes());

                return Challenge(properties);
            }

            return Redirect("/");
        }

    }
}
