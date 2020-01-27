// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.BotService;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.Rest;
using SampleBot.Extensions;
using SampleBot.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.PowerVirtualAgents.Samples.RelayBotSample.Controllers
{
    [Route("api/configuration")]
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
        public async Task<IActionResult> GetAsync()
        {
            bool areAzureBotChannelsConfigured = false;

            if (User.Identity.IsAuthenticated)
            {
                var token = await _tokenAcquisition.GetArmTokenAsync();
                if (token != null)
                {
                    using (var client = new AzureBotServiceClient(new TokenCredentials(token)))
                    {
                        client.SubscriptionId = _configuration["SubscriptionId"];
                        var page = await client.Bots
                            .ListByResourceGroupAsync(_configuration["ResourceGroupName"]);

                        areAzureBotChannelsConfigured = page.Any();

                        if (!areAzureBotChannelsConfigured)
                        {
                            var bot = await client.Bots
                                .CreateAsync(
                                    _configuration["ResourceGroupName"],
                                    "rgammonrelaybot1-bot-xyz123",
                                    new Azure.Management.BotService.Models.Bot()
                                    {
                                        Properties = new Azure.Management.BotService.Models.BotProperties
                                        {
                                            // DeveloperAppInsightsApplicationId { get; set; }
                                            // DeveloperAppInsightsApiKey { get; set; }
                                            // DeveloperAppInsightKey { get; set; }
                                            MsaAppId = _configuration["MicrosoftAppId"],
                                            Endpoint = "https://rgammonrelaybot-webapp-smxylql.azurewebsites.net/api/messages",
                                            Description = "A bot",
                                            DisplayName = "A bot"
                                        }
                                    }
                                );

                            await client.Channels
                                .CreateAsync(
                                    _configuration["ResourceGroupName"],
                                    "rgammonrelaybot1-bot-xyz123",
                                    

                                    );
                        }
                    }
                }
            }

            return Json(new Configuration()
            {
                SignInName = this.User.Identity.IsAuthenticated ?
                    this.User.GetDisplayName() : null,
                AreAzureBotChannelsConfigured = areAzureBotChannelsConfigured 
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConfigureAzureBotServiceAsync()
        {
            var token = await _tokenAcquisition.GetArmTokenAsync();
            if (token != null)
            {
                return Unauthorized();
            }

            using (var client = new AzureBotServiceClient(new TokenCredentials(token)))
            {
                client.SubscriptionId = _configuration["SubscriptionId"];
            }
        }
    }
}