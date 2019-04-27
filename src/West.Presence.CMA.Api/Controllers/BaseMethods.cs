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

        protected bool IsResourcesRequestValid(QueryFilter filter, IEnumerable<School> schools, List<int> categories)
        {
            if (!categories.Contains(filter.Categories))
            {
                _validateErrors.Add((int)ValidateErrors.RequestValidationCategoryNotValid);
                //  ValidateErrorMsg = "categories filter in request is must have and in correct categore.";
                return false;
            }

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

            filter.ChannelServerIds = allServerList;
            return true;
        }

        protected Links GetLinks(string baseUrl, QueryFilter filter, QueryPagination page, string query, bool includeChannels, int totalItemNumber, DateTime? startTime = null, DateTime? endTime = null)
        {
            var RequestPath = baseUrl;
            if (Request != null)
            {
                RequestPath += Request.Path;
            }

            int nextOffset = page.Offset + page.Limit;
            int prevOffset = page.Offset - page.Limit;
            Links links = new Links();

            links.prev = $"{RequestPath}?filter[categories]={filter.Categories}&filter[channels]={filter.Channels}&page[offset]={prevOffset}&page[limit]={page.Limit}";
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

            
            links.next = $"{RequestPath}?filter[categories]={filter.Categories}&filter[channels]={filter.Channels}&page[offset]={nextOffset}&page[limit]={page.Limit}";
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
