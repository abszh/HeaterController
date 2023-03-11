// -----------------------------------------------------------------------
// <copyright file="HeaterController.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Controllers;

using HeaterControllerApplication.Models;
using HeaterControllerApplication.Services;
using Microsoft.Extensions.Logging;

public class HeaterController : IHeaterController
{
    private const int DelayIfOnline = 100;
    private const int DelayIfOffline = 10000;

    private readonly ILogger<HeaterController> logger;
    private readonly IRelayService relayService;
    private readonly ITimeService timeService;
    private readonly ITemperatureSensorService temperatureSensorService;
    private readonly IHeaterLogicService heaterLogicService;
    private DateTime lastChangeTime;

    public HeaterController(
        ILogger<HeaterController> logger,
        IRelayService relayService,
        ITimeService timeService,
        ITemperatureSensorService temperatureSensorService,
        IHeaterLogicService heaterLogicService)
    {
        this.logger = logger;
        this.relayService = relayService;
        this.temperatureSensorService = temperatureSensorService;
        this.timeService = timeService;
        this.heaterLogicService = heaterLogicService;
        this.lastChangeTime = DateTime.MinValue;
    }

    public bool IsOnline => this.RelayStatus is not null && this.Temperature is not null;

    public HeaterSettings Settings { get; set; }

    public RelayStatus? RelayStatus { get; private set; }

    public float? Temperature { get; private set; }

    public async Task Run(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            (this.RelayStatus, this.Temperature) = await GetHeaterState(this.relayService, this.temperatureSensorService, cancellationToken);
            if (this.RelayStatus is null || this.Temperature is null)
            {
                if (this.RelayStatus is null)
                {
                    this.logger.LogError("Relay is offline");
                }

                if (this.Temperature is null)
                {
                    this.logger.LogError("Temperature sensor is offline");
                }

                await Task.Delay(DelayIfOffline, cancellationToken);
                continue;
            }

            var timeSinceLastChange = this.timeService.Now - this.lastChangeTime;
            var heaterAction = this.heaterLogicService.HeaterLogic(
                this.Settings,
                this.Temperature.Value,
                this.RelayStatus.Value,
                timeSinceLastChange);

            if (heaterAction == HeaterActions.TurnOn)
            {
                await this.relayService.TurnOnAsync(cancellationToken);
                this.logger.LogInformation($"Turned the relay on. Temperature is {this.Temperature}, time since last change {timeSinceLastChange}");
                this.lastChangeTime = this.timeService.Now;
            }
            else if (heaterAction == HeaterActions.TurnOff)
            {
                await this.relayService.TurnOnAsync(cancellationToken);
                this.logger.LogInformation($"Turned the relay off. Temperature is {this.Temperature}, time since last change {timeSinceLastChange}");
                this.lastChangeTime = this.timeService.Now;
            }

            await Task.Delay(DelayIfOnline, cancellationToken);
        }
    }

    private static async Task<HeaterState> GetHeaterState(
        IRelayService relayService,
        ITemperatureSensorService temperatureSensorService,
        CancellationToken cancellationToken)
    {
        var relayTask = relayService.GetStatusAsync(cancellationToken);
        var temperatureSensorTask = temperatureSensorService.GetTemperatureAsync(cancellationToken);
        await Task.WhenAll(relayTask, temperatureSensorTask);
        return new HeaterState(relayTask.Result, temperatureSensorTask.Result);
    }

    private readonly record struct HeaterState(RelayStatus? RelayStatus, float? Temperature);
}
