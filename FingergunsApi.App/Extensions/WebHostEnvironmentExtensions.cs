using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FingergunsApi.App.Extensions
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsLocal(this IWebHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return hostEnvironment.IsEnvironment("Local");
        }
    }
}