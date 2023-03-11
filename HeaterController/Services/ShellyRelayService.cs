// -----------------------------------------------------------------------
// <copyright file="ShellyRelayService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services;

using System.Text.Json;
using HeaterControllerApplication.Models;
using Microsoft.Extensions.Logging;

public class ShellyRelayService : IRelayService
{
    private readonly ILogger<ShellyRelayService> logger;
    private readonly IHttpService httpService;
    private readonly string ipAddress;

    public ShellyRelayService(ILogger<ShellyRelayService> logger, IHttpService httpService, string ipAddress)
    {
        this.logger = logger;
        this.httpService = httpService;
        this.ipAddress = ipAddress;
    }

    public async Task<RelayStatus?> GetStatusAsync(CancellationToken cancellationToken)
    {
        var url = $"http://{this.ipAddress}/status";
        var httpResponse = await this.httpService.GetAsync(url, cancellationToken);
        if (httpResponse == null)
        {
            return null;
        }

        try
        {
            ShellyRelayStatus? shellyStatus = JsonSerializer.Deserialize<ShellyRelayStatus>(httpResponse);
            if (shellyStatus == null || shellyStatus.relays == null)
            {
                this.logger.LogWarning("Json desersilizer returned null.");
                return null;
            }

            return shellyStatus.relays[0].ison ? RelayStatus.On : RelayStatus.Off;
        }
        catch (Exception ex)
        {
            this.logger.LogError("Failed to deserilize", ex);
            return null;
        }
    }

    public async Task TurnOffAsync(CancellationToken cancellationToken)
    {
        var url = $"http://{this.ipAddress}/relay/0?turn=off";
        await this.httpService.GetAsync(url, cancellationToken);
    }

    public async Task TurnOnAsync(CancellationToken cancellationToken)
    {
        var url = $"http://{this.ipAddress}/relay/0?turn=on";
        await this.httpService.GetAsync(url, cancellationToken);
    }
}
