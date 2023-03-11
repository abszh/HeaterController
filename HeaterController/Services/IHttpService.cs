// -----------------------------------------------------------------------
// <copyright file="IHttpService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

/// <summary>
/// Interface for a service to get a string from a url.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Gets a string from a url.
    /// </summary>
    /// <param name="url">The url.</param>
    /// <param name="cancellationToken">Cancellation token can be used to cancel the async operation.</param>
    /// <returns>Awaitable task that gives a string on success and null on failure.</returns>
    public Task<string?> GetAsync(string url, CancellationToken cancellationToken);
}
