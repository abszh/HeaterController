// -----------------------------------------------------------------------
// <copyright file="ShellyTemperatureSensorTest.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerTests;

using System.Threading;
using System.Threading.Tasks;
using HeaterControllerApplication.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class ShellyTemperatureSensorTest
{
    // GetTemperatureAsync interprets response from the sensor correctly.
    [TestMethod]
    public async Task GetTemperatureAsyncInterpretsResponseCorrectly()
    {
        var mockLogger = new Mock<ILogger<ShellyTemperatureSensorService>>();
        var mockHttpService = new Mock<IHttpService>();
        string ipAddress = "1.2.3.4";
        string httpResponse = @"{""ext_temperature"": {""0"": {""tC"":1.72}}}";

        mockHttpService.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<string?>(httpResponse));

        ITemperatureSensorService shellySensorService = new ShellyTemperatureSensorService(mockLogger.Object, mockHttpService.Object, ipAddress);
        var cancellationTokenSource = new CancellationTokenSource();
        float? temperature = await shellySensorService.GetTemperatureAsync(cancellationTokenSource.Token);
        Assert.IsNotNull(temperature);
        Assert.AreEqual(1.72f, temperature.Value);
    }
}
