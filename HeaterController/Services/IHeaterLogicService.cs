// -----------------------------------------------------------------------
// <copyright file="IHeaterLogicService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using HeaterControllerApplication.Models;

/// <summary>
/// Interface for heater logic.
/// </summary>
public interface IHeaterLogicService
{
    /// <summary>
    /// Determines the next action for the heater.
    /// </summary>
    /// <param name="heaterSettings">Heater settings.</param>
    /// <param name="currentTemperature">Current temperature.</param>
    /// <param name="currentRelayStatus">Current status of the relay.</param>
    /// <param name="timeSinceLastChange">Time since the last change applied to the relay.</param>
    /// <returns>Heater action can be turn on, turn off or no change.</returns>
    HeaterActions HeaterLogic(HeaterSettings heaterSettings, float currentTemperature, RelayStatus currentRelayStatus, TimeSpan timeSinceLastChange);
}
