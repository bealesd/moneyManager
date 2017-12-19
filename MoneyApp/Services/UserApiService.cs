using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        public IUser CreateUser(string username)
        {
            string path = $"{_apiPath}/user/{username}";
            var httpResponse = _client.PostAsync(path, null).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception();
            return httpResponse.Content.ReadAsAsync<User>().Result;
        }

        public UserDto GetUserDto(string username)
        {
            var user = GetUser(username);
            var userDto = new UserDto() { Username = user.Username, UserGuid = user.UserGuid, Accounts = new List<Account>() };
            user.AccountGuid.ForEach(guid => userDto.Accounts.Add(LoadAnAccount(guid)));
            return userDto;
        }

        private User GetUser(string username)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/{username}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception();

            return httpResponse.Content.ReadAsAsync<User>().Result;
        }

        public Account LoadAnAccount(Guid accountGuid)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/account/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception();
            return httpResponse.Content.ReadAsAsync<Account>().Result;
        }

        public void CreateMoneySpentItem(Guid accountGuid, MoneySpentItemDto moneySpentItem)
        {
            var myContent = JsonConvert.SerializeObject(moneySpentItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpResponse = _client.PostAsync($"{_apiPath}/user/account/{accountGuid}", byteContent).Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception();
        }

        public void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var httpResponse = _client.DeleteAsync($"{_apiPath}/user/account/{accountGuid}/{moneyItemGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                throw new Exception();
        }
    }
}
