// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace SampleBot.Models
{
    public class Configuration
    {
        public string SignInName { get; set; }

        public bool AreAzureBotChannelsConfigured { get; set; }

        public bool IsAlexaChannelConfigured { get; set; }
    }
}
