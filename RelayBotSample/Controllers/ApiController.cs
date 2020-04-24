// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.BotService;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Rest;
using SampleBot.Extensions;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.PowerVirtualAgents.Samples.RelayBotSample.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : Controller
    {
        private IConfiguration _configuration;
        private ITokenAcquisition _tokenAcquisition;

        public ApiController(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        [Route("me")]
        public IActionResult GetConfigurationAsync()
        {
            return Json(new
            {
                SignInName = this.User.Identity.IsAuthenticated ?
                    this.User.GetDisplayName() : null
            });
        }

        [HttpGet]
        [Authorize]
        [Route("channelapps")]
        public async Task<IActionResult> GetChannelAppsAsync()
        {
            var token = await _tokenAcquisition.GetGraphTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(requestMessage =>
            {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                return Task.CompletedTask;
            }));

            var page = await graphClient
                .Applications
                .Request()
                .Filter("tags/any(t:t%20eq%20'PVAChannelApp')")
                .GetAsync();

            var value = page
                .Select(a => new
                {
                    AppId = a.AppId,
                    DisplayName = a.DisplayName,
                    ResourceGroup = a.Tags.Where(t => t.StartsWith("/subscription")).FirstOrDefault()
                })
                .ToArray();

            return Json(new
            { 
                Value = value
            });
        }

        [Authorize]
        [Route("channelSolution/subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}")]
        public async Task<IActionResult> GetChannelSolutionAsync(string subscriptionId, string resourceGroup)
        {
            var armToken = await _tokenAcquisition.GetArmTokenAsync();
            if (string.IsNullOrEmpty(armToken))
            {
                return Unauthorized();
            }

            Microsoft.Azure.Management.BotService.Models.Bot bot;
            using (var client = new AzureBotServiceClient(new TokenCredentials(armToken)))
            {
                client.SubscriptionId = subscriptionId;
                var page = await client.Bots
                    .ListByResourceGroupAsync(resourceGroup);
                bot = page.FirstOrDefault();
            }

            return Json(new 
            {
                BotName = bot.Properties.DisplayName
            });
        }
    }
}