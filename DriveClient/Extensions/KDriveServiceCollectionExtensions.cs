using kDriveClient.kDriveClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace kDriveClient.Extensions
{
    /// <summary>
    /// DI registration helpers for kDriveClient.
    /// </summary>
    public static class KDriveServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="KDriveClient"/> and domain interfaces against a shared scoped instance.
        /// Uses <see cref="IHttpClientFactory"/> for the underlying HTTP client.
        /// Upload bandwidth probing is deferred until the first upload (or call <see cref="KDriveClient.CreateAsync"/> yourself).
        /// </summary>
        /// <remarks>
        /// Registers a named HttpClient <c>kDrive</c> by default. If you set
        /// <see cref="KDriveClientOptions.HttpClientName"/> to another value, register that named client yourself
        /// (or keep the default name).
        /// </remarks>
        public static IServiceCollection AddKDriveClient(
            this IServiceCollection services,
            Action<KDriveClientOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configure);

            services.AddOptions<KDriveClientOptions>()
                .Configure(configure)
                .Validate(o => !string.IsNullOrWhiteSpace(o.Token), "KDriveClientOptions.Token is required.")
                .Validate(o => o.DriveId > 0, "KDriveClientOptions.DriveId must be a positive value.")
                .Validate(o => o.BaseAddress is { IsAbsoluteUri: true }, "KDriveClientOptions.BaseAddress must be an absolute URI.")
                .Validate(o => o.AccountId is null or > 0, "KDriveClientOptions.AccountId must be positive when set.")
                .ValidateOnStart();

            services.AddHttpClient("kDrive")
                .ConfigureHttpClient((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<KDriveClientOptions>>().Value;
                    client.BaseAddress = options.BaseAddress;
                    if (!string.IsNullOrWhiteSpace(options.Token))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", options.Token);
                    }
                });

            services.AddScoped<KDriveClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<KDriveClientOptions>>().Value;
                if (string.IsNullOrWhiteSpace(options.Token))
                    throw new InvalidOperationException("KDriveClientOptions.Token is required.");
                if (options.DriveId <= 0)
                    throw new InvalidOperationException("KDriveClientOptions.DriveId must be a positive value.");
                if (!options.BaseAddress.IsAbsoluteUri)
                    throw new InvalidOperationException("KDriveClientOptions.BaseAddress must be an absolute URI.");

                var clientName = string.IsNullOrWhiteSpace(options.HttpClientName) ? "kDrive" : options.HttpClientName;
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var http = factory.CreateClient(clientName);
                http.BaseAddress ??= options.BaseAddress;
                if (http.DefaultRequestHeaders.Authorization is null && !string.IsNullOrWhiteSpace(options.Token))
                {
                    http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", options.Token);
                }

                var logger = sp.GetService<ILogger<KDriveClient>>();
                return new KDriveClient(options.Token, options.DriveId, options.Upload, logger, http);
            });

            services.AddScoped<IKDriveClient>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveUpload>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveDownload>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveFiles>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveShares>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveComments>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveCategories>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveDrive>(sp => sp.GetRequiredService<KDriveClient>());
            services.AddScoped<IKDriveSmart>(sp => sp.GetRequiredService<KDriveClient>());

            return services;
        }
    }
}
