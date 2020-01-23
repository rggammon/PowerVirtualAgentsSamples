// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using SampleBot.Models;

namespace Microsoft.PowerVirtualAgents.Samples.RelayBotSample.Controllers
{
    [Route("api/configuration")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        private IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAsync()
        {
            return Json(new Configuration()
            {
                SignInName = this.User.Identity.IsAuthenticated ?
                    this.User.GetDisplayName() : null
            });
        }
    }
}