// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleBot.Extensions
{
    public static class ITokenAcquisitionExtensions
    {
        private const string _armScope = "https://management.core.windows.net/user_impersonation";
        private const string _cdsScope = "00000007-0000-0000-c000-000000000000/user_impersonation";

        public static string[] GetArmScope(this ITokenAcquisition tokenAcquisition)
        {
            return new string[]
            {
                _armScope
            };
        }

        public static async Task<string> GetArmTokenAsync(this ITokenAcquisition tokenAcquisition)
        {
            return await GetAccessTokenAsync(tokenAcquisition, _armScope);
        }

        public static async Task<string> GetCdsTokenAsync(this ITokenAcquisition tokenAcquisition)
        {
            return await GetAccessTokenAsync(tokenAcquisition, _cdsScope);
        }

        private static async Task<string> GetAccessTokenAsync(ITokenAcquisition tokenAcquisition, string scope)
        {
            var scopes = new List<string>() {
                scope
            };

            string token = null;
            try
            {
                token = await tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(scopes);
            }
            catch (MsalUiRequiredException)
            {
            }

            return token;
        }
    }
}
