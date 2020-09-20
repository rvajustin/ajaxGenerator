using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RvaJustin.AjaxGenerator.Internal
{
    public class AjaxGeneratorService : IAjaxGeneratorService
    {
        private readonly IServiceProvider serviceProvider;
        public IAjaxGeneratorConfiguration Configuration { get; }
        private readonly string coreScript;

        private AjaxGeneratorService(
            IServiceProvider serviceProvider,
            IAjaxGeneratorConfiguration configuration)
        {
            Configuration = configuration;
            
            this.serviceProvider = serviceProvider;
            
            coreScript = Resources.GetResourceString(Resources.Names.Core);
        }

        public static AjaxGeneratorService Build(
            IServiceProvider serviceProvider,
            Action<IAjaxGeneratorConfiguration> configureService)
        {
            var config = new AjaxGeneratorConfiguration();
            configureService.Invoke(config);

            var service = new AjaxGeneratorService(serviceProvider, config);
            return service;
        }

        public string GenerateScript(IRouter router, params string[] collections)
        {
            var sb = new StringBuilder();
            var allCollections = collections.Union(Configuration.GlobalIncludes);
            var ajaxEndpoints = GetAjaxEndpoints(allCollections);

            if (!ajaxEndpoints.Any())
            {
                return string.Empty;
            }

            sb.Append(BuildScript(router, ajaxEndpoints));
            return CompressScript(sb);
        }

        private string CompressScript(StringBuilder sb)
        {
            var scriptCompressor =
                (IScriptCompressor) serviceProvider.GetService(typeof(IScriptCompressor));

            if (!Configuration.CompressScript.HasValue && Debugger.IsAttached ||
                !(Configuration.CompressScript ?? true))
            {
                return sb.ToString();
            }

            return scriptCompressor.Compress(sb.ToString());
        }

        private IAjaxEndpoint[] GetAjaxEndpoints(IEnumerable<string> collections)
        {
            var actionListRepository =
                (IAjaxEndpointListRepository) serviceProvider.GetService(typeof(IAjaxEndpointListRepository));
            var actions = new List<IAjaxEndpoint>();

            foreach (var collection in collections)
            {
                if (!actionListRepository.TryGet(collection, out var actionList))
                {
                    throw new KeyNotFoundException($"The collection named '{collection}' could not be found.");
                }

                // generate collection script
                actions.AddRange(actionList);
            }

            return actions.ToArray();
        }

        private string BuildScript(IRouter router, IEnumerable<IAjaxEndpoint> ajaxEndpoints)
        {
            var agObjectName = Configuration.JavaScriptObjectName;
            var sb = BuildBaseScript(agObjectName);
            var sbDeclarations = new StringBuilder();
            var sbFunctions = new StringBuilder();

            ISet<string> declarations = new HashSet<string>();
            ISet<string> methods = new HashSet<string>();

            foreach (var ajaxEndpoint in ajaxEndpoints)
            {
                string route;
                if (ajaxEndpoint is IRoutableEndpoint routableEndpoint)
                {
                    // todo: support custom route tokens
                    JavaScriptUtilities.CheckReservedWord(routableEndpoint.Area, "Formatted area");
                    JavaScriptUtilities.CheckReservedWord(routableEndpoint.Controller, "Formatted controller");
                    JavaScriptUtilities.CheckReservedWord(routableEndpoint.Action, "Formatted action");
                    
                    route = string.IsNullOrEmpty(routableEndpoint.Area)
                        ? router.GetUrl(routableEndpoint.Controller,routableEndpoint.Action)
                        : router.GetUrl(routableEndpoint.Area, routableEndpoint.Controller, routableEndpoint.Action);
                }
                else
                {
                    foreach (var segment in ajaxEndpoint.Path)
                    {
                        JavaScriptUtilities.CheckReservedWord(segment, "Path segment");
                    }

                    route = ajaxEndpoint.Url;
                }

                AddDeclarations(ajaxEndpoint, ref sbDeclarations, ref declarations);
                AddFunctions(ajaxEndpoint, ref sbFunctions, ref methods, route);
            }

            sb = sb.Replace("//**DECLARATIONS**//", sbDeclarations.ToString());
            sb = sb.Replace("//**FUNCTIONS**//", sbFunctions.ToString());

            return sb.ToString();
        }

        private static void AddFunctions(
            IAjaxEndpoint ajaxEndpoint,
            ref StringBuilder sbFunctions, 
            ref ISet<string> methods,
            string route)
        {
            if (methods.Contains(ajaxEndpoint.Id))
            {
                return;
            }

            var functionScript = BuildFunctionScript(ajaxEndpoint, route);
            sbFunctions.AppendLine(functionScript);
            methods.Add(ajaxEndpoint.Id);
        }

        private StringBuilder BuildBaseScript(string agObjectName)
        {
            var sb = new StringBuilder($"window.{agObjectName}={coreScript}");

            JavaScriptUtilities.CheckReservedWord(agObjectName, "Root object");

            sb = sb.Replace(
                "$ERROR_CALLBACK",
                string.IsNullOrEmpty(Configuration.ErrorCallback) ? "_onError" : Configuration.ErrorCallback);

            return sb;
        }

        private static void AddDeclarations(
            IAjaxEndpoint ajaxEndpoint,
            ref StringBuilder sbDeclarations,
            ref ISet<string> declarations)
        {
            var declaration = string.Empty;
            var queue = new Queue<string>(ajaxEndpoint.Path);

            while (queue.Any())
            {
                var segment = queue.Dequeue();
                declaration += (declaration.Length > 0 ? "." : string.Empty) + segment;

                if (declarations.Contains(declaration) || !queue.Any())
                {
                    continue;
                }

                declarations.Add(declaration);
                sbDeclarations.AppendLine(
                    @$"if (typeof self.{declaration} == u) self.{declaration} = {{ }};");
            }
        }

        private static string BuildFunctionScript(IAjaxEndpoint ajaxEndpoint, string route)
        {
            var parameters = "";
            var data = "";

            foreach (var parameter in ajaxEndpoint.Parameters)
            {
                JavaScriptUtilities.CheckReservedWord(parameter, "Literal parameter");
                parameters = parameters + (parameters.Length > 0 ? ", " : "") + parameter;
                data = data + (data.Length > 0 ? ", " : "") + parameter + ":" + parameter;
            }

            var functionName = "self." + string.Join(".", ajaxEndpoint.Path);
            var bodyParameter = ajaxEndpoint.BodyParameter ?? $"{{ {data} }}";

            if (parameters.Length > 0)
            {
                parameters += ", ";
            }

            var functionScript =
                $"{functionName} = function ({parameters}ignoreErrors, options) " +
                $"{{ return _ajax(" +
                $"'{ajaxEndpoint.Behavior.Method}', " + //http method
                $"'{route}', " + //url
                $"'{ajaxEndpoint.Id}', " + //id
                $"{JsonConvert.SerializeObject(ajaxEndpoint.Path)}, " + // path
                $"{bodyParameter}, " + // parameters as object
                $"ignoreErrors || false, " + // ignore errors flag
                $"options || {{ }}); }}"; // additional options
            
            if (ajaxEndpoint.Metadata != null)
            {
                functionScript +=
                    $"{Environment.NewLine}{functionName}._metadata = {JsonConvert.SerializeObject(ajaxEndpoint.Metadata, Formatting.Indented)}"; // metadata
            }

            return functionScript;
        }
    }
}