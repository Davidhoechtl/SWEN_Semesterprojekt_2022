
namespace MonsterTradingCardGame_Hoechtl.Handler
{
    using MonsterTradingCardGame_Hoechtl.Handler.HttpAttributes;
    using MonsterTradingCardGame_Hoechtl.Infrastructure;
    using MonsterTradingCardGame_Hoechtl.Models;
    using Newtonsoft.Json;
    using System.Reflection;

    internal class HandlerMethodResolver
    {
        public HandlerMethodResolver(SessionService sessionService, CardJsonConverter cardJsonConverter)
        {
            this.sessionService = sessionService;
            this.cardJsonConverter = cardJsonConverter;
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
                        return HttpResponse.GetInternalServerErrorResponse();
                    }
                }
            }

            return HttpResponse.GetNotFoundResponse();
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
                object parsedParameter = JsonConvert.DeserializeObject(jsonContent, parameterType, cardJsonConverter);
                return parsedParameter;
            }
        }

        private string GetMethodNameFromMethodPath(string[] pathData)
        {
            return pathData.Skip(1).First();
        }

        private readonly SessionService sessionService;
        private readonly CardJsonConverter cardJsonConverter;
    }
}
