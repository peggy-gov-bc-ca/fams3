﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SearchApi.Web.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BcGov.Fams3.SearchApi.Contracts.PersonSearch;
using BcGov.Fams3.Redis;
using SearchApi.Web.Search;
using System.Collections.Generic;
using SearchApi.Web.DeepSearch.Schema;
using System.Linq;
using SearchApi.Web.DeepSearch;

namespace SearchApi.Web.Notifications
{

    public class WebHookNotifierSearchEventStatus :  ISearchApiNotifier<PersonSearchAdapterEvent>
    {

        private readonly HttpClient _httpClient;
        private readonly SearchApiOptions _searchApiOptions;
        private readonly IDeepSearchService _deepSearchService;
        private readonly ILogger<WebHookNotifierSearchEventStatus> _logger;
        private readonly ICacheService _cacheService;

        public WebHookNotifierSearchEventStatus(HttpClient httpClient, IOptions<SearchApiOptions> searchApiOptions,
            ILogger<WebHookNotifierSearchEventStatus> logger, ICacheService cacheService, IDeepSearchService deepSearchService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _searchApiOptions = searchApiOptions.Value;
            _cacheService = cacheService;
            _deepSearchService = deepSearchService;
        }

        public async Task NotifyEventAsync(string searchRequestKey, PersonSearchAdapterEvent eventStatus, string eventName,
           CancellationToken cancellationToken)
        {
            var webHookName = "PersonSearch";
                foreach (var webHook in _searchApiOptions.WebHooks)
                {
                    _logger.LogDebug(
                       $"The webHook {webHookName} notification is attempting to send status {eventName} event for {webHook.Name} webhook.");

                    if (!URLHelper.TryCreateUri(webHook.Uri, eventName, $"{searchRequestKey}", out var endpoint))
                    {
                        _logger.LogWarning(
                            $"The webHook {webHookName} notification uri is not established or is not an absolute Uri for {webHook.Name}. Set the WebHook.Uri value on SearchApi.WebHooks settings.");
                        return;
                    }

                    using var request = new HttpRequestMessage();

                    try
                    {
                       StringContent content;
                            if (eventName == EventName.Finalized)
                            {
                                PersonSearchEvent finalizedSearch = new PersonSearchFinalizedEvent()
                                {
                                    SearchRequestKey = eventStatus.SearchRequestKey,
                                    Message = "Search Request Finalized",
                                    SearchRequestId = eventStatus.SearchRequestId,
                                    TimeStamp = DateTime.Now
                                };
                                content = new StringContent(JsonConvert.SerializeObject(finalizedSearch));
                            }
                            else
                            {
                                content = new StringContent(JsonConvert.SerializeObject(eventStatus));
                            }

                            content.Headers.ContentType =
                                System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                            request.Content = content;
                            request.Method = HttpMethod.Post;
                            request.Headers.Accept.Add(
                                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
                            request.RequestUri = endpoint;
                            var response = await _httpClient.SendAsync(request, cancellationToken);

                            if (!response.IsSuccessStatusCode)
                            {
                                _logger.LogError(
                                    $"The webHook {webHookName} notification has not executed status {eventName} successfully for {webHook.Name} webHook. The error code is {response.StatusCode.GetHashCode()}.");
                                return;
                            }

                            _logger.LogInformation(
                                $"The webHook {webHookName} notification has executed status {eventName} successfully for {webHook.Name} webHook.");
                        }
                    
                    catch (Exception exception)
                    {
                        _logger.LogError($"The webHook {webHookName} notification failed for status {eventName} for {webHook.Name} webHook. [{exception.Message}]");
                    }
                }

                  await   _deepSearchService.UpdateDataPartner(searchRequestKey, eventStatus.ProviderProfile.Name, eventName);
                 await _deepSearchService.ProcessWaveSearch(searchRequestKey);
        }

        //private async Task<bool> IsCurrentWaveCompleted(string searchRequestKey, string eventName)
        //{
        //    try
        //    {
        //        if (eventName.Equals(EventName.WaveCompleted))
        //        {
        //            return JsonConvert.SerializeObject(await _cacheService.GetRequest(searchRequestKey)).AllPartnerCompleted();
        //        }
        //        return true;
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Check Data Partner Status Failed. [{eventName}] for {searchRequestKey}. [{exception.Message}]");
        //        return false;
        //    }
        //}

        //private async Task<bool> IsCurrentWaveCompleted(string searchRequestKey)
        //{
        //    try
        //    {
        //           return JsonConvert.SerializeObject(await _cacheService.GetRequest(searchRequestKey)).AllPartnerCompleted();

        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Check Data Partner Status Failed. [] for {searchRequestKey}. [{exception.Message}]");
        //        return false;
        //    }
        //}
        //private async Task<IEnumerable<WaveMetaData>> GetWaveDataForSearch(string searchRequestKey)
        //{
        //    List<WaveMetaData> waveMetaDatas = new List<WaveMetaData>();

        //     var  keys = await  _cacheService.SearchKeys($"*{searchRequestKey}*");

        //    foreach( var key in keys)
        //    {
        //        waveMetaDatas.Add(JsonConvert.DeserializeObject<WaveMetaData>(await _cacheService.Get(key)));
        //    }

        //    return waveMetaDatas.AsEnumerable();
        //}

        //private async void DeleteFromCache(string searchRequestKey, string eventName)
        //{
        //    try
        //    {
        //        if (eventName.Equals(EventName.Finalized))
        //        {
        //            await _cacheService.DeleteRequest(searchRequestKey);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Delete search request failed. [{eventName}] for {searchRequestKey}. [{exception.Message}]");
                
        //    }

        //}


        //private async Task UpdateDataPartner(string searchRequestKey, PersonSearchAdapterEvent eventStatus, string eventName)
        //{
        //    try
        //    {
        //        if (eventName.Equals(EventName.Completed) || eventName.Equals(EventName.Rejected))
        //        {
        //            var searchRequest = JsonConvert.SerializeObject(await _cacheService.GetRequest(searchRequestKey)).UpdateDataPartner(eventStatus.ProviderProfile.Name);
        //            await _cacheService.SaveRequest(searchRequest);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Update Data Partner Status Failed. [{eventName}] for {searchRequestKey}. [{exception.Message}]");
                
        //    }
        //}
    }
}
