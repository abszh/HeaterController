// -----------------------------------------------------------------------
// <copyright file="IRelayService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using HeaterControllerApplication.Models;

/// <summary>
/// Interface to communicate with a relay.
/// </summary>
public interface IRelayService
{
    /// <summary>
    /// Gets the current status of the relay asynchronously.
    /// The task return on/off status of the relay if successful.
    /// If unsuccessful, it returns null.
    /// </summary>
    public Task<RelayStatus?> GetStatusAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Turns on the relay asynchronously.
    /// </summary>
    public Task TurnOnAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Turns off the relay asynchronously.
    /// </summary>
    public Task TurnOffAsync(CancellationToken cancellationToken);
}
