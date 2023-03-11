// -----------------------------------------------------------------------
// <copyright file="ITimeService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services
{
    public interface ITimeService
    {
        DateTime Now { get; }
    }
}
