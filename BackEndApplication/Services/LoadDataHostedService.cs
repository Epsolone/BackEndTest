using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BackEndApplication.Interfaces;
using BackEndApplication.Models;
using BackEndApplication.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BackEndApplication.Services
{
    public class LoadDataHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private ICachedDataService _cachedDataService;

        private TimeSpan SleepDelay => TimeSpan.FromMinutes(1);
        private const string serverUrl = "http://interview.agileengine.com";

        private class AuthModel
        {
            public bool Auth { get; set; }
            public string Token { get; set; }
        }

        public LoadDataHostedService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        _cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();

                        await LoadImages();
                    }
                }
                catch
                {
                    _cachedDataService.LoadingFailed();
                }

                await Task.Delay(SleepDelay, stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested);

            Dispose();
        }

        private async Task LoadImages()
        {
            var result = new List<Image>();

            var authorizeHeader = await GetHeader();

            int page = 1;
            bool allPagesLoaded = false;
            do
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = authorizeHeader;

                    var pageResponse = await client.GetAsync($"{serverUrl}/images?page={page}");

                    if(pageResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var pageResult = ParseContent<Page>(pageResponse);

                        var imageTasks = new List<Task<Image>>();

                        foreach (var image in pageResult.Pictures)
                        {
                            imageTasks.Add(Task.Factory.StartNew(() => GetImage(image.Id, client).Result));
                        }

                        Task.WaitAll(imageTasks.ToArray(), 600);

                        foreach (var imageTask in imageTasks)
                        {
                            var imageResult = imageTask.Result;

                            if(imageResult != null)
                            {
                                result.Add(imageResult);
                            }
                        }

                        allPagesLoaded = !pageResult.HasMore;

                        throw new Exception();
                    }
                    else
                    {
                        allPagesLoaded = true;
                    }
                }

                page++;
            } while (!allPagesLoaded);

            _cachedDataService.CacheImages(result);
        }

        private async Task<Image> GetImage(string id, HttpClient client)
        {
            var pageResponse = await client.GetAsync($"{serverUrl}/images/{id}");

            if (pageResponse.StatusCode == HttpStatusCode.OK)
            {
                return ParseContent<Image>(pageResponse);
            }

            return null;
        }

        private async Task<AuthenticationHeaderValue> GetHeader()
        {
            var token = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client
                        .PostAsync(
                            $"{serverUrl}/auth",
                            new JsonContent(new { apiKey = "23567b218376f79d9415" })).Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else
                    {
                        token = ParseContent<AuthModel>(response).Token;
                    }

                }
            }
            catch (Exception e)
            {
                throw new ApplicationException();
            }

            return new AuthenticationHeaderValue("Bearer", token);
        }

        private T ParseContent<T>(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
