﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.Session;
using Microsoft.PowerVirtualAgents.Samples.RelayBotSample.Bots;
using SampleBot.Configurations;

namespace Microsoft.PowerVirtualAgents.Samples.RelayBotSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(option =>
            {
                option.Cookie.IsEssential = true;
            });

            services.AddMicrosoftIdentityPlatformAuthentication(Configuration)
                .AddMsal(Configuration, new string[] { "User.Read" })
                .AddSessionTokenCaches();

            services.AddAuthorization();

            services.AddControllersWithViews();

            // Configure SetSpeakMiddleware
            var setSpeakMiddlewareSection = Configuration.GetSection("SetSpeakMiddleware");
            services.Configure<SetSpeakMiddlewareConfiguration>(setSpeakMiddlewareSection);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddSingleton<IBot, RelayBot>();

            // Create the singleton instance of BotService from appsettings
            var botService = new BotService();
            Configuration.Bind("BotService", (object)botService);
            services.AddSingleton<IBotService>(botService);

            // Create the singleton instance of ConversationPool from appsettings
            var conversationManager = new ConversationManager();
            Configuration.Bind("ConversationPool", conversationManager);
            services.AddSingleton(conversationManager);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseWebSockets();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
