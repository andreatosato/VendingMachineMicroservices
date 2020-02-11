﻿using System.Net.Http;

namespace VendingMachine.Service.Gateway.RefitModels
{
    public interface IAuthClient
    {
        HttpClient GetClient(string baseUrl);
    }

    public interface IAuthUserClient
    {
        HttpClient GetClient(string baseUrl, string username, string password);
    }
}
