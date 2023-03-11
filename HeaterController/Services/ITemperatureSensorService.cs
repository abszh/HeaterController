// -----------------------------------------------------------------------
// <copyright file="ITemperatureSensorService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

public interface ITemperatureSensorService
{
    /// <summary>
    /// Returns the temperature asynchronously.
    /// </summary>
    public Task<float?> GetTemperatureAsync(CancellationToken cancellationToken);
}
