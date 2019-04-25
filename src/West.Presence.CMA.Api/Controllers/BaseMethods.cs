using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using West.Presence.CMA.Api.Model;
using West.Presence.CMA.Core.Models;
using West.Presence.CMA.Core.Servies;

namespace West.Presence.CMA.Api.Controllers
{
    public class BaseMethods : ControllerBase
    {
        protected List<int> _validateErrors = new List<int>();
        protected ISchoolsService _schoolService;

        protected bool IsChannelsRequestValid(QueryFilter filter)
        {
            List<int> requestChannels = new List<int>();
            if (!string.IsNullOrEmpty(filter.Channels))
            {
                var channels = filter.Channels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (channels.Length > 0)
                    return true;
            }

            _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);

            return false;
        }

        protected bool IsResourcesRequestValid(QueryFilter filter, IEnumerable<School> schools)
        {
            if (filter.Categories == 0)
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);
                //  ValidateErrorMsg = "categories filter in request is must have.";
                return false;
            }
            
            //var category = _categoryService.GetById(filter.Categories);
            //if (category == null || category.id == 0)
            //{
            //    _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);
            //    // ValidateErrorMsg = "categories id is not valid.";
            //    return false;
            //}

            //var matchPattern = string.Format("resources/{0}", category.relationships.schema.data.id);//"school‐messenger.news",
            //if (Request.RequestUri.AbsolutePath.IndexOf(matchPattern, 0, StringComparison.CurrentCultureIgnoreCase) < 0)
            //{
            //    _validateErrors.Add((int)ValidateErrors.RequestValidationApiDoesnotMatchRequestCategory);
            //    //ValidateErrorMsg = "requart api uri does not macth request category type.";
            //    return false;
            //}

            //validaing category and channels.
            List<int> requestChannels = new List<int>();
            if (!string.IsNullOrEmpty(filter.Channels))
            {
                var channels = filter.Channels.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int serverId;
                foreach (var s in channels)
                {
                    if (int.TryParse(s, out serverId))
                    {
                        requestChannels.Add(serverId);
                    }
                }
            }

            //var schoolList = _schoolService.GetSchools(GetBaserUrl(), "");
            if (schools.FirstOrDefault().Total == 0)
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationSchoolBaseUrlInvalid);
                // ValidateErrorMsg = "school website url not valid.";
                return false;
            }


            var districtServerId = schools.Select(x => x.DistrictServerId).First();
            if (districtServerId <= 0)
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationDistrictServerIdNotFound);
                //    ValidateErrorMsg = "district server not found.";
                return false;
            }

            var allServerList = (requestChannels.Count > 0) ? requestChannels : schools.Select(x => x.ServerId).Take(5).ToList();

            //filter.ChannelServerIds = (category.IsRootCategory()) ? allServerList : allServerList.Where(x => x != districtServerId).ToList();
            filter.ChannelServerIds = allServerList;
            return true;
        }

        protected string GetBaserUrl()
        {
            string url = GetQueryString("baseurl");
            //if (url != null && url.EndsWith("/"))
            //    url = url.Substring(0, url.Length - 1);
            return url;
        }

        protected string GetQueryString(string key)
        {
            if (Request == null)
                return null;

            var queryStrings = Request.Query;
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;
            return match.Value;
        }

        protected Links GetLinks(QueryFilter filter, QueryPagination page, string query, bool includeChannels, int totalItemNumber, DateTime? startTime = null, DateTime? endTime = null)
        {
            var baseUrl = GetBaserUrl();

            var RequestPath = baseUrl + Request.QueryString;

            int nextOffset = page.Offset + page.Limit;
            int prevOffset = page.Offset - page.Limit;
            Links links = new Links();

            links.prev = string.Format("{0}?filter[categories]={1}&filter[channels]={2}&page[offset]={3}&page[limit]={4}",
               RequestPath, filter.Categories, filter.Channels, prevOffset, page.Limit);
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
            if (page.Offset == 0)
            {
                links.prev = null;
            }
            if (query.Length > 0)
            {
                links.prev += "&query=" + query;
            }


            links.next = string.Format("{0}?filter[categories]={1}&filter[channels]={2}&page[offset]={3}&page[limit]={4}",
                   RequestPath, filter.Categories, filter.Channels, nextOffset, page.Limit);
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

            if (page.Offset + page.Limit >= totalItemNumber)
            {
                links.next = null;
            }
            return links;
        }
    }
}
