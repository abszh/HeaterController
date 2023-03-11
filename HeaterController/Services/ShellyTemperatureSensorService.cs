// -----------------------------------------------------------------------
// <copyright file="ShellyTemperatureSensorService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using System.Text.Json;
using HeaterControllerApplication.Models;
using Microsoft.Extensions.Logging;

public class ShellyTemperatureSensorService : ITemperatureSensorService
{
    private readonly ILogger<ShellyTemperatureSensorService> logger;
    private readonly IHttpService httpService;
    private readonly string ipAddress;

    public ShellyTemperatureSensorService(ILogger<ShellyTemperatureSensorService> logger, IHttpService httpService, string ipAddress)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.ipAddress = ipAddress;
    }

    public async Task<float?> GetTemperatureAsync(CancellationToken cancellationToken)
    {
        string? httpResponse = await this.httpService.GetAsync("http://" + this.ipAddress + "/status", cancellationToken);
        if (httpResponse == null)
        {
            return null;
        }

        try
        {
            ShellySensorStatus? shellyStatus = JsonSerializer.Deserialize<ShellySensorStatus>(httpResponse);
            if (shellyStatus == null || shellyStatus.ext_temperature == null || shellyStatus.ext_temperature._0 == null)
            {
                this.logger.LogWarning("Invalid or null response from the temperature sensor");
                return null;
            }

            float temperature = shellyStatus.ext_temperature._0.tC;
            return temperature;
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to deserilize");
            return null;
        }
    }
}
