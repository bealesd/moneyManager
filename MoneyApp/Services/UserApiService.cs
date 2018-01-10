using System;
using System.Collections.Generic;
using System.Net.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;
using MoneyApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace MoneyApp.Services
{
    public class UserApiService : IUserApiService
    {
        private string _apiPath;
        private HttpClient _client;

        public UserApiService()
        {
            _apiPath = "http://localhost:37266/api";
            _client = new HttpClient();
        }

        public void Register(string username, string password)
        {
            string path = $"{_apiPath}/credentials?username={username}&Password={password}&ConfirmPassword={password}";
            var httpResponse = _client.PostAsync(path, null).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Create user failure.");
        }

        public UserDto GetUserDto()
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/User/user").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find user failure, invalid user.");

            var user = httpResponse.Content.ReadAsAsync<User>().Result;
            var userDto = new UserDto() { Username = user.Username, Accounts = new List<Account>() };
            user.AccountGuid.ForEach(guid => userDto.Accounts.Add(LoadMoneyAccount(guid)));
            return userDto;
        }

        public void Login(string username, string password)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/credentials?username={username}&Password={password}&RememberMe={false}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find user failure, invalid username or password.");
        }

        public void Logout()
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/credentials/Logout").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Couldn't logout.");
        }

        public void DeleteUser()
        {
            var httpResponse = _client.DeleteAsync($"{_apiPath}/credentials").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Delete user failure.");
        }

        public Account LoadMoneyAccount(Guid accountGuid)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/account/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find account failure, invalid accountGuid.");
            return httpResponse.Content.ReadAsAsync<Account>().Result;
        }

        public void DeleteAccount(Guid accountGuid)
        {
            if (accountGuid == Guid.Empty)
                throw new Exception("Delete accountfailure. Empty accountGuid.");
            var path = $"{_apiPath}/user/{accountGuid}";
            var httpResponse = _client.DeleteAsync($"{_apiPath}/user/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Delete accountfailure.");
        }

        public void CreateMoneyAccountForUser(string accountName)
        {
            var httpResponse = _client.PostAsync($"{_apiPath}/user/account/{accountName}", null).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Create account failure.");
        }

        public void CreateMoneySpentItem(Guid accountGuid, MoneySpentItemDto moneySpentItem)
        {
            var myContent = JsonConvert.SerializeObject(moneySpentItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponse = _client.PostAsync($"{_apiPath}/user/account/moneySpentItem/{accountGuid}", byteContent).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Create transaction failure.");
        }

        public void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var httpResponse = _client.DeleteAsync($"{_apiPath}/user/account/{accountGuid}/{moneyItemGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Delete transaction failure.");
        }
    }
}