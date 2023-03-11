// -----------------------------------------------------------------------
// <copyright file="IHeaterController.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Controllers;

using HeaterControllerApplication.Models;

public interface IHeaterController
{
    bool IsOnline { get; }

    HeaterSettings Settings { get;  set; }

    RelayStatus? RelayStatus { get; }

    float? Temperature { get; }

    Task Run(CancellationToken cancellationToken);
}
