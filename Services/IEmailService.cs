// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace courseappchallenge.Services;

public interface IEmailService
{
    bool Send(string email, string subject, string message);
}
