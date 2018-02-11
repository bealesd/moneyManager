using System;
using System.Collections.Generic;
using System.Net.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;
using MoneyApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
        public IUser CreateUser(string username, string password)
        {
            string path = $"{_apiPath}/user/{username}/{password}";
            var httpResponse = _client.PostAsync(path, null).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Create user failure.");
            return httpResponse.Content.ReadAsAsync<User>().Result;
        }

        public UserDto GetUserDto(Guid userGuid)
        {
            var user = GetUser(userGuid);
            var userDto = new UserDto() {UserGuid = user.UserGuid, Accounts = new List<Account>() };
            user.AccountGuid.ForEach(guid => userDto.Accounts.Add(LoadMoneyAccount(guid)));
            return userDto;
        }


        public void DeleteUser(Guid userGuid)
        {
            if(userGuid == Guid.Empty)
                throw new Exception("Delete user failure. Empty userGuid.");
            var httpResponse = _client.DeleteAsync($"{_apiPath}/user/deleteUser/{userGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Delete user failure.");
        }


        private User GetUser(Guid userGuid)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/{userGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find user failure, invalid userGuid.");

            return httpResponse.Content.ReadAsAsync<User>().Result;
        }

        public Guid GetUserGuid(string username, string password)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/{username}/{password}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find user failure, invalid credentials.");

            return httpResponse.Content.ReadAsAsync<Guid>().Result;
        }


        public Account LoadMoneyAccount(Guid accountGuid)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/account/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Find account failure, invalid accountGuid.");
            return httpResponse.Content.ReadAsAsync<Account>().Result;
        }

        public void DeleteAccount(Guid userGuid, Guid accountGuid)
        {
            if (accountGuid == Guid.Empty)
                throw new Exception("Delete accountfailure. Empty accountGuid.");
            var path = $"{_apiPath}/user/{userGuid}/{accountGuid}";
            var httpResponse = _client.DeleteAsync($"{_apiPath}/user/{userGuid}/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Delete accountfailure.");
        }

        public void CreateMoneyAccountForUser(string accountName, Guid userGuid)
        {
            var httpResponse = _client.PostAsync($"{_apiPath}/user/account/{userGuid}/{accountName}", null).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception("Create account failure.");
        }

        public void CreateMoneySpentItem(Guid accountGuid, MoneySpentItemDto moneySpentItem)
        {
            var myContent = JsonConvert.SerializeObject(moneySpentItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponse = _client.PostAsync($"{_apiPath}/user/account/{accountGuid}", byteContent).Result;
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
