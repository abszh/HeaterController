// -----------------------------------------------------------------------
// <copyright file="ShellyRelayServiceTest.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerTests;

using System.Threading;
using System.Threading.Tasks;
using HeaterControllerApplication.Models;
using HeaterControllerApplication.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class ShellyRelayServiceTest
{
    // GetStatusAsync interprets response from the relay correctly
    [TestMethod]
    public async Task GetStatusAsyncInterpretsResponseCorrectly()
    {
        var mockLogger = new Mock<ILogger<ShellyRelayService>>();
        var mockHttpService = new Mock<IHttpService>();
        string ipAddress = "1.2.3.4";
        string httpResponse = @"{""relays"": [{""ison"": true}]}";
        mockHttpService.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<string?>(httpResponse));

        IRelayService shellyRelayService = new ShellyRelayService(mockLogger.Object, mockHttpService.Object, ipAddress);
        RelayStatus? relayStatus = await shellyRelayService.GetStatusAsync(CancellationToken.None);
        Assert.IsNotNull(relayStatus);
        Assert.AreEqual(RelayStatus.On, relayStatus);
    }

    // GetStatusAsync sends correct http request
    [TestMethod]
    public async Task GetStatusAsyncSendsCorrectHttpRequest()
    {
        var mockLogger = new Mock<ILogger<ShellyRelayService>>();
        var mockHttpService = new Mock<IHttpService>();
        string ipAddress = "1.2.3.4";
        string httpRequest = string.Empty;
        mockHttpService.Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, CancellationToken>((url, cancelllationToken) =>
            {
                Assert.AreEqual(url, "http://1.2.3.4/status");
            })
            .Returns(Task.FromResult<string?>(string.Empty));

        IRelayService shellyRelayService = new ShellyRelayService(mockLogger.Object, mockHttpService.Object, ipAddress);
        _ = await shellyRelayService.GetStatusAsync(CancellationToken.None);
        mockHttpService.Verify(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // TurnOnAsync sends correct http request
    [TestMethod]
    public async Task TurnOnAsyncSendsCorrectHttpRequest()
    {
        var mockLogger = new Mock<ILogger<ShellyRelayService>>();
        var mockHttpService = new Mock<IHttpService>();
        string ipAddress = "1.2.3.4";
        var shellyRelayService = new ShellyRelayService(mockLogger.Object, mockHttpService.Object, ipAddress);
        await shellyRelayService.TurnOnAsync(CancellationToken.None);
        mockHttpService.Verify(
            m => m.GetAsync(
                It.Is<string>(s => s == "http://1.2.3.4/relay/0?turn=on"),
                It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Once);
    }

    // TurnOffAsync sends correct http request
    [TestMethod]
    public async Task TurnOffAsyncSendsCorrectHttpRequest()
    {
        var mockLogger = new Mock<ILogger<ShellyRelayService>>();
        var mockHttpService = new Mock<IHttpService>();
        string ipAddress = "1.2.3.4";
        IRelayService shellyRelayService = new ShellyRelayService(mockLogger.Object, mockHttpService.Object, ipAddress);
        await shellyRelayService.TurnOffAsync(CancellationToken.None);
        mockHttpService.Verify(
            m => m.GetAsync(
                It.Is<string>(s => s == "http://1.2.3.4/relay/0?turn=off"),
                It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Once);
    }
}
