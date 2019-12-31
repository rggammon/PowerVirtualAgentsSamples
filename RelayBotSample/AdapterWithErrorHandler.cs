// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.Solutions.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleBot.Configurations;
using Microsoft.Extensions.Options;

namespace Microsoft.PowerVirtualAgents.Samples.RelayBotSample
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(
            IConfiguration configuration,
            IOptions<SetSpeakMiddlewareConfiguration> setSpeakMiddlewareOptions,
            ILogger<BotFrameworkHttpAdapter> logger)
            : base(configuration, logger)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError($"Exception caught : {exception.ToString()}");

                // Send a catch-all apology to the user.
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");
            };

            Use(new SetSpeakMiddleware(setSpeakMiddlewareOptions.Value.Locale, setSpeakMiddlewareOptions.Value.VoiceFont));
        }
    }
}
