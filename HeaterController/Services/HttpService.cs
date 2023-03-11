// -----------------------------------------------------------------------
// <copyright file="HttpService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using Microsoft.Extensions.Logging;

public class HttpService : IHttpService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<HttpService> logger;

    public HttpService(ILogger<HttpService> logger, HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<string?> GetAsync(string url, CancellationToken cancellationToken)
    {
        string responseBody;
        try
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // When cancellation token is set, GetAsync throws an exception.
            // This is a signal to end the process. Rethrow so that the
            // process is ended.
            this.logger.LogInformation("Http reuquest to {url} was cancelled.", url);
            throw;
        }
        catch (Exception ex)
        {
            // Swallow any other exceptions and return null to signal
            // that something went wrong. This is a more functional approach.
            this.logger.LogWarning(ex, "Failed to read from {url}", url);
            return null;
        }

        return responseBody;
    }
}
