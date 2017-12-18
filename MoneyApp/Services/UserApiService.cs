using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoneyApp.Dto;
using MoneyApp.Interfaces;
using MoneyApp.Models;

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

        public UserDto GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public Account LoadAnAccount(Guid accountGuid)
        {
            throw new NotImplementedException();
        }
    }
}
