using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;

namespace ForeignExchangeRates.WebAPI.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static void ConfigurePollyPolicies(this IServiceCollection services)
        {
            var registry = new PolicyRegistry
            {
                { "HttpRequestPolicy", Policy.Handle<Exception>().RetryAsync(3)}
            };
            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(registry);
        }
    }
}
