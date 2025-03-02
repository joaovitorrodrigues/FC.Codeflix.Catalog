﻿using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
            => _httpClient = httpClient;


        public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string route, object payload) where TOutput : class
        {
            var response = await _httpClient.PostAsync(
                route,
                new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                )
            );
            var output = await GetOutPut<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
        {
            var url = PrepareGetRoute(route, queryStringParametersObject);
            var response = await _httpClient.GetAsync(url);
            var output = await GetOutPut<TOutput>(response);

            return (response, output);
        }
        public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(string route) where TOutput : class
        {
            var response = await _httpClient.DeleteAsync(route);
            var output = await GetOutPut<TOutput>(response);

            return (response, output);
        }
        public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(string route, object payload) where TOutput : class
        {
            var response = await _httpClient.PutAsync(
                route,
                new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                )
            );
           var output = await GetOutPut<TOutput>(response);

            return (response, output);
        }

        private async Task<TOutput?> GetOutPut<TOutput>(HttpResponseMessage response) where TOutput  : class
        {
            var outputString = await response.Content.ReadAsStringAsync();

            TOutput? output = null;
            if (!string.IsNullOrWhiteSpace(outputString))
                output = JsonSerializer.Deserialize<TOutput>(outputString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return output;
        }

        private string PrepareGetRoute(string route, object? queryStringParametersObject)
        {
            //if (queryStringParametersObject == null)
            //    return route;
            //var queryString = string.Join("&", queryStringParametersObject.GetType().GetProperties().Select(x => $"{x.Name}={x.GetValue(queryStringParametersObject)}"));
            //return $"{route}?{queryString}";
            if (queryStringParametersObject == null)
                return route;
            var parametersJson = JsonSerializer.Serialize(queryStringParametersObject);
            var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);
            return QueryHelpers.AddQueryString(route, parametersDictionary!);
        }
    }
}
