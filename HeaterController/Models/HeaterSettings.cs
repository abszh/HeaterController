// -----------------------------------------------------------------------
// <copyright file="HeaterSettings.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Models;

/// <summary>
/// Heater settings.
/// </summary>
/// <param name="TurnOffTemperature">Heater will turn off when temperature gets to this value.</param>
/// <param name="TurnOnTemperature">Heater can turn off when temperature drops to this value.</param>
/// <param name="MaxOnTime">Maximum time that heater can stay on.</param>
/// <param name="MinOffTime">Minimum time that the heater must stay off, before it can turn on again.</param>
public readonly record struct HeaterSettings(float TurnOffTemperature, float TurnOnTemperature, TimeSpan MaxOnTime, TimeSpan MinOffTime);
