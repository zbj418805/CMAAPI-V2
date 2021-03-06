﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;

namespace West.Presence.CMA.Api.Controllers
{
    public class BaseMethods : ControllerBase
    {
        protected List<int> _validateErrors = new List<int>();

        protected bool IsChannelsRequestValid(QueryFilter filter)
        {
            List<int> requestChannels = new List<int>();
            if (!string.IsNullOrEmpty(filter.channels))
            {
                var channels = filter.channels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (channels.Length > 0)
                    return true;
            }

            _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);

            return false;
        }

        protected bool IsResourcesRequestValid(QueryFilter filter, IEnumerable<School> schools, List<int> categories)
        {
            if (!categories.Contains(filter.categories))
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);
                //  ValidateErrorMsg = "categories filter in request is must have and in correct categore.";
                return false;
            }

            //validaing category and channels.
            List<int> requestChannels = new List<int>();
            if (!string.IsNullOrEmpty(filter.channels))
            {
                var channels = filter.channels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int serverId;
                foreach (var s in channels)
                {
                    if (int.TryParse(s, out serverId))
                    {
                        requestChannels.Add(serverId);
                    }
                }
            }
            
            if (schools.Count() == 0)
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationSchoolBaseUrlInvalid);
                // ValidateErrorMsg = "school list is not generated.";
                return false;
            }


            var districtServerId = schools.FirstOrDefault().DistrictServerId;
            if (districtServerId <= 0)
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationDistrictServerIdNotFound);
                //    ValidateErrorMsg = "district server not found.";
                return false;
            }

            var allServerList = (requestChannels.Count > 0) ? requestChannels : schools.Select(x => x.ServerId).Take(5).ToList();

            filter.channelServerIds = allServerList;
            return true;
        }

        protected Links GetLinks(string baseUrl, QueryFilter filter, QueryPagination page, string query, bool includeChannels, int totalItemNumber, DateTime? startTime = null, DateTime? endTime = null)
        {
            var RequestPath = baseUrl.Substring(0, baseUrl.Length-1);
            if (Request != null)
            {
                RequestPath += Request.Path;
            }

            int nextOffset = page.offset + page.limit;
            int prevOffset = page.offset - page.limit;
            Links links = new Links();

            links.prev = $"{RequestPath}?filter[categories]={filter.categories}&filter[channels]={filter.channels}&page[offset]={prevOffset}&page[limit]={page.limit}";
            if (startTime != null)
            {
                links.prev += "&filter[starttime]=" + startTime.Value.ToString("yyyy-MM-dd");
            }
            if (endTime != null)
            {
                links.prev += "&filter[endtime]=" + endTime.Value.ToString("yyyy-MM-dd");
            }
            if (includeChannels)
            {
                links.prev += "&include=channels";
            }
            if (page.offset == 0)
            {
                links.prev = null;
            }
            if (query.Length > 0)
            {
                links.prev += "&query=" + query;
            }

            links.next = $"{RequestPath}?filter[categories]={filter.categories}&filter[channels]={filter.channels}&page[offset]={nextOffset}&page[limit]={page.limit}";
            if (startTime != null)
            {
                links.next += "&filter[starttime]=" + startTime.Value.ToString("yyyy-MM-dd");
            }
            if (endTime != null)
            {
                links.next += "&filter[endtime]=" + endTime.Value.ToString("yyyy-MM-dd");
            }
            if (includeChannels)
            {
                links.next += "&include=channels";
            }
            if (query.Length > 0)
            {
                links.next += "&query=" + query;
            }

            if (page.offset + page.limit >= totalItemNumber)
            {
                links.next = null;
            }
            return links;
        }

        protected string GetBaseUrl(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                return Request == null ? "" : Request.Scheme + "://" + Request.Host.Host + "/";
            else
                return baseUrl.EndsWith('/') ? baseUrl : baseUrl + '/';
        }

        protected string GetSearchKey(string filter, string query)
        {
            return $"{filter}{query}".ToLower().Trim();
        }

        protected QueryFilter GetQueryFilter()
        {
            if (Request != null)
            {
                DateTime sTime;
                DateTime eTime;
                int parent;
                int categories;

                if (!DateTime.TryParse(Request.Query["filter[starttime]"].ToString(), out sTime))
                    sTime = DateTime.Today.AddDays(-10);

                if(!DateTime.TryParse(Request.Query["filter[endtime]"].ToString(), out eTime))
                    eTime = DateTime.Today.AddMonths(12);

                if (!int.TryParse(Request.Query["filter[parent]"].ToString(), out parent))
                    parent = -1;

                if (!int.TryParse(Request.Query["filter[categories]"].ToString(), out categories))
                    categories = 0;

                return new QueryFilter()
                {
                    categories = categories,
                    channels = Request.Query["filter[channels]"].ToString(),
                    starttime = sTime,
                    endtime = eTime,
                    search = Request.Query["filter[search]"].ToString(),
                    parent = parent
                };
            }
            else
                return new QueryFilter();
        }
    }
}
