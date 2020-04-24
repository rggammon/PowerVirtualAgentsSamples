// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Threading.Tasks;

namespace SampleBot.Extensions
{
    public static class ITokenAcquisitionExtensions
    {
        private static readonly string[] _armScopes = new string[] { "openid", "https://management.core.windows.net/user_impersonation" };
        private static readonly string[] _cdsScopes = new string[] { "openid", "00000007-0000-0000-c000-000000000000/user_impersonation" };
        private static readonly string[] _graphScopes = new string[] { "openid", "profile", "User.Read", "Application.ReadWrite.All" };
        
        public static string[] GetArmScopes(this ITokenAcquisition tokenAcquisition)
        {
            return _armScopes;
        }

        public static string[] GetCdsScopes(this ITokenAcquisition tokenAcquisition)
        {
            return _cdsScopes;
        }

        public static string[] GetGraphScopes(this ITokenAcquisition tokenAcquisition)
        {
            return _graphScopes;
        }

        public static async Task<string> GetArmTokenAsync(this ITokenAcquisition tokenAcquisition)
        {
            return await GetAccessTokenAsync(tokenAcquisition, _armScopes);
        }

        public static async Task<string> GetCdsTokenAsync(this ITokenAcquisition tokenAcquisition)
        {
            return await GetAccessTokenAsync(tokenAcquisition, _cdsScopes);
        }

        public static async Task<string> GetGraphTokenAsync(this ITokenAcquisition tokenAcquisition)
        {
            return await GetAccessTokenAsync(tokenAcquisition, _graphScopes);
        }

        private static async Task<string> GetAccessTokenAsync(ITokenAcquisition tokenAcquisition, string[] scopes)
        {
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
