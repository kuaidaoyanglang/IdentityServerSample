using System;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace PwdClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // 从元数据中发现终结点,查找IdentityServer(5000端口)
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            Console.WriteLine(disco.HttpResponse.Content.ReadAsStringAsync().Result);
            //向IdentityServer请求令牌
            var tClient = new HttpClient();
            tClient.BaseAddress = new Uri("http://localhost:5000/connect/token");
            var tokenClient = new TokenClient(tClient, new TokenClientOptions()
            {
                ClientId = "PasswordClient",
                ClientSecret = "secret"
            });
            //var tokenResponse = tokenClient.RequestClientCredentialsAsync("api");
            var tokenResponse = tokenClient.RequestPasswordTokenAsync("davy","123456","api").Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            //访问Api
            //var client = new HttpClient();
            //把令牌添加进请求
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = client.GetAsync("http://localhost:5002/WeatherForecast").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
