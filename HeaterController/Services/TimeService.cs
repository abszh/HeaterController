// -----------------------------------------------------------------------
// <copyright file="TimeService.cs" company="Abbas Shahzadeh">
// Copyright (c) Abbas Shahzadeh. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace HeaterControllerApplication.Services
{
    public class TimeService : ITimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
