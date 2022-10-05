// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace courseappchallenge.Services;

public interface IEmailService
{
    Task SendAsync(string toName, string toEmail, string subject, string body);
}
