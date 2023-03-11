// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication;

using System.Net;
using HeaterControllerApplication.Controllers;
using HeaterControllerApplication.Models;
using HeaterControllerApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

internal class Program
{
    private static readonly ManualResetEvent ShutDownEvent = new(false);
    private static readonly CancellationTokenSource CancellationTokenSource = new();
    private static Microsoft.Extensions.Logging.ILogger? logger;

    private static string LogFilePath =>
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        + @"\HeaterController\ControllerLogs.log";

    private static void PrintUsageAndExit(string? message = null)
    {
        if (message is not null)
        {
            Console.WriteLine(message);
        }

        Console.WriteLine("Usage: HeaterController <relayIpAddress> <sensorIpAddress>");
        Environment.Exit(-1);
    }

    private static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            PrintUsageAndExit();
        }

        var relayIpAddress = args[0];
        if (!IPAddress.TryParse(relayIpAddress, out _))
        {
            PrintUsageAndExit($"Invalid IP specified for the relay: {relayIpAddress}");
        }

        var sensorIpAddress = args[1];
        if (!IPAddress.TryParse(sensorIpAddress, out _))
        {
            PrintUsageAndExit($"Invalid IP specified for the sensor: {sensorIpAddress}");
        }

        Console.CancelKeyPress += Console_CancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

        var httpClient = new HttpClient();
        var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                logging.AddNLog("nlog.config");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<IHttpService>(
                    sp => ActivatorUtilities.CreateInstance<HttpService>(sp, httpClient));
                services.AddSingleton<IRelayService>(
                    sp => ActivatorUtilities.CreateInstance<ShellyRelayService>(sp, relayIpAddress));
                services.AddSingleton<ITemperatureSensorService>(
                    sp => ActivatorUtilities.CreateInstance<ShellyTemperatureSensorService>(sp, sensorIpAddress));
                services.AddSingleton<ITimeService, TimeService>();
                services.AddSingleton<IHeaterLogicService, HeaterLogicService>();
                services.AddSingleton<IHeaterController, HeaterController>();
            })
            .Build();

        LogManager.Configuration.Variables["logFileName"] = LogFilePath;
        host.Start();
        logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Program started.");

        var heaterController = host.Services.GetRequiredService<IHeaterController>();

        heaterController.Settings = new HeaterSettings(
            TurnOnTemperature: 19,
            TurnOffTemperature: 20,
            MaxOnTime: TimeSpan.FromMinutes(6),
            MinOffTime: TimeSpan.FromMinutes(6));

        try
        {
            await heaterController.Run(CancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            logger.LogDebug("Heater task cancelled.");
        }

        httpClient.Dispose();
        logger.LogInformation("Exiting the program.");
        ShutDownEvent.Set();
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        logger?.LogInformation("Console window closing.");
        CancellationTokenSource.Cancel();
        ShutDownEvent.WaitOne();
    }

    private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        logger?.LogInformation("Ctrl+C pressed.");
        CancellationTokenSource.Cancel();
        ShutDownEvent.WaitOne();
    }
}
