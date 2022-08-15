using System;
using Splat;

namespace SyncedLyricsCreator.Extensions
{
    /// <summary>
    /// Provides extension methods for the Splat DI framework
    /// </summary>
    internal static class SplatExtensions
    {
        /// <summary>
        /// Gets a DI-resolved instance of the specified service
        /// </summary>
        /// <typeparam name="TService">The type of the service to resolve</typeparam>
        /// <param name="resolver">The DI resolver</param>
        /// <returns>The resolved instance</returns>
        /// <exception cref="InvalidOperationException">Thrown if the DI framework failed to resolve the service</exception>
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        {
            var service = resolver.GetService<TService>();
            if (service == null)
            {
                throw new InvalidOperationException($"Failed to resolve DI object of type {typeof(TService)}");
            }

            return service;
        }
    }
}
