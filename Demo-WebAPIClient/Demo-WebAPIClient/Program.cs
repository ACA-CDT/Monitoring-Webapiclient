using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Demo_WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        const string urlTemplate = "https://dev-we-carfix-webapp.azurewebsites.net/Home/Index?CountryCode={country}&Year={year}";
        static async Task<int> Main(string[] args)
        {
            //-UNDER Construction:
            // Test if input arguments were supplied.
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter parameters url, requested method call, country code (BE,US,...) and year (yyyy).");
                Console.WriteLine("Usage: Demo-WebAPIClient -u <url> -d <data> -c <country code> -y <year>");
                Console.WriteLine("Example: Demo-WebAPIClient -u https://dev-we-carfix-webapp.azurewebsites.net -d Home/Index -c BE -y 2022");
                return 1;
            }
            ConnectWithHttpClient();

            //var repositories = await ProcessRepositories();

            //foreach (var repo in repositories)
            //{
            //    Console.WriteLine(repo.Name);
            //    Console.WriteLine(repo.Description);
            //    Console.WriteLine(repo.GitHubHomeUrl);
            //    Console.WriteLine(repo.Homepage);
            //    Console.WriteLine(repo.Watchers);
            //    Console.WriteLine(repo.LastPush);
            //    Console.WriteLine();
            //}
            Console.ReadKey();
            return 0;
        }
        public static async void ConnectWithHttpClient()
        {
            try
            {
                List<string> countryList = new List<string>() { "BE" , "UK", "US"};


                foreach (string country in countryList)
                {
                    var url = urlTemplate.Replace("{country}", country);

                    for (int i = 2000; i <= 2025; i++)
                    {
                        url = url.Replace("{year}", i.ToString());
                        //var httpClient = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Get, url);
                        var response = await client.SendAsync(request);
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(content);
                        response.Dispose();
                    }

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
      
        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }
}