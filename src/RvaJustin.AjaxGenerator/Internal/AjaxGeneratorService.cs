using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RvaJustin.AjaxGenerator.ObjectModel;

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
            var actions = GetActions(allCollections);

            if (!actions.Any())
            {
                return string.Empty;
            }

            sb.Append(BuildScript(router, actions));
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

        private ControllerAction[] GetActions(IEnumerable<string> collections)
        {
            var actionListRepository =
                (IAjaxEndpointListRepository) serviceProvider.GetService(typeof(IAjaxEndpointListRepository));
            var actions = new List<ControllerAction>();

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

        private string BuildScript(IRouter router, IEnumerable<ControllerAction> actions)
        {
            var agObjectName = Configuration.JavaScriptObjectName;
            var sb = BuildBaseScript(agObjectName);
            var sbDeclarations = new StringBuilder();
            var sbFunctions = new StringBuilder();

            ISet<string> areas = new HashSet<string>();
            ISet<string> controllers = new HashSet<string>();
            ISet<string> methods = new HashSet<string>();

            foreach (var actionMethod in actions)
            {
                // todo: support custom route tokens
                var route = string.IsNullOrEmpty(actionMethod.Area)
                    ? router.GetUrl(actionMethod.Controller, actionMethod.Action)
                    : router.GetUrl(actionMethod.Area, actionMethod.Controller, actionMethod.Action);

                JavaScriptUtilities.CheckReservedWord(actionMethod.Area, "Formatted area");
                JavaScriptUtilities.CheckReservedWord(actionMethod.Controller, "Formatted controller");
                JavaScriptUtilities.CheckReservedWord(actionMethod.Action, "Formatted action");

                AddDeclarations(actionMethod, ref areas, ref controllers, ref sbDeclarations);
                AddFunctions(ref methods, actionMethod, route, ref sbFunctions);
            }

            sb = sb.Replace("//**DECLARATIONS**//", sbDeclarations.ToString());
            sb = sb.Replace("//**FUNCTIONS**//", sbFunctions.ToString());

            return sb.ToString();
        }

        private static void AddFunctions(
            ref ISet<string> methods,
            ControllerAction actionMethod,
            string route,
            ref StringBuilder sbFunctions)
        {
            if (methods.Contains(actionMethod.Id))
            {
                return;
            }

            var functionScript = BuildFunctionScript(actionMethod, route);
            sbFunctions.AppendLine(functionScript);
            methods.Add(actionMethod.Id);
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
            ControllerAction actionMethod,
            ref ISet<string> areas,
            ref ISet<string> controllers,
            ref StringBuilder sbDeclarations)
        {
            if (!string.IsNullOrEmpty(actionMethod.Area) && !areas.Contains(actionMethod.Area))
            {
                sbDeclarations.AppendLine(
                    @$"if (typeof self.{actionMethod.Area} == u) self.{actionMethod.Area} = {{ }};");
                if (!controllers.Contains(actionMethod.Controller))
                {
                    sbDeclarations.AppendLine(
                        @$"if (typeof self.{actionMethod.Area}.{actionMethod.Controller} == u) self.{actionMethod.Area}.{actionMethod.Controller} = {{ }};");
                    areas.Add(actionMethod.Area);
                }
            }

            if (!controllers.Contains(actionMethod.Controller))
            {
                sbDeclarations.AppendLine(
                    @$"if (typeof self.{actionMethod.Controller} == u) self.{actionMethod.Controller} = {{ }};");
                controllers.Add(actionMethod.Controller);
            }
        }

        private static string BuildFunctionScript(ControllerAction actionMethod, string route)
        {
            var parameters = "";
            var data = "";

            foreach (var parameter in actionMethod.Parameters)
            {
                JavaScriptUtilities.CheckReservedWord(parameter, "Literal parameter");
                parameters = parameters + (parameters.Length > 0 ? ", " : "") + parameter;
                data = data + (data.Length > 0 ? ", " : "") + parameter + ":" + parameter;
            }

            var functionName = string.IsNullOrEmpty(actionMethod.Area)
                ? $"self.{actionMethod.Controller}.{actionMethod.Action}"
                : $"self.{actionMethod.Area}.{actionMethod.Controller}.{actionMethod.Action}";

            var functionScript =
                $"{functionName} = function ({parameters}, ignoreErrors, options) " +
                $"{{ return _ajax(" +
                $"'{actionMethod.AjaxBehavior.Methods[0]}', " + //http method
                $"'{route}', " + //url
                $"'{actionMethod.Id}', " + //id
                $"'{actionMethod.Area}', " + // area
                $"'{actionMethod.Controller}', " + // controller
                $"'{actionMethod.Action}', " + // action
                $"{{ {data} }}, " + // parameters as object
                $"ignoreErrors || false, " + // ignore errors flag
                $"options || {{ }}); }};"; // additional options

            return functionScript;
        }
    }
}