
namespace MonsterTradingCardGame_Hoechtl.Handler
{

    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Handler.PremissionAttributes;
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using MonsterTradingCardGame_Hoechtl.Models;
    using Newtonsoft.Json;
    using System.Reflection;

    internal class HandlerMethodResolver
    {
        public HandlerMethodResolver(SessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public HttpResponse InvokeHandlerMethod(IHandler handler, HttpMethod httpMethod, string[] pathData, string jsonContent, string sessionKey)
        {
            string methodName = GetMethodNameFromMethodPath(pathData);
            Type implentation = handler.GetType();

            foreach (MethodInfo methodInfo in implentation.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                HttpAttribute attr = methodInfo.GetCustomAttributes(typeof(HttpAttribute), true).FirstOrDefault() as HttpAttribute;
                if (attr == null)
                {
                    // Method without a HttpAttribute
                    continue;
                }

                if (attr.Method == httpMethod &&
                   methodName.Equals(methodInfo.Name, StringComparison.Ordinal))
                {
                    // Check if requester is authenticated
                    PermissionAttribute permissionAttribute = methodInfo.GetCustomAttributes(typeof(PermissionAttribute), true).FirstOrDefault() as PermissionAttribute;
                    if (sessionService.HasPermission(sessionKey, permissionAttribute) == false)
                    {
                        return GetUnauthorizedResponse();
                    }

                    // create parameter list
                    SessionContext context = sessionService.CreateSessionContext(sessionKey);
                    List<object> parameterCollection = new() { context };
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    if (parameters.Length > 1)
                    {
                        parameterCollection.Add(ParseParameter(parameters[1], jsonContent));
                    }

                    try
                    {
                        object returnValue = methodInfo.Invoke(handler, parameterCollection.ToArray());

                        if (returnValue is not HttpResponse)
                        {
                            throw new Exception("Method was excecuted succsessfully but the return vlaue is not a HttpResponse.");
                        }

                        return returnValue as HttpResponse;
                    }
                    catch (Exception ex)
                    {
                        return GetInternalServerErrorResponse();
                    }
                }
            }

            return GetNotFoundResponse();
        }

        private object ParseParameter(ParameterInfo firstParameter, string jsonContent)
        {
            if(firstParameter == null)
            {
                return Array.Empty<object>();
            }
            else
            {
                Type parameterType = firstParameter.ParameterType;
                object parsedParameter = JsonConvert.DeserializeObject(jsonContent, parameterType);
                return parsedParameter;
            }
        }

        private string GetMethodNameFromMethodPath(string[] pathData)
        {
            return pathData.Skip(1).First();
        }

        private HttpResponse GetUnauthorizedResponse()
        {
            return new HttpResponse(401, "Unauthorized", string.Empty);
        }

        private HttpResponse GetNotFoundResponse()
        {
            return new HttpResponse(404, "Not Found");
        }

        private HttpResponse GetInternalServerErrorResponse()
        {
            return new HttpResponse(500, "Internal Server Error");
        }

        private readonly SessionService sessionService;
    }
}
