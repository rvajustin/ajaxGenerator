using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using RvaJustin.AjaxGenerator.OpenApi.ObjectModel;

namespace RvaJustin.AjaxGenerator.OpenApi.Services
{
    internal static class OpenApiEndpointBuilder
    {

        private static  readonly char[] splitters = {'/', '\\'};
        private static readonly HttpMethod patchHttpMethod = new HttpMethod("patch");

        public static string UpperFirst(string input) 
            => input?.Length > 1 ? $"{input[0]}".ToUpper() + input.Substring(1) : input?.ToUpper();
        
        public static string LowerFirst(string input) 
            => input?.Length > 1 ? $"{input[0]}".ToLower() + input.Substring(1) : input?.ToLower();

        public static bool TryBuild(
            string collection,
            HostString host,
            OpenApiServer server,
            string endpointUrl,
            OpenApiPathItem activity,
            out IEnumerable<OpenApiEndpoint> endpoints)
        {
            var list = new List<OpenApiEndpoint>();
            endpoints = list;
            var operations = GetOrderedOperations(activity).ToArray();

            foreach (var apiOperation in operations)
            {
                var method = GetHttpMethod(apiOperation.Key);
                var behavior = new EndpointBehavior(method, collection);
                var path = GetPathSegments(endpointUrl, apiOperation.Value.Parameters).ToArray();
                path[path.Length - 1] = apiOperation.Key.ToString().ToLower() + UpperFirst(path[path.Length - 1]);
                var parameters = GetParameters(apiOperation.Value.Parameters, apiOperation.Value.RequestBody,
                    out var bodyParameter).ToArray();

                var endpoint = new OpenApiEndpoint(
                    Guid.NewGuid().ToString(),
                    $"{host}{server.Url}{endpointUrl}",
                    null,
                    behavior,
                    path,
                    parameters,
                    bodyParameter);

                list.Add(endpoint);
            }

            return true;
        }

        private static IOrderedEnumerable<KeyValuePair<OperationType, OpenApiOperation>> GetOrderedOperations(
            OpenApiPathItem activity)
        {
            return activity.Operations.OrderBy(o => (int) o.Key);
        }

        private static IEnumerable<string> GetParameters(IEnumerable<OpenApiParameter> parameters,
            OpenApiRequestBody requestBody, out string bodyParameter)
        {
            bodyParameter = null;
            var endpointParameters = new List<string>();
            foreach (var parameter in parameters)
            {
                if (parameter.In == ParameterLocation.Query || parameter.In == ParameterLocation.Path)
                {
                    endpointParameters.Add(parameter.Name);
                }
            }

            if (requestBody?.Content != null)
            {
                bodyParameter = requestBody.Content?.Values?.FirstOrDefault()?.Schema?.Title
                                ?? requestBody.Content?.Values?.FirstOrDefault()?.Schema?.Reference?.Id
                                ?? "body";
                bodyParameter = LowerFirst(bodyParameter);

                endpointParameters.Add(bodyParameter);
            }

            return endpointParameters;
        }

        private static IEnumerable<string> GetPathSegments(
            string url, 
            IList<OpenApiParameter> parameters)
        {
            var path = url.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
            var hasUrlParameters = parameters.Any(p => p.In == ParameterLocation.Path);
            foreach (var segment in path)
            {
                if (hasUrlParameters && parameters.Any(p=> segment.Contains($"{{{p.Name}}}")) )
                {
                    continue;
                }
                yield return segment;
            }
        }

        private static HttpMethod GetHttpMethod(OperationType apiOperationKey)
        {
            switch (apiOperationKey)
            {
                case OperationType.Delete:
                    return HttpMethod.Delete;
                case OperationType.Get:
                    return HttpMethod.Get;
                case OperationType.Head:
                    return HttpMethod.Head;
                case OperationType.Options:
                    return HttpMethod.Options;
                case OperationType.Patch:
                    return patchHttpMethod;
                case OperationType.Post:
                    return HttpMethod.Post;
                case OperationType.Put:
                    return HttpMethod.Put;
                case OperationType.Trace:
                    return HttpMethod.Trace;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiOperationKey), apiOperationKey, null);
            }
        }
    }
}