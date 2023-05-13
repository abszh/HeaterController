// -----------------------------------------------------------------------
// <copyright file="HeaterLogicService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using HeaterControllerApplication.Models;

public class HeaterLogicService : IHeaterLogicService
{
    /// <inheritdoc/>
    public HeaterActions HeaterLogic(HeaterSettings settings, float temperature, RelayStatus relayStatus, TimeSpan timeSinceLastChange)
    {
        if (relayStatus == RelayStatus.On &&
            (temperature >= settings.TurnOffTemperature ||
            timeSinceLastChange >= settings.MaxOnTime))
        {
            return HeaterActions.TurnOff;
        }

        if (relayStatus == RelayStatus.Off &&
            timeSinceLastChange >= settings.MinOffTime &&
            temperature <= settings.TurnOnTemperature)
        {
            return HeaterActions.TurnOn;
        }

        return HeaterActions.NoChange;
    }
}
