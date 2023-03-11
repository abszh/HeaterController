// -----------------------------------------------------------------------
// <copyright file="HeaterLogicTest.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerTests;

using System;
using HeaterControllerApplication.Models;
using HeaterControllerApplication.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HeaterLogicTest
{
    private readonly IHeaterLogicService heaterLogic = new HeaterLogicService();

    // Heater turns off immediately if warm enough.
    [TestMethod]
    public void HeaterTurnsOffImmediatelyIfWarmEnough()
    {
        var heaterSettings = new HeaterSettings(
            TurnOnTemperature: 19,
            TurnOffTemperature: 20,
            MaxOnTime: TimeSpan.MaxValue,
            MinOffTime: TimeSpan.Zero);

        var action = this.heaterLogic.HeaterLogic(
            heaterSettings: heaterSettings,
            currentTemperature: 21,
            currentRelayStatus: RelayStatus.On,
            timeSinceLastChange: TimeSpan.Zero);

        Assert.AreEqual(action, HeaterActions.TurnOff);
    }

    // Heater stays off forever if warm enough.
    [TestMethod]
    public void HeaterStaysOffIfWarmEnough()
    {
        var heaterSettings = new HeaterSettings(
            TurnOnTemperature: 19,
            TurnOffTemperature: 20,
            MaxOnTime: TimeSpan.MaxValue,
            MinOffTime: TimeSpan.Zero);

        var action = this.heaterLogic.HeaterLogic(
            heaterSettings: heaterSettings,
            currentTemperature: 21,
            currentRelayStatus: RelayStatus.Off,
            timeSinceLastChange: TimeSpan.MaxValue);

        Assert.AreEqual(action, HeaterActions.NoChange);
    }

    // Heater doesn't turn on unless it has been off for the minimum off time.
    [TestMethod]
    public void HeaterDoesntTurnOfUnlessItHasBeenOffLongEnough()
    {
        var heaterSettings = new HeaterSettings(
            TurnOnTemperature: 19,
            TurnOffTemperature: 20,
            MaxOnTime: TimeSpan.MaxValue,
            MinOffTime: TimeSpan.FromMinutes(10));

        var action = this.heaterLogic.HeaterLogic(
            heaterSettings: heaterSettings,
            currentTemperature: 5,
            currentRelayStatus: RelayStatus.Off,
            timeSinceLastChange: TimeSpan.FromMinutes(5));

        Assert.AreEqual(action, HeaterActions.NoChange);

        action = this.heaterLogic.HeaterLogic(
            heaterSettings: heaterSettings,
            currentTemperature: 5,
            currentRelayStatus: RelayStatus.Off,
            timeSinceLastChange: TimeSpan.FromMinutes(11));

        Assert.AreEqual(action, HeaterActions.TurnOn);
    }

    // Heater turns off if has been on for max on time even if still cold.
    [TestMethod]
    public void HeaterTurnsOffAfterMaxOnTime()
    {
        var heaterSettings = new HeaterSettings(
            TurnOnTemperature: 19,
            TurnOffTemperature: 20,
            MaxOnTime: TimeSpan.FromMinutes(5),
            MinOffTime: TimeSpan.FromMinutes(10));

        var action = this.heaterLogic.HeaterLogic(
            heaterSettings: heaterSettings,
            currentTemperature: 5,
            currentRelayStatus: RelayStatus.On,
            timeSinceLastChange: TimeSpan.FromMinutes(6));

        Assert.AreEqual(action, HeaterActions.TurnOff);
    }
}